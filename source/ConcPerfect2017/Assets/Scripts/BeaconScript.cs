using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BeaconScript : MonoBehaviour
{
    public List<Material> TeamMaterials;
    public List<GameObject> LockedBeacons;
    public string BeaconName = "Conc Beacon";
    public float SafeTimerSeconds = 15.0f;

    private bool BeaconOn = false;

    // Use this for initialization
    void Start()
    {
        GetComponentInParent<BeaconMarker>().CurrentTimer = 0.0f;
        LockedBeacons[0] = Instantiate(LockedBeacons[0], gameObject.transform);
        LockedBeacons[1] = Instantiate(LockedBeacons[1], gameObject.transform);
        LockedBeacons[2] = Instantiate(LockedBeacons[2], gameObject.transform);
        LockedBeacons[3] = Instantiate(LockedBeacons[3], gameObject.transform);
        LockedBeacons[0].SetActive(false);
        LockedBeacons[1].SetActive(false);
        LockedBeacons[2].SetActive(false);
        LockedBeacons[3].SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponentInParent<BeaconMarker>().CurrentTimer > 0.0f)
        {
            GetComponentInParent<BeaconMarker>().CurrentTimer -= Time.deltaTime;
        }

        ChangeBeaconMaterial();
        ChangeBeaconLockedParticle();
    }

    private void OnTriggerExit(Collider other)
    {
        var concer = other.gameObject.GetComponent<Concer>();
        if (concer != null)
        {
            var colliderTeam = concer.CurrentTeam;

            if (colliderTeam.Equals(GetComponentInParent<BeaconMarker>().OwnedByTeam))
                GetComponentInParent<BeaconMarker>().Guarded = false;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        var concer = collider.gameObject.GetComponent<Concer>();
        if (concer != null)
        {
            var colliderTeam = concer.CurrentTeam;

            if (colliderTeam.Equals(GetComponentInParent<BeaconMarker>().OwnedByTeam))
                GetComponentInParent<BeaconMarker>().Guarded = true;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.gameObject.GetComponent<NetworkIdentity>().netId == ApplicationManager.GetLocalPlayerObject().GetComponent<NetworkIdentity>().netId)
            {
                var colliderTeam = collider.gameObject.GetComponent<Concer>().CurrentTeam;

                if (GetComponentInParent<BeaconMarker>().CurrentTimer <= 0.0f && !GetComponentInParent<BeaconMarker>().Guarded)
                {
                    if (!colliderTeam.Equals(GetComponentInParent<BeaconMarker>().OwnedByTeam))
                    {
                        GetComponentInParent<BeaconMarker>().OwnedByTeam = collider.gameObject.GetComponent<Concer>().CurrentTeam;
                        GetComponentInParent<BeaconMarker>().CurrentTimer = SafeTimerSeconds;
                        Debug.Log("Now owned by team " + GetComponentInParent<BeaconMarker>().OwnedByTeam);

                        GetComponentInParent<BeaconMarker>().LastCapturer = ApplicationManager.Nickname;
                        GameObject localPlayer = ApplicationManager.GetLocalPlayerObject();
                        GetComponentInParent<BeaconMarker>().LastCapturerNetId = localPlayer.GetComponent<NetworkIdentity>().netId;
                        localPlayer.GetComponent<LocalPlayerStats>().UpdateCapturedBeacons(localPlayer.GetComponent<Concer>().CurrentTeam, ApplicationManager.Nickname, localPlayer.GetComponent<NetworkIdentity>().netId, GetComponentInParent<NetworkIdentity>().netId, SafeTimerSeconds);

                        collider.gameObject.GetComponent<MultiplayerChatScript>().SendBeaconCaptureMessage(BeaconName);
                        gameObject.GetComponent<AudioSource>().volume = ApplicationManager.sfxVolume;
                        gameObject.GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
    }

    public void ChangeBeaconMaterial()
    {
        switch (GetComponentInParent<BeaconMarker>().OwnedByTeam)
        {
            case "Red Rangers":
                gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[1];
                break;
            case "Green Gorillas":
                gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[2];
                break;
            case "Blue Bandits":
                gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[3];
                break;
            case "Yellow Yahoos":
                gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[4];
                break;
            default:
                gameObject.GetComponent<MeshRenderer>().material = TeamMaterials[0];
                break;

        }
    }

    public void ChangeBeaconLockedParticle()
    {
        if (GetComponentInParent<BeaconMarker>().Guarded || GetComponentInParent<BeaconMarker>().CurrentTimer > 0)
        {
            BeaconOn = true;
            switch (GetComponentInParent<BeaconMarker>().OwnedByTeam)
            {
                case "Red Rangers":
                    LockedBeacons[1].SetActive(true);
                    break;
                case "Green Gorillas":
                    LockedBeacons[2].SetActive(true);
                    break;
                case "Blue Bandits":
                    LockedBeacons[0].SetActive(true);
                    break;
                case "Yellow Yahoos":
                    LockedBeacons[3].SetActive(true);
                    break;
                default:
                    break;
            }
        }
        else if (BeaconOn)
        {
            LockedBeacons[0].SetActive(false);
            LockedBeacons[1].SetActive(false);
            LockedBeacons[2].SetActive(false);
            LockedBeacons[3].SetActive(false);
            BeaconOn = false;
        }
    }
}
