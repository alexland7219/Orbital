using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotSpeed;
    private bool enemyBullet;
    public float timeToLive;

    // Start is called before the first frame update
    void Start()
    {
        enemyBullet = false; // for now
        rotSpeed = 50.0f;
        timeToLive = 10.0f;

        if (!GameObject.FindObjectOfType<Player>().lookingLeft()) rotSpeed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0) die();

        transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.LogWarning("Collision with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Block")
        {

            // Die
            Destroy(gameObject);

        }
        else if (collision.gameObject.tag ==  "Enemy")
        {
            // Hurt
            Debug.Log("Enemy hit");
            die();
        }
    }

    public void die(){
        Destroy(gameObject);
    }

}
