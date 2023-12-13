using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorV : MonoBehaviour
{
    private float totalTime; // Seconds
    public Player player; // Player is the script attached to the Player object

    public bool activated;
    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        totalTime = 3.8f;

        if (player == null){
            player = GameObject.FindObjectOfType<Player>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (activated && totalTime >= 0) transform.Translate(Vector3.up * 3 * Time.deltaTime);
        if (activated) totalTime -= Time.deltaTime;

        if (activated && totalTime < 0){
            activated = false;
            player.releaseFromElevator();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            activated = true;
        }
    }

}
