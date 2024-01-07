using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour

{
    public GameObject rockObject;
    public GameObject player;
    public GameObject audioMgr;
    private GameObject hand;
    private GameObject rock;
    private Animator anim;
    private bool throwing;
    private float rotSpeed;
    private float oldrotSpeed;
    public bool turning;
    private float turnTimer;
    private float targetangle;
    private float r;
    private float lerpSpeed = 0.001f;
    private float angle;
    private float anglestart;
    public bool punchHasStrength;

    Health_Bar healthBarComponent;
    public int hp;
    public int shield;
    private bool first, second, third, dead;
    private bool golemwalkSound;

    public Player playerScript; // Script of the player
    private bool spawned;
    public int level; // IMPORTANT: TO CHANGE IN THE EDITOR


    // Start is called before the first frame update
    void Start()
    {

        hp = 1000;
        shield = 0;
        hand = GameObject.Find("RockPoint");
        rotSpeed = 15.0f;
        turnTimer = 2.0f;
        turning = false;
        spawned = false;
        angle = 0f;
        anim = GetComponent<Animator>();
        oldrotSpeed = rotSpeed;
        punchHasStrength = false;
        healthBarComponent = gameObject.transform.Find("HealthCanvasBoss/HealthBar").gameObject.GetComponent<Health_Bar>();
        first = second = third = dead = false;
        golemwalkSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            if (playerScript.getLevel() >= level) spawned = true;
            else return;
        }


        if (!dead && spawned)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            //Debug.Log(dist + " " + angle);
            if (dist > 14.5f && !anim.GetBool("Throw"))
            {
                anim.SetBool("Throw", true);
                anim.SetBool("Walk", false);
                anim.SetBool("Punch", false);
                //oldrotSpeed = rotSpeed;
                rotSpeed = 0;
            }

            else if (dist > 3.0f && !anim.GetBool("Walk") && !anim.GetBool("Throw") && !anim.GetBool("Punch") && !anim.GetBool("Hit"))
            {
                anim.SetBool("Walk", true);
            }

            else if (dist <= 3.0f && !anim.GetBool("Punch"))
            {
                Debug.Log("Punchable");
                anim.SetBool("Punch", true);
                anim.SetBool("Walk", false);
                anim.SetBool("Throw", false);
                rotSpeed = 0;
            }

            if (anim.GetBool("Walk"))
            {
                if (transform.position.z <= 0) rotSpeed = 15.0f;
                else rotSpeed = -15.0f;

                if (!isgettingCloser() && !turning)
                {
                    Debug.Log("GOLEM HAS TO TURN AROUND " + rotSpeed + " " + transform.position.z);
                    turning = true;
                    rotSpeed = 0;
                    anglestart = angle;
                }
            }

            if (turning)
            {
                rotSpeed = 0;
                if (anglestart <= 0)
                {
                    float torotate = 100f * Time.deltaTime;
                    transform.Rotate(0, torotate, 0);
                    angle += torotate;
                    //Debug.Log(angle);
                    if (angle >= 180f)
                    {
                        if (anim.GetBool("Walk")) rotSpeed = -15.0f;
                        turning = false;
                    }
                }
                else
                {   
                    float torotate = 100f * Time.deltaTime;
                    transform.Rotate(0, torotate, 0);
                    angle -= torotate;
                    //Debug.Log(angle);
                    if (angle <= 0f)
                    {
                        if (anim.GetBool("Walk")) rotSpeed = 15.0f;
                        turning = false;
                    }
                }
            }

            /*if (anim.GetBool("Throw")) {
                Vector3 targetdirection = player.transform.position;
                targetdirection.y = 0;
                Quaternion rotation = Quaternion.LookRotation(targetdirection);
                transform.rotation = rotation;
            }*/
            transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);
            //Debug.Log(isgettingCloser());
        }
        if (hp != healthBarComponent.health) healthBarComponent.health = hp;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet otherBull = other.GetComponent<Bullet>();
            if (!otherBull.getIsEnemy())
            {

                //anim.SetTrigger("GetHit");

                if (shield <= 0) hp -= 100;
                else shield -= 100;

                if (!first && hp <= 750f) {
                    first = true;
                    anim.SetBool("Walk", false);
                    anim.SetBool("Hit", true);
                    rotSpeed = 0;
                }

                else if (!second && hp <= 500f)
                {
                    second = true;
                    anim.SetBool("Walk", false);
                    anim.SetBool("Hit", true);
                    rotSpeed = 0;
                }

                else if (!third && hp <= 250f)
                {
                    third = true;
                    anim.SetBool("Walk", false);
                    anim.SetBool("Hit", true);
                    rotSpeed = 0;
                }

                if (hp <= 0) {
                    rotSpeed = 0;
                    dead = true;
                    anim.SetBool("Dead", true);
                }

                Destroy(other.gameObject);
            }
        }
    }

    void throwRock ()
    {
        //Debug.Log("Rock thrown");
        Rock script = rock.GetComponent<Rock>();
        script.release();
    }

    void pickupRock() {
        //Debug.Log("Creating rock");
        rock = Instantiate(rockObject, hand.transform.position, hand.transform.rotation);
        rock.GetComponent<Rock>().audioMgr = GameObject.Find("AudioManager");
        rock.transform.localScale = new Vector3(0.0065f, 0.0065f, 0.0065f);
        rock.transform.SetParent(hand.transform);
        anim.SetBool("Throw", false);
    }

    bool isgettingCloser()
    {
        if (angle <= 0) return rotSpeed > 0;
        else return rotSpeed < 0;
    }

    void setpunchStrength() { punchHasStrength = true; }

    void unsetpunchStrength() { punchHasStrength = false; }

    void setWalk() {
        if (Vector3.Distance(player.transform.position, transform.position) > 3.0f) {
            //Debug.Log("Entro");
            anim.SetBool("Punch", false);
            //anim.SetBool("Walk", true);
        }
    }

    void unsetHit() { anim.SetBool("Hit", false); }

    void stepSound() { audioMgr.GetComponent<MainAudioManager>().PlayGolemStepSound(); }

    void damageSound(){ audioMgr.GetComponent<MainAudioManager>().PlayGolemDamageSound(); }

    void deathSound() {
        // S'HA DE CREAR AQUESTA FUNCIÓ
        //audioMgr.GetComponent<MainAudioManager>().stopAllSounds();
        audioMgr.GetComponent<MainAudioManager>().PlayGolemDeathSound();
    }

    void fallSound() { audioMgr.GetComponent<MainAudioManager>().PlayGolemFallSound(); }

    private void winMusic() { audioMgr.GetComponent<MainAudioManager>().PlayWinMusic(); }

    private void puchSound() { audioMgr.GetComponent<MainAudioManager>().PlayGolemPunchSound(); }

    private void swingSound() { audioMgr.GetComponent<MainAudioManager>().PlayGolemSwingSound(); }
}
