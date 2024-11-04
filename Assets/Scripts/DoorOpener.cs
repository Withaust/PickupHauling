using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField]
    GameObject TargetRotation;

    [SerializeField]
    [Range(0.001f, 1.0f)]
    public float Speed = 0.02f;

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation.transform.rotation, Speed * Time.deltaTime);
    }
}
