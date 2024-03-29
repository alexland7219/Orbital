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
    public bool hasTemporizer;
    public float timeToRefill;
    //private Renderer objectRenderer;

    void Start()
    {
        initialHeight = transform.position.y + 0.2f;
        //objectRenderer = GetComponent<Renderer>();

        initialPos = transform.position;
        timeToRefill = 0.0f;
        rotSpeed   = 50.0f;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {   
        if (hasTemporizer && timeToRefill > 0.0f) timeToRefill -= Time.deltaTime;
        else if (hasTemporizer) SetActiveToAllChildren(true);


        Quaternion parentRotation = transform.parent.rotation;

        // Rotate around the Y-axis of the parent
        transform.RotateAround(transform.parent.position, parentRotation * Vector3.up, rotSpeed * Time.deltaTime);  

        float yPos = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - startTime));

        transform.position = new Vector3(transform.position.x, yPos + initialHeight, transform.position.z);
    }

    public bool getIsPermanent(){ return hasTemporizer; }
    public bool getCanTake(){ return !hasTemporizer || timeToRefill <= 0;}
    public void restartTempo(){ 
        timeToRefill = 20.0f; 
        SetActiveToAllChildren(false);
    }

    void SetActiveToAllChildren(bool k)
    {
        // Iterate through all children
        foreach (Transform child in transform)
        {
            // Deactivate the child GameObject
            child.gameObject.SetActive(k);
        }
    }
}
