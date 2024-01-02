using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotSpeed;
    private bool enemyBullet;
    public float timeToLive;
    public bool isSmallBullet;

    // Start is called before the first frame update
    void Start()
    {
        enemyBullet = false; // for now
        rotSpeed = 50.0f;
        if (!isSmallBullet) timeToLive = 10.0f;
        else timeToLive = 0.5f;

        if (!GameObject.FindObjectOfType<Player>().lookingLeft()) rotSpeed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0) die();

        transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);

    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.LogWarning("Collision with " + collision.gameObject.tag);
        if (other.CompareTag("Block")) die();
        else if (other.CompareTag("Enemy"))
        {
            // Hurt
            die();
        }
    }

    public void die(){
        Destroy(gameObject);
    }

}
