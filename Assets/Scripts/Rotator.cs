using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    readonly float rotationSpeed = 180.0f;

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
    }
}