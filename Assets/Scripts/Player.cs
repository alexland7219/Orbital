using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public bool canRotateLeft;
    public bool canRotateRight;
    public bool isGrounded;
    public bool onElevator;
    public const float jumpForce = 100f;
    public string dirToGo;
    public Animator anim;
    private float canJumpTimer;
    private float canShootTimer;
    private Vector3 directionToOrigin;

    public GameObject bulletObject;
    public GameObject smallBulletObject;

    private Slider healthSlider;
    private Slider ammoSlider;
    public float timeSinceLastDamage;
    public const float timeBetweenDamages = 0.5f;

    public bool haveSmallGun;
    public bool discoveredBigGun;

    private int hp;
    private int ammo;

    // Start is called before the first frame update
    void Start()
    {
        canRotateRight = true;
        canRotateLeft = true;
        discoveredBigGun = false;
        isGrounded = false;
        timeSinceLastDamage = 0;
        onElevator = false;
        haveSmallGun = true;
        dirToGo = "none";
        canShootTimer = 0.5f;
        anim = GetComponent<Animator>();
        canJumpTimer = 0;
        directionToOrigin = Vector3.Normalize(Vector3.zero - transform.position);
        directionToOrigin.y = 0f;
        directionToOrigin = Vector3.Normalize(directionToOrigin);
        hp = 100;
        ammo = 32;
        healthSlider = GameObject.FindWithTag("Healthbar").GetComponent<Slider>();
        ammoSlider = GameObject.FindWithTag("AmmoBar").GetComponent<Slider>();
    }

    private bool checkGrounded(){
        // Checks if the player is touching ground
        //Vector3 shift = new Vector3(0, +f, 0);

        Ray ray = new Ray(transform.position, Vector3.down); // Change the direction to Vector3.down
        RaycastHit hit;

        return Physics.Raycast(ray, out hit, .1f);
    }

    public bool lookingLeft(){
        // Returns true if we are looking to the left
        return (transform.localScale.z > 0);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastDamage += Time.deltaTime;
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
                if (!haveSmallGun) canShootTimer = 0.5f;
                else canShootTimer = 0.2f; // Small gun takes less time to reload

                if (!Input.GetKey(KeyCode.P)) anim.SetBool("shooting", false);
                else shoot();
            }
        }

        // Roll
        if (Input.GetKey(KeyCode.R)){
            anim.SetTrigger("roll");
        }

        if (Input.GetKey(KeyCode.S) && discoveredBigGun){
            haveSmallGun = !haveSmallGun;
        }
    }

    void shoot()
    {
        if (ammo == 0) return;
        if (!anim.GetBool("shooting")) return; // we have to have the shooting animations started

        // Shoot a bullet
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 1.1f;

        GameObject bullObj;

        if (!haveSmallGun) bullObj = Instantiate(bulletObject, spawnPosition, transform.rotation);
        else bullObj = Instantiate(smallBulletObject, spawnPosition, transform.rotation);

        //bullObj.transform.parent = GameObject.Find("Level").transform;

        if (lookingLeft()) RotateObjectAroundY(bullObj, 180f);

        // Update ammo bar
        if (haveSmallGun) ammo = ammo - 1;
        else ammo -= 2;

        ammoSlider.value = ammo / 32.0f;
    }

    void RotateObjectAroundY(GameObject obj, float angle)
    {
        // Get the current rotation of the object
        Vector3 currentRotation = obj.transform.rotation.eulerAngles;

        // Set the new Y-axis rotation
        currentRotation.y = angle;

        // Apply the new rotation to the object
        obj.transform.rotation = Quaternion.Euler(currentRotation);
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
            if (timeSinceLastDamage < timeBetweenDamages) return;
            else timeSinceLastDamage = 0f;
            // Hurt
            //Debug.LogWarning("PLAYER HURT");
            hp -= 10;
            healthSlider.value = hp / 100.0f;

        }
        else if (collision.gameObject.tag == "Ammo"){
            ammo = 16;
            ammoSlider.value = ammo / 16.0f;
            Destroy(collision.gameObject);
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
        else if (other.CompareTag("Enemy-Low-HP")){
            hp -= 2;
            healthSlider.value = hp / 100.0f;
        }   
        else if (other.CompareTag("Checkpoint"))
        {
            ammo = 16;
            ammoSlider.value = ammo / 16.0f;
            discoveredBigGun = true;
            haveSmallGun = false;
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
        //Debug.Log("Diferencies " + Math.Abs(center.x - pos.x) + " " + Math.Abs(center.z - pos.z));
        if ((Math.Abs(center.z - pos.z) < 0.1f)) {
            dirToGo = "none";
            return true;
        }
        return false;
        //transform.Translate(directionToOrigin * center * Time.deltaTime);
    }
}
