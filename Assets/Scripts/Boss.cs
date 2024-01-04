using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour

{
    public GameObject rockObject;
    private GameObject hand;
    private GameObject rock;
    private bool throwing;
    private float rotSpeed;
    private float oldrotSpeed;
    public bool turning;
    private float dirTimer;
    private float turnTimer;
    private float targetangle;
    private float r;
    private float lerpSpeed = 0.001f;
    private float angle;


    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.Find("RockPoint");
        rotSpeed = 15.0f;
        dirTimer = 0;
        turnTimer = 2.0f;
        turning = false;
        angle = 0f; 
    }

    // Update is called once per frame
    void Update()
    {
        /*if (dirTimer > 5.0f && !turning)
        {
            oldrotSpeed = rotSpeed;
            rotSpeed = 0;
            turning = true;
            targetangle = -transform.rotation.y;
        }*/
        if (turning) {
            transform.Rotate(0, 0.5f, 0);
            angle += 0.5f;
            Debug.Log(angle);
            if (angle == 180) {
                turning = false;
                dirTimer = 0;
                rotSpeed = -oldrotSpeed;
                angle = 0;
            }
        }
        if (!turning)
        {
            //transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);
            dirTimer += Time.deltaTime;
        }
    }

    void throwRock ()
    {
        Debug.Log("Rock thrown");
        Rock script = rock.GetComponent<Rock>();
        script.release();
    }

    void pickupRock() {
        Debug.Log("Creating rock");
        rock = Instantiate(rockObject, hand.transform.position, hand.transform.rotation);
        rock.transform.localScale = new Vector3(0.0065f, 0.0065f, 0.0065f);
        rock.transform.SetParent(hand.transform);
    }

    //void activateWalk() { }
}
