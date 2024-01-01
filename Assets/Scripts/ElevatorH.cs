using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorH : MonoBehaviour
{
    private float totalTime; // Seconds
    public Player player; // Player is the script attached to the Player object
    private Vector3 directionToOrigin;

    public bool player_triggering;
    public int moveInwards;
    public bool moving;
    public bool started;
    public bool centered;
    // Start is called before the first frame update
    void Start()
    {
        moveInwards = 0;
        player_triggering = false;
        moving = false;
        centered = false;
        started = false;

        Vector3 vrp = Vector3.zero;
        vrp.x = 5.0f;
        vrp.z = 10.3f;

        directionToOrigin = Vector3.Normalize(vrp - transform.position);
        directionToOrigin.y = 0f;
        directionToOrigin = Vector3.Normalize(directionToOrigin);

        totalTime = 2.6f;

        if (player == null){ 
            player = GameObject.FindObjectOfType<Player>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player_triggering && Input.GetKey(KeyCode.Space) && totalTime >= 0 && !started && !moving)
        {
            player.centerElevator(transform.position);
            started = true;
        }

        if (started) centered = player.isCentered(transform.position);

        if (centered && started && !moving){
            moving = true;
            centered = false;
            moveInwards = (moveInwards + 1) % 2;
        }

        if (moving){
            Debug.LogWarning("moving");
            transform.Translate((moveInwards * 2 - 1) * directionToOrigin * Time.deltaTime);
            player.moveInwards(moveInwards*2 - 1);
            totalTime -= Time.deltaTime;
        }

        if (moving && totalTime < 0){
            moving = false;
            started = false;
            totalTime = 2.6f;
            player.releaseFromElevator();
        }

    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated){
            totalTime = 2.6f;

            activated = true;
            moveInwards = !moveInwards;
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            player_triggering = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_triggering = false;
        }
    }


}
