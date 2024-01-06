using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import the TextMeshPro namespace

public class ElevatorV : MonoBehaviour
{
    private float totalTime; // Seconds
    public Player player; // Player is the script attached to the Player object

    public bool player_triggering;
    public bool moving;
    public bool started;
    public int level;
    public bool centered;
    private Vector3 direction;
    private float timeLock;
    public bool toLock;
    public TextMeshProUGUI countdownText;
    public GameObject countdownElem;
    // Start is called before the first frame update
    void Start()
    {
        player_triggering = false;
        moving = false;
        centered = false;
        started = false;
        timeLock = 60f;
        totalTime = 11.4226939f;
        direction = Vector3.up;

        if (player == null){
            player = GameObject.FindObjectOfType<Player>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (toLock && level == player.getLevel() && timeLock > 0)
        {
            countdownElem.SetActive(true);
            timeLock -= Time.deltaTime;
            countdownText.text = string.Format("00:{0:D2}", Mathf.RoundToInt(timeLock) % 60);
            return;
        }

        if (player_triggering && Input.GetKey(KeyCode.Space) && totalTime >= 0 && !started && !moving) {
            player.centerElevator(transform.position);
            started = true;
            if (direction == Vector3.up) player.changeLevel(+1);
            else player.changeLevel(-1);
        }

        if (started) centered = player.isCentered(transform.position);

        if (centered && started && !moving) {
            moving = true;
            centered = false;
        }

        if (moving) {
            if (totalTime > 3 * Time.deltaTime)
            {
                //Debug.Log("IF - Quenden " + totalTime + " segons");
                transform.Translate(direction * 3 * Time.deltaTime);
                totalTime -= 3 * Time.deltaTime;
            }
            else {
                //Debug.Log("ELSE - Quenden " + totalTime + " segons");
                transform.Translate(direction * totalTime);
                totalTime = -1;
            }
        }

        if (moving && totalTime < 0){
            //Debug.Log("Para de moures");
            moving = false;
            started = false;
            totalTime = 11.4226939f;
            if (direction == Vector3.up) direction = Vector3.down;
            else direction = Vector3.up;
            player.releaseFromElevator();
        }
    }

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
