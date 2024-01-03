using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    Rigidbody body;
    GameObject hand;
    GameObject player;
    private bool released;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        hand = GameObject.Find("RockPoint");
        player = GameObject.Find("T-Pose");
        released = false;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void release() { 
        transform.parent = null;
        //body.useGravity = true;
        Vector3 dir = ((player.transform.position) - transform.position);
        dir.Normalize();
        Debug.Log(dir);
        body.AddForce(dir * 10000);
    }
}
