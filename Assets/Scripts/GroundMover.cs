using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMover : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;
    private readonly float repeatGroundDevisor = 2;

    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.z * transform.localScale.z / repeatGroundDevisor;
    }

    void Update()
    {
        if (GameManager.IsGameActive)
        {
            Move();
        }
    }

    void Move()
    {
        if (transform.position.z < startPos.z - repeatWidth)
        {
            transform.position = startPos;
        }

        transform.Translate(Vector3.back * GameManager.CharacterSpeed * Time.deltaTime);
    }
}

