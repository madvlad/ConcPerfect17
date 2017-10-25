﻿// original by Eric Haines (Eric5h5)
// adapted by @torahhorse
// http://wiki.unity3d.com/index.php/FPSWalkerEnhanced

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonDrifter : NetworkBehaviour
{
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;

    // If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster
    private bool limitDiagonalSpeed = true;

    public bool enableRunning = false;

    public float jumpSpeed = 4.0f;
    public float gravity = 10.0f;

    // Model animations
    public AnimationClip walkAnimation;
    public AnimationClip idleAnimation;
    public AnimationClip fallAnimation;

    // Player Model
    public GameObject playerModel;
    public GameObject playerModelRenderer;

    public AudioClip jumpSoundClip;

    // Units that player can fall before a falling damage function is run. To disable, type "infinity" in the inspector
    private float fallingDamageThreshold = 10.0f;

    // If the player ends up on a slope which is at least the Slope Limit as set on the character controller, then he will slide down
    public bool slideWhenOverSlopeLimit = false;

    // If checked and the player is on an object tagged "Slide", he will slide down it regardless of the slope limit
    public bool slideOnTaggedObjects = false;

    public float slideSpeed = 5.0f;

    // If checked, then the player can change direction while in the air
    public bool airControl = true;

    // Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
    public float antiBumpFactor = .75f;

    // Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping
    public int antiBunnyHopFactor = 1;

    public Vector3 moveDirection = Vector3.zero;
    public bool grounded = false;
    public bool underwater = false;
    private bool escaped = false;
    private bool frozen = false;
    private CharacterController controller;
    private Transform myTransform;

    private float speed;
    private RaycastHit hit;
    private float fallStartLevel;
    private bool falling;
    private float slideLimit;
    private float rayDistance;
    private Vector3 contactPoint;
    private bool playerControl = false;
    private int jumpTimer;
	private Camera playerCam;

    private Vector3 lastPosition;
    private float animTimer;

    void Awake() {
		playerCam = GetComponentInChildren<Camera>();
		playerCam.gameObject.SetActive(false);
    }

    public override void OnStartLocalPlayer()
    {
		playerCam.gameObject.SetActive(true);
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;
        rayDistance = controller.height * .5f + controller.radius;
        slideLimit = controller.slopeLimit - .1f;
        jumpTimer = antiBunnyHopFactor;

        if (ApplicationManager.GameType != GameTypes.ConcminationGameType)
        {
            playerModelRenderer.GetComponent<SkinnedMeshRenderer>().material = GameObject.FindGameObjectWithTag("PlayerSkins").GetComponent<PlayerSkinSelectBehavior>().playerSkins[ApplicationManager.PlayerModel];
            GetLocalPlayerObject().GetComponent<LocalPlayerStats>().RequestPlayerSkins();
        }
    }

    void Start()
    {
        if (isLocalPlayer)
            return;

        GetComponent<CharacterController>().enabled = false;
    }

    void Update()
    {
        if (gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;

        var currentPosition = gameObject.transform.position;
        var remoteController = GetComponent<CharacterController>();
        var remoteGrounded = Physics.Raycast(currentPosition, -gameObject.transform.up, 1.5f);

        if (!remoteGrounded && animTimer <= 0)
        {
            playerModel.GetComponent<Animation>().Play(fallAnimation.name, PlayMode.StopAll);
            animTimer = 0.1f;
        }
        else if (currentPosition != lastPosition && animTimer <= 0)
        {
            playerModel.GetComponent<Animation>().Play(walkAnimation.name, PlayMode.StopAll);
            lastPosition = currentPosition;
            animTimer = 0.1f;
        }
        else if (animTimer <= 0)
        {
            playerModel.GetComponent<Animation>().Play(idleAnimation.name, PlayMode.StopAll);
        }

        animTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (escaped || frozen)
        {
            inputX = 0;
            inputY = 0;
        }

        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;

        if (grounded)
        {
            if (inputX != 0 || inputY != 0)
            {
                playerModel.GetComponent<Animation>().Play(walkAnimation.name, PlayMode.StopAll);
            }
            else
            {
                playerModel.GetComponent<Animation>().Play(idleAnimation.name);
            }

            bool sliding = false;
            // See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
            // because that interferes with step climbing amongst other annoyances
            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
            {
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    sliding = true;
            }
            // However, just raycasting straight down from the center can fail when on steep slopes
            // So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
            else
            {
                Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    sliding = true;
            }

            // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
            if (falling)
            {
                falling = false;
                if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
                    FallingDamageAlert(fallStartLevel - myTransform.position.y);
            }

            if (enableRunning)
            {
                speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
            }

            // If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
            if ((sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide"))
            {
                Vector3 hitNormal = hit.normal;
                moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
                moveDirection *= slideSpeed;
                playerControl = false;
            }
            // Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
            else
            {
                moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
                moveDirection = myTransform.TransformDirection(moveDirection) * speed;
                playerControl = true;
            }

            // Jump! But only if the jump button has been released and player has been grounded for a given number of frames
            if (!Input.GetButton("Jump"))
            {
                jumpTimer++;
            }
            else if (jumpTimer >= antiBunnyHopFactor && !frozen && !escaped)
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(jumpSoundClip, ApplicationManager.sfxVolume);
                moveDirection.y = jumpSpeed;
                jumpTimer = 0;
            }
        }
        else
        {
            playerModel.GetComponent<Animation>().Play(fallAnimation.name);
            // If we stepped over a cliff or something, set the height at which we started falling
            if (!falling)
            {
                falling = true;
                fallStartLevel = myTransform.position.y;
            }

            // If air control is allowed, check movement but don't touch the y component
            if (airControl && playerControl)
            {
                moveDirection.x = inputX * speed * inputModifyFactor;
                moveDirection.z = inputY * speed * inputModifyFactor;
                moveDirection = myTransform.TransformDirection(moveDirection);
            }
        }

        if (underwater)
        {
            moveDirection.x = inputX * speed * inputModifyFactor;
            moveDirection.z = inputY * speed * inputModifyFactor * Math.Abs(playerCam.transform.forward.z);
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = ((inputY * playerCam.transform.forward.y) * speed / 4) - 0.16f;
            }
            moveDirection = myTransform.TransformDirection(moveDirection);
        }
        else
        {
            // Apply gravity
            moveDirection.y -= (gravity / 2) * Time.deltaTime;
        }
        // Move the controller, and set grounded true or false depending on whether we're standing on something
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    // Store point that we're in contact with for use in FixedUpdate if needed
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
		if (!isLocalPlayer)
			return;
        contactPoint = hit.point;
    }

    // If falling damage occured, this is the place to do something about it. You can make the player
    // have hitpoints and remove some of them based on the distance fallen, add sound effects, etc.
    void FallingDamageAlert(float fallDistance)
    {
		if (!isLocalPlayer)
			return; 
    }

    public void SetEscaped(bool isEscaped)
    {
        escaped = isEscaped;
    }

    public bool IsEscaped()
    {
        return escaped;
    }

    public void ResetConcminationAssets()
    {
        CmdResetConcminationAssets();
    }

    [Command]
    public void CmdResetConcminationAssets()
    {
        var recipients = GameObject.FindGameObjectsWithTag("Player");

        foreach (var recipient in recipients)
        {
            recipient.GetComponent<FirstPersonDrifter>().RpcResetConcminationAssets();
        }
    }

    [ClientRpc]
    public void RpcResetConcminationAssets()
    {
        var beaconManager = GameObject.FindGameObjectWithTag("BeaconManager").GetComponent<BeaconManager>();
        beaconManager.ResetBeacons();
        var concminationStarter = GameObject.FindGameObjectWithTag("RaceStart").GetComponent<ConcminationStarter>();
        concminationStarter.ResetStarter();
        var raceStarter = GameObject.FindGameObjectWithTag("RaceStart").GetComponent<RaceStarter>();
        raceStarter.RestartGate();
        var gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        gameStateManager.ResetTimer();
    }

    [Command]
    public void CmdFreezeAll(bool freeze)
    {
        var recipients = GameObject.FindGameObjectsWithTag("Player");

        foreach (var recipient in recipients)
        {
            recipient.GetComponent<FirstPersonDrifter>().RpcFreezeMe(freeze);
        }
    }

    [ClientRpc]
    public void RpcFreezeMe(bool isFrozen)
    {
        Unfreeze(isFrozen);
    }

    private void Unfreeze(bool freeze)
    {
        if (!isLocalPlayer)
            return;

        frozen = freeze;

        if (!freeze)
            RestartRun();
    }

    public void RestartRun()
    {
        var gameStateManager = GameObject.FindGameObjectWithTag("GameManager");
        gameStateManager.GetComponent<GameStateManager>().SetIsCourseComplete(false);
        var player = GetLocalPlayerObject();
        player.GetComponent<LocalPlayerStats>().UpdateStatus("Not Started");
        player.transform.position = new Vector3(0, 2, 0);
        player.GetComponent<Concer>().SetConcCount(0);
        var gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        gameManager.SetTimerIsRunning(false);
        gameManager.ResetTimer();
        gameManager.SetJumpNumber(0);
        gameManager.TimerHUDElement.GetComponent<Text>().text = "00:00:00";
        gameManager.JumpHUDElement.GetComponent<Text>().text = "...";
        gameManager.JumpNameHUDElement.GetComponent<Text>().text = "";

        var jumpSeparators = GameObject.FindGameObjectsWithTag("JumpSeparator");
        var startTrigger = GameObject.FindGameObjectWithTag("TimerTriggerOn");
        var startTriggerLabel = GameObject.FindGameObjectWithTag("TimerTriggerOn").GetComponent<SetTimerOnTrigger>().startLabel;

        startTrigger.GetComponent<MeshRenderer>().enabled = true;
        startTriggerLabel.GetComponent<MeshRenderer>().enabled = true;

        foreach (var jumpSeparator in jumpSeparators)
        {
            jumpSeparator.GetComponent<JumpTrigger>().UnsetTrigger();
        }

        var RaceStarter = GameObject.FindGameObjectWithTag("RaceStart").GetComponent<RaceStarter>();
        RaceStarter.RestartGate();

        var music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();

        if (!music.isPlaying)
        {
            music.Play();
        }
    }

    public void TeleportToConcminationOrigin()
    {
        var player = GetLocalPlayerObject();
        var restartPoint = GameObject.FindGameObjectWithTag("ConcminationRespawn");
        player.transform.position = restartPoint.transform.position;
    }

    internal bool IsFrozen()
    {
        return frozen;
    }

    private GameObject GetLocalPlayerObject()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject playerObject = null;
        foreach (GameObject obj in playerObjects)
        {
            if (obj.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerObject = obj;
            }
        }

        return playerObject;
    }
}