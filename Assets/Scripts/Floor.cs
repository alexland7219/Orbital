using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public float rotSpeed = 80f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
            // Rotate everything around the Y axis
            transform.Rotate(Vector3.up, - rotSpeed * Time.deltaTime);
            //Debug.Log("turning");
        }
        else if (Input.GetKey(KeyCode.LeftArrow)){
            // Rotate CCW around Y-axis
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        }
    }
}
