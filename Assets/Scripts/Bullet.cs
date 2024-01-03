using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotSpeed;
    private bool enemyBullet = false;
    public float timeToLive;
    public bool isSmallBullet;
    private bool negSpeed = false;

    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 50.0f;
        if (!isSmallBullet) timeToLive = 10.0f;
        else if (!enemyBullet) timeToLive = 0.5f;
        else timeToLive = 2.0f;

        //if (!GameObject.FindObjectOfType<Player>().lookingLeft()) rotSpeed *= -1;
        if (negSpeed) rotSpeed *= -1;
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
    }

    public void die(){
        Destroy(gameObject);
    }

    public void setEnemyBullet(bool isEnem){ enemyBullet = isEnem; timeToLive = 2.0f;}

    public bool getIsEnemy(){ return enemyBullet; }

    public void negateSpeed(){ 
        negSpeed = true;
    }

}
