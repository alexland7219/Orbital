using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingGun : MonoBehaviour
{
    private Vector3 initialPos;
    private float rotSpeed;
    private float angularSpeed;
    private float startTime;
    private float amplitude = 0.5f;  // Amplitude of the sine wave
    private float frequency = 0.5f;  // Frequency of the sine wave
    public float initialHeight;

    void Start()
    {
        initialHeight = transform.position.y;

        initialPos = transform.position;
        rotSpeed   = 50.0f;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {   
        Quaternion parentRotation = transform.parent.rotation;

        // Rotate around the Y-axis of the parent
        transform.RotateAround(transform.parent.position, parentRotation * Vector3.up, rotSpeed * Time.deltaTime);  

        float yPos = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - startTime));

        transform.position = new Vector3(transform.position.x, yPos + 1.5f*initialHeight, transform.position.z);
    }

}
