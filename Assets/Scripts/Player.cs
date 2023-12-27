using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canRotateLeft;
    public bool canRotateRight;
    public bool isGrounded;
    public bool onElevator;
    public float jumpForce; // Adjust this value to control jump height
    public Animator anim;
    private float canJumpTimer;
    private float canShootTimer;
    private Vector3 directionToOrigin;
    public GameObject bulletObject;

    // Start is called before the first frame update
    void Start()
    {
        canRotateRight = true;
        canRotateLeft = true;
        jumpForce = 100f;
        isGrounded = false;
        onElevator = false;
        canShootTimer = 1f;
        anim = GetComponent<Animator>();
        canJumpTimer = 0;
        bulletObject = GameObject.Find("Bullet-template");

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

        if (!anim.GetBool("shooting") && Input.GetKey(KeyCode.P)){
            anim.SetBool("shooting", true);
            // Shoot
            shoot();
        }
        else if (anim.GetBool("shooting")){
            canShootTimer -= Time.deltaTime;
            if (canShootTimer < 0){
                canShootTimer = 1f;
                if (!Input.GetKey(KeyCode.P)) anim.SetBool("shooting", false);
            }
        }
    }

    void shoot()
    {
        // Shoot a bullet
        GameObject bullet = Instantiate(bulletObject, transform.position, transform.rotation);
        bullet.transform.parent = GameObject.Find("Level").transform;
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
        else if (collision.gameObject.tag ==  "Enemy")
        {
            // Hurt
            Debug.LogWarning("PLAYER HURT");

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            onElevator = true;
            Debug.Log("Collision player-elevator");
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
}
