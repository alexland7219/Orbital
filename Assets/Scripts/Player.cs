using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canRotateLeft;
    public bool canRotateRight;
    public bool isGrounded;
    public float jumpForce; // Adjust this value to control jump height
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        canRotateRight = true;
        canRotateLeft = true;
        jumpForce = 100f;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Grounded: " + isGrounded + " LeftRot: " + canRotateLeft + " RightRot: " + canRotateRight);
        if (anim.GetBool("running") && !anim.GetBool("jumpRunning") && !anim.GetBool("roll") && Input.GetKey(KeyCode.R)){
            anim.SetTrigger("roll");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("Collision with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Block")
        {
            Vector3 shift = new Vector3(0, +0.5f, 0);

            Ray ray = new Ray(transform.position - shift, Vector3.down); // Change the direction to Vector3.down
            RaycastHit hit;

            // Checks if we hit something from below
            if (Physics.Raycast(ray, out hit, 0.7f)){
                isGrounded = true;
                anim.SetBool("jump", false);
                anim.SetBool("jumpRunning", false);

                if (!hit.collider.CompareTag("Block")){
                    // We are not standing on a block
                    if (Input.GetKey(KeyCode.LeftArrow)) canRotateLeft = false;
                    else if (Input.GetKey(KeyCode.RightArrow)) canRotateRight = false;

                }
            }
            else {
                if (Input.GetKey(KeyCode.LeftArrow)) canRotateLeft = false;
                else if (Input.GetKey(KeyCode.RightArrow)) canRotateRight = false;

            }

        }
        else {
            isGrounded = true;
            anim.SetBool("jump", false);
            anim.SetBool("jumpRunning", false);

        }
    }

    public void jump(){
        // Perform a Jump
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        if (anim.GetBool("running")) anim.SetBool("jumpRunning", true);
        else anim.SetBool("jump", true);
    }
}
