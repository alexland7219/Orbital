using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilboard : MonoBehaviour
{
    public Transform cam;

    // LateUpdate is called once per frame at the end
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
