using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float rotSpeed;
    public int hp;
    public bool shieldOn;

    public float amplitude = 1.5f;  // Amplitude of the sine wave
    public float frequency = 1.5f;  // Frequency of the sine wave

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 20.0f;
        hp = 100;
        shieldOn = true;
        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        float yPos = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - startTime));

        transform.position = new Vector3(transform.position.x, yPos + 5, transform.position.z);
        transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);
    }
}
