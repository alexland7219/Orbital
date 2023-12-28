using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float rotSpeed;
    private bool enemyBullet;

    // Start is called before the first frame update
    void Start()
    {
        enemyBullet = false; // for now
        rotSpeed = 50.0f;

        if (!GameObject.FindObjectOfType<Player>().lookingLeft()) rotSpeed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.LogWarning("PLAYER HURT");

        }
    }

}
