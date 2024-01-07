using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro; // Import the TextMeshPro namespace
using UnityEngine.SceneManagement;

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

    public GameObject golemObject;
    public GameObject bulletObject;
    public GameObject smallBulletObject;
    public TextMeshProUGUI counterObject;
    public TextMeshProUGUI weaponNameObject;
    public Image weaponNameImageBg;
    public GameObject floatingGun;
    private GameObject audioMgr;

    private Slider healthSlider;
    private Slider ammoSlider;
    public float timeSinceLastDamage;
    public const float timeBetweenDamages = 0.5f;

    private bool haveSmallGun;
    public bool discoveredBigGun;

    private int hp;
    private int ammo;
    private bool isInsideEnemy;
    //private bool checkpointVisited;

    private float rollTimer;
    private float changeWeaponTimer;

    private bool crashedagainstGolem;

    public int level;

    // Start is called before the first frame update
    void Start()
    {
        canRotateRight = true;
        canRotateLeft = true;
        discoveredBigGun = false;
        //checkpointVisited = false;
        isGrounded = false;
        timeSinceLastDamage = 0;
        onElevator = false;
        haveSmallGun = true;
        dirToGo = "none";
        canShootTimer = 0.5f;
        changeWeaponTimer = 0.5f;
        anim = GetComponent<Animator>();
        canJumpTimer = 0;
        directionToOrigin = Vector3.Normalize(Vector3.zero - transform.position);
        directionToOrigin.y = 0f;
        directionToOrigin = Vector3.Normalize(directionToOrigin);
        hp = 100;
        ammo = 32;
        healthSlider = GameObject.FindWithTag("Healthbar").GetComponent<Slider>();
        ammoSlider = GameObject.FindWithTag("AmmoBar").GetComponent<Slider>();
        isInsideEnemy = false;
        crashedagainstGolem = false;
        level = 0;
        audioMgr = GameObject.Find("AudioManager");

        counterObject.text = "32";
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

    void DelayDeath()
    {
        // Code to be executed after the delay
        SceneManager.LoadScene(4);
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("dead")) return;

        if (hp <= 0 ){
            anim.SetBool("dead", true);
            audioMgr.GetComponent<AudioManager>().PlayAtIndex(9);          
            audioMgr.GetComponent<AudioManager>().StopAtIndex(1); 
            audioMgr.GetComponent<AudioManager>().StopAtIndex(8);          

            Invoke("DelayDeath", 5.0f);
        } 

        timeSinceLastDamage += Time.deltaTime;
        if (changeWeaponTimer > 0) changeWeaponTimer -= Time.deltaTime;

        if (anim.GetBool("roll"))
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer < 0) anim.SetBool("roll", false);
        }

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

        if (!anim.GetBool("shooting") && Input.GetKey(KeyCode.D))
        {
            anim.SetBool("shooting", true);
            // Shoot
            shoot();
        }
        else if (anim.GetBool("shooting"))
        {
            canShootTimer -= Time.deltaTime;
            if (canShootTimer < 0)
            {
                if (!haveSmallGun) canShootTimer = 0.5f;
                else canShootTimer = 0.2f; // Small gun takes less time to reload

                if (!Input.GetKey(KeyCode.D)) anim.SetBool("shooting", false);
                else shoot();
            }
        }

        // Roll
        if (Input.GetKey(KeyCode.R))
        {

            if (!anim.GetBool("jump") && !anim.GetBool("jumpRunning") && !anim.GetBool("roll") && anim.GetBool("running"))
            {
                anim.SetBool("roll", true);
                rollTimer = 2.5f;
            }
        }

        if (Input.GetKey(KeyCode.S) && discoveredBigGun && changeWeaponTimer <= 0)
        {
            haveSmallGun = !haveSmallGun;
            changeWeaponTimer = 0.5f;
            if (haveSmallGun)
            {
                weaponNameObject.text = "SHORT";
                weaponNameImageBg.color = new Color(0f, 172f / 255f, 1f);
            }
            else
            {
                weaponNameObject.text = "LONG";
                weaponNameImageBg.color = new Color(158f / 255f, 90f / 255f, 1f);
            }
        }

        // BOSS LEVEL -- REALLY IMPORTANT
        if (level == 6)
        {
            Vector3 golemposnoy = new Vector3(golemObject.transform.position.x, 0f, golemObject.transform.position.z);
            Vector3 playerposnoy = new Vector3(transform.position.x, 0f, transform.position.z);

            float dist = Vector3.Distance(golemposnoy, playerposnoy);
            //Debug.Log("Distance: " + dist);
            if (dist < 2.5)
            {
                if (!crashedagainstGolem)
                {
                    if (golemObject.transform.position.z < 0) canRotateRight = false;
                    else canRotateLeft = false;
                    crashedagainstGolem = true;
                }
            }
            else if (crashedagainstGolem)
            {
                canRotateRight = true;
                canRotateLeft = true;
                crashedagainstGolem = false;
            }

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
        else {
            // Other direction
            Bullet script = bullObj.GetComponent<Bullet>();
            script.negateSpeed();
        }

        // Update ammo bar
        if (haveSmallGun) ammo = ammo - 1;
        else ammo -= 2;

        counterObject.text = ammo.ToString();

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
        //Debug.Log("Collision with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Turret")
        {
            Vector3 normal = collision.contacts[0].normal;


            Debug.Log("NORMAL" + normal);

            if (normal.y == 1)
            {
                // Collision has been when falling
                isGrounded = true;
                anim.SetBool("jump", false);
                anim.SetBool("jumpRunning", false);
            }
            else if (normal.z < -0.95f){
                canRotateLeft = false;
                Debug.Log("Cant rotate left");
            }
            else if (normal.z > 0.95f) {
                canRotateRight = false;
                Debug.Log("Cant rotate right");
            }
        }/*
        else if (collision.gameObject.tag == "Ammo"){
            ammo = 32;
            ammoSlider.value = ammo / 32.0f;


            counterObject.text = ammo.ToString();

            Destroy(collision.gameObject);
        }*/
    }

    void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Elevator"))
        {
            if (Input.GetKey(KeyCode.Space)) {
                onElevator = true;
            }
        }*/
        if (other.CompareTag("Enemy-Low-HP")){
            hp -= 2;
            healthSlider.value = hp / 100.0f;
            Debug.LogWarning("Collision with a trap");
        }   
        else if (other.CompareTag("Checkpoint"))
        {
            ammo = 32;
            counterObject.text = ammo.ToString();

            ammoSlider.value = ammo / 32.0f;
            discoveredBigGun = true;
            haveSmallGun = false;

            weaponNameObject.text = "LONG";
            weaponNameImageBg.color = new Color(158f/255f, 90f/255f, 1f);

            //checkpointVisited = true;
            Destroy(other.gameObject);

        }
        else if (other.CompareTag("Recharge"))
        {
            if (!other.gameObject.GetComponent<FloatingGun>().getCanTake()) return;

            ammo = 32;
            ammoSlider.value = ammo / 32.0f;


            counterObject.text = ammo.ToString();

            if (!other.gameObject.GetComponent<FloatingGun>().getIsPermanent()) Destroy(other.gameObject);
            else other.gameObject.GetComponent<FloatingGun>().restartTempo();

        }
        else if (other.CompareTag("Hearts"))
        {
            hp = 100;
            healthSlider.value =1.0f;

            Destroy(other.gameObject);

        }

        else if (other.CompareTag("Enemy"))
        {
            if (isInvincible()) return;
            isInsideEnemy = true;
            timeSinceLastDamage = 0;
            hp -= 10;
            healthSlider.value = hp / 100.0f;
        }
        else if (other.CompareTag("Punch"))
        {
            if (golemObject.GetComponent<Boss>().punchHasStrength) {
                if (isInvincible()) return;
                isInsideEnemy = true;
                timeSinceLastDamage = 0;
                hp -= 20;
                healthSlider.value = hp / 100.0f;
                Rigidbody rb = GetComponent<Rigidbody>();
                if (other.gameObject.transform.position.z < 0) dirToGo = "punchleft";
                else dirToGo = "punchright";
                rb.AddForce(Vector3.up * 4000);
            }
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet otherBull = other.GetComponent<Bullet>();
            if (otherBull.getIsEnemy()) {
                if (!isInvincible()){
                    timeSinceLastDamage = 0f;
                    hp -= 10;
                    healthSlider.value = hp / 100.0f;

                }
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Rock"))
        {
            Rock script = other.GetComponent<Rock>();
            if (!isInvincible()) {
                timeSinceLastDamage = 0f;
                hp -= 10;
                healthSlider.value = hp / 100.0f;
            }
            Destroy(other.gameObject);
            script.die();
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("Enemy")) isInsideEnemy = false;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Turret"){
            canRotateLeft = true;
            canRotateRight = true;
            Debug.Log("Exiting collision");
        }

        if (!checkGrounded()) isGrounded = false;
    }

    public void jump(){
        if (canJumpTimer > 0) return;
        if (anim.GetBool("roll")) return; // Cant jump while evading

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

    bool isInvincible()
    {
        return anim.GetBool("roll"); // Haurem de afegir la tecla G
    }

    public void changeLevel(int dif)
    {
        level += dif;
    }

    public int getLevel()
    {
        return level;
    }
}
