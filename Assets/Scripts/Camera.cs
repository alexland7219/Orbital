using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform camTransf;
    public Player player; // Player is the script attached to the Player object

    // Start is called before the first frame update
    void Start()
    {
        if (player == null){
            player = GameObject.FindObjectOfType<Player>();
        }

        camTransf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 current = camTransf.position;
        current.y = player.getYpos() + 4.3f;
        camTransf.position = current;
    }
}
