using Unity.VisualScripting;
using UnityEngine;

public class walkingEnemy : MonoBehaviour
{
    public float rotSpeed;
    public int hp;
    public int shield;
    public float angle;
    public float maxangle;

    private float oldrotSpeed;
    private float deathTimer;
    private float stopTimer;
    private bool stopped;
    private GameObject playergameObject;

    Animator anim;
    Health_Bar healthBarComponent;

    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 15.0f;
        hp = 100;
        shield = 100;
        angle = 0;
        healthBarComponent = gameObject.transform.Find("HealthCanvas/HealthBar").gameObject.GetComponent<Health_Bar>();
        anim = GetComponent<Animator>();
        oldrotSpeed = 0;
        deathTimer = 2.0f;
        stopped = false;
        stopTimer = 5.0f;
        playergameObject = GameObject.Find("T-Pose");
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped && Vector3.Distance(playergameObject.transform.position, transform.position) < 5 && isgettingCloser()) {

            /////////
            //    Aqui s'aur� de posar la posicio exacta del jugador
            /////////
            
            //Debug.Log(new Vector3(-8.56129646f, 12.5699997f, 0) + " i " + transform.position);
            anim.SetBool("Walk", false);
            oldrotSpeed = rotSpeed;
            rotSpeed = 0;
            stopped = true;
        }

        if (stopped && stopTimer <= 0) {
            anim.SetBool("Walk", true);
            rotSpeed = oldrotSpeed;
            stopped = false;
            stopTimer = 5.0f;
        }

        if (stopped) stopTimer -= Time.deltaTime;

        transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);
        angle += rotSpeed * Time.deltaTime;

        if ((angle > maxangle || angle < 0) && !stopped)
        {
            rotSpeed = -rotSpeed;
            transform.Rotate(new Vector3(0, 180, 0));
            transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);
            angle += rotSpeed * Time.deltaTime;
        }

        if (hp != healthBarComponent.health) healthBarComponent.health = hp;
        if (shield != healthBarComponent.shield) healthBarComponent.shield = shield;


        if (anim.GetBool("Death"))
        {
            deathTimer -= Time.deltaTime;
        }
        if (anim.GetBool("Death") && deathTimer <= 0) die();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet otherBull = other.GetComponent<Bullet>();
            if (!otherBull.getIsEnemy()) {
                
                anim.SetTrigger("GetHit");

                if (shield <= 0) hp -= 20;
                else shield -= 20;
                if (hp <= 20)
                {
                    if (hp > 0) anim.SetBool("1HP", true);
                    if (hp <= 0) anim.SetBool("Death", true);
                    rotSpeed = 0;
                }

                Destroy(other.gameObject);
            }
        }
    }


    void die()
    {
        Destroy(gameObject);
    }

    bool isgettingCloser() {
        if (transform.position.z > 0) return rotSpeed < 0;
        else return rotSpeed > 0;
    }
}
