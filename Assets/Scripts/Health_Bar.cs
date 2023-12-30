using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easehealthSlider;
    public float health;
    private float lerpSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((healthSlider != null) + " " + (easehealthSlider != null));
        if (healthSlider != null && easehealthSlider != null) {
            if (healthSlider.value != health) { healthSlider.value = health; }

            if (easehealthSlider.value != healthSlider.value) { easehealthSlider.value = Mathf.Lerp(easehealthSlider.value, health, lerpSpeed); }
        }
    }
}
