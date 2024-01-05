using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private float rotSpeed = 40f;
    public Player player; // Player is the script attached to the Player object
    private float punchTimer;
    public GameObject walls;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null){
            player = GameObject.FindObjectOfType<Player>();
        }
        punchTimer = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (player.dirToGo == "left") { transform.Rotate(Vector3.up, -rotSpeed/10.0f * Time.deltaTime); }
        else if (player.dirToGo == "right") { transform.Rotate(Vector3.up, rotSpeed/10.0f * Time.deltaTime); }

        if (player.canRotateLeft && Input.GetKey(KeyCode.LeftArrow) && !player.onElevator && player.dirToGo == "none"){
            // Rotate everything around the Y axis
            transform.Rotate(Vector3.up, - rotSpeed * Time.deltaTime);

            walls.transform.Rotate(Vector3.up, - 0.2f * rotSpeed * Time.deltaTime);

            player.canRotateRight = true;
            player.anim.SetBool("running", true);
            //Debug.Log("TURNING LEFT");

            // Update Y rotation on Samus
            if (player.transform.localScale.z < 0){
                Vector3 newScale = player.transform.localScale;
                newScale.z = -player.transform.localScale.z;
                player.transform.localScale = newScale;
            }
        }
        else if (player.canRotateRight && Input.GetKey(KeyCode.RightArrow) && !player.onElevator && player.dirToGo == "none")
        {
            // Rotate CCW around Y-axis
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
            walls.transform.Rotate(Vector3.up, 0.2f * rotSpeed * Time.deltaTime);
            //Debug.Log("TURNING RIGHT");

            player.canRotateLeft = true;
            player.anim.SetBool("running", true);

            if (player.transform.localScale.z > 0){
                Vector3 newScale = player.transform.localScale;
                newScale.z = -player.transform.localScale.z;
                player.transform.localScale = newScale;

            }

        }

        else if (player.anim.GetBool("running"))
        {
            // Stopped running
            player.anim.SetBool("running", false);
            player.anim.SetBool("jumpRunning", false);
        }
        
        if (player.dirToGo == "punchleft" && player.canRotateLeft)
        {
            transform.Rotate(Vector3.up, -rotSpeed * 2 * Time.deltaTime);
            punchTimer += Time.deltaTime;
        }

        if (player.dirToGo == "punchright" && player.canRotateRight)
        {
            transform.Rotate(Vector3.up, rotSpeed * 2 * Time.deltaTime);
            punchTimer += Time.deltaTime;
        }

        if (punchTimer > 1.0f && player.isGrounded)
        {
            punchTimer = 0;
            player.dirToGo = "none";
        }

        if (Input.GetKey(KeyCode.UpArrow) && player.isGrounded){
            player.jump();
            player.isGrounded = false;
        }

    }
}
