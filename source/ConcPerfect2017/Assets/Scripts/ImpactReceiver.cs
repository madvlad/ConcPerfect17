﻿using UnityEngine;
using System.Collections;

public class ImpactReceiver : MonoBehaviour
{
    float mass = 3.0f;
    Vector3 impact = Vector3.zero;
    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        impact += dir.normalized * force / mass;
    }

    void Update()
    {
        if (impact.magnitude > 0.2)
        {
            character.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, 4 * Time.deltaTime);
        }
    }
}