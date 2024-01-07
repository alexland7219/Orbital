using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.WSA;

public class Portal : MonoBehaviour
{
    public bool activated;
    public ParticleSystem partsys;
    public GameObject player;
    private float timer;
    private bool playerinside;
    public GameObject audioMgr;
    // Start is called before the first frame update
    void Start()
    {
        audioMgr = GameObject.Find("AudioManager");

        activated = false;
        playerinside = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated) timer += Time.deltaTime;
        Vector3 portalposnoy = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 playerposnoy = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
        float dist = Vector3.Distance(portalposnoy, playerposnoy);
        //Debug.Log(name + " " + !activated + " " + dist + " " + (player.GetComponent<Player>().level == 5));
        if (!activated && dist < 7.0f && player.GetComponent<Player>().level == 2) {
            Debug.Log(name + " activating");
            partsys.Play();
            activated = true;
            audioMgr.GetComponent<AudioManager>().PlayAtIndex(11);
        }

        if (timer >= 3.0f) {
            if (playerinside) {
                if (Input.GetKey(KeyCode.Space)) {
                    //                                                      POSAR AL�ADA DEL NIVELL DEL BOSS
                    player.GetComponent<Player>().level = 6;
                    player.transform.position = new Vector3(player.transform.position.x, 34.46f, player.transform.position.z);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") Debug.Log("IS INSIDE"); playerinside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") Debug.Log("IS OUTSIDE"); playerinside = false;
    }
}
