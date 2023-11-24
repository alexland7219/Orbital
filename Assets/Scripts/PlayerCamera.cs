using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float rotSpeed = 60f;
    private bool canRotate = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate){

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Floor"){
            Debug.Log("colisión");
            canRotate = false;
        }
    }
}
