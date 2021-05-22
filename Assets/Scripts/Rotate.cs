using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateRate;

    private Vector3 rotateVector;

    private void Start()
    {
        rotateVector = new Vector3(0, rotateRate, 0);
    }

    void Update()
    {
        transform.Rotate(rotateVector, Space.World);
    }
}
