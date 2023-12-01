using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public float rotSpeed = 60f;
    public Player player; // Player is the script attached to the Player object

    // Start is called before the first frame update
    void Start()
    {
        if (player == null){
            player = GameObject.FindObjectOfType<Player>();
        }

    }


    // Update is called once per frame
    void Update()
    {
        if ((player.canRotateLeft) && Input.GetKey(KeyCode.LeftArrow)){
            // Rotate everything around the Y axis
            transform.Rotate(Vector3.up, - rotSpeed * Time.deltaTime);
            player.canRotateRight = true;
            //Debug.Log("turning");
        }
        else if ((player.canRotateRight) && Input.GetKey(KeyCode.RightArrow)){
            // Rotate CCW around Y-axis
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
            player.canRotateLeft = true;
        }

        if (Input.GetKey(KeyCode.UpArrow) && player.isGrounded){
            player.jump();
        }

    }
}
