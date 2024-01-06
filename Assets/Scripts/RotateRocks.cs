using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRocks : MonoBehaviour
{
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, angle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
