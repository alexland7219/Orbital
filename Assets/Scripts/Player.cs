using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public bool canRotateLeft;
    public bool canRotateRight;
    public bool isGrounded;
    public bool onElevator;
    public float jumpForce; // Adjust this value to control jump height
    public string dirToGo;
    public Animator anim;
    private float canJumpTimer;
    private Vector3 directionToOrigin;


    // Start is called before the first frame update
    void Start()
    {
        canRotateRight = true;
        canRotateLeft = true;
        jumpForce = 100f;
        isGrounded = false;
        onElevator = false;
        dirToGo = "none";
        anim = GetComponent<Animator>();
        canJumpTimer = 0;

        directionToOrigin = Vector3.Normalize(Vector3.zero - transform.position);
        directionToOrigin.y = 0f;

    }

    private bool checkGrounded(){
        // Checks if the player is touching ground
        //Vector3 shift = new Vector3(0, +f, 0);

        Ray ray = new Ray(transform.position, Vector3.down); // Change the direction to Vector3.down
        RaycastHit hit;

        return Physics.Raycast(ray, out hit, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Grounded: " + isGrounded + " LeftRot: " + canRotateLeft + " RightRot: " + canRotateRight);
        //if (anim.GetBool("running") && !anim.GetBool("jumpRunning") && !anim.GetBool("roll") && Input.GetKey(KeyCode.R)){
        //    anim.SetTrigger("roll");
        //}

        if (canJumpTimer > 0) canJumpTimer -= Time.deltaTime;

        // Check if we are grounded
        if (!isGrounded && checkGrounded())
        {
            // Debug.Log("GROUNDING");
            // We are grounded
            isGrounded = true;
            anim.SetBool("jump", false);
            anim.SetBool("jumpRunning", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.LogWarning("Collision with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Block")
        {

            Vector3 normal = collision.contacts[0].normal;

            if (normal.y == 1)
            {
                // Collision has been when falling
                isGrounded = true;
                anim.SetBool("jump", false);
                anim.SetBool("jumpRunning", false);
            }
            else if (normal.z == -1) canRotateLeft = false;
            else if (normal.z == +1) canRotateRight = false;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            if (Input.GetKey(KeyCode.Space)) {
                onElevator = true;
                Debug.Log("Collision player-elevator");
            }
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Block"){
            canRotateLeft = true;
            canRotateRight = true;
        }

        if (!checkGrounded()) isGrounded = false;
    }

    public void jump(){
        if (canJumpTimer > 0) return;

        // Perform a Jump
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;

        canJumpTimer = 0.5f;

        if (anim.GetBool("running")) anim.SetBool("jumpRunning", true);
        else anim.SetBool("jump", true);
    }

    public float getYpos()
    {
        return GetComponent<Transform>().position.y;
    }

    public void releaseFromElevator(){ onElevator = false; }

    public void moveInwards(int inw){ transform.Translate(directionToOrigin * inw * Time.deltaTime); }

    public void centerElevator(Vector3 center) {
        onElevator = true;
        Vector3 pos = transform.position;
        if (center.z > pos.z) dirToGo = "left";
        else dirToGo = "right";
        //transform.Translate(directionToOrigin * center * Time.deltaTime);
    }

    public bool isCentered(Vector3 center)
    {
        Vector3 pos = transform.position;
        Debug.Log("Diferencies " + Math.Abs(center.x - pos.x) + " " + Math.Abs(center.z - pos.z));
        if ((Math.Abs(center.z - pos.z) < 0.1f)) {
            dirToGo = "none";
            return true;
        }
        return false;
        //transform.Translate(directionToOrigin * center * Time.deltaTime);
    }
}
