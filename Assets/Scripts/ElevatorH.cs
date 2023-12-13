using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorH : MonoBehaviour
{
    private float totalTime; // Seconds
    public Player player; // Player is the script attached to the Player object
    private Vector3 directionToOrigin;

    public bool activated;
    public bool moveInwards;
    // Start is called before the first frame update
    void Start()
    {
        moveInwards = false;

        Vector3 vrp = Vector3.zero;
        vrp.x = 3.0f;
        vrp.z = 10.3f;

        directionToOrigin = Vector3.Normalize(vrp - transform.position);
        directionToOrigin.y = 0f;

        activated = false;
        totalTime = 2.6f;

        if (player == null){ 
            player = GameObject.FindObjectOfType<Player>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            totalTime -= Time.deltaTime;

            if (totalTime < 0){
                activated = false;
                player.releaseFromElevator();

            }
            else if (moveInwards)
            {
                transform.Translate(directionToOrigin * Time.deltaTime);
                player.moveInwards(1);
            }
            else {
                transform.Translate(-directionToOrigin * Time.deltaTime);
                player.moveInwards(-1);

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated){
            totalTime = 2.6f;

            activated = true;
            moveInwards = !moveInwards;
        }
    }

}
