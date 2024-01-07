using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider shieldSlider;
    public Slider easehealthSlider;
    public float health;
    public float shield;
    private float lerpSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("PARENT: " + gameObject.transform.parent.gameObject.transform.parent.tag);
        if (gameObject.transform.parent.gameObject.transform.parent.tag == "Golem") {
            //Debug.Log("Entramos");
            shield = 0;
            health = 1000;
        }
        else {
        shield = 100;
        health = 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((healthSlider != null) + " " + (easehealthSlider != null));
        if (healthSlider != null && easehealthSlider != null && shieldSlider != null) {
            if (healthSlider.value != health) { healthSlider.value = health; }

            if (shieldSlider.value != shield) { shieldSlider.value = shield; }

            if (easehealthSlider.value != healthSlider.value) { easehealthSlider.value = Mathf.Lerp(easehealthSlider.value, health, lerpSpeed); }
        }
    }
}
