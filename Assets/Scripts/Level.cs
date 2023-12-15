using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private float rotSpeed = 40f;
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
        if (player.dirToGo == "left") { transform.Rotate(Vector3.up, -rotSpeed/10.0f * Time.deltaTime); }
        else if (player.dirToGo == "right") { transform.Rotate(Vector3.up, rotSpeed/10.0f * Time.deltaTime); }
        if (player.canRotateLeft && Input.GetKey(KeyCode.LeftArrow) && !player.onElevator){
            // Rotate everything around the Y axis
            transform.Rotate(Vector3.up, - rotSpeed * Time.deltaTime);
            player.canRotateRight = true;
            player.anim.SetBool("running", true);
            //Debug.Log("turning");

            // Update Y rotation on Samus
            if (player.transform.localScale.z < 0){
                Vector3 newScale = player.transform.localScale;
                newScale.z = -player.transform.localScale.z;
                player.transform.localScale = newScale;
            }
        }
        else if (player.canRotateRight && Input.GetKey(KeyCode.RightArrow) && !player.onElevator){
            // Rotate CCW around Y-axis
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
            player.canRotateLeft = true;
            player.anim.SetBool("running", true);

            if (player.transform.localScale.z > 0){
                Vector3 newScale = player.transform.localScale;
                newScale.z = -player.transform.localScale.z;
                player.transform.localScale = newScale;

            }

        }
        else if (player.anim.GetBool("running")){
            // Stopped running
            player.anim.SetBool("running",false);
            player.anim.SetBool("jumpRunning", false);
        }

        if (Input.GetKey(KeyCode.UpArrow) && player.isGrounded){
            player.jump();
            player.isGrounded = false;
        }

    }
}
