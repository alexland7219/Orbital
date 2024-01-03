using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : MonoBehaviour

{
    public GameObject rockObject;
    private GameObject hand;
    private GameObject rock;

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.Find("RockPoint");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void throwRock ()
    {
        Debug.Log("Rock thrown");
        Rock script = rock.GetComponent<Rock>();
        script.release();
    }

    void pickupRock() {
        Debug.Log("Creating rock");
        rock = Instantiate(rockObject, hand.transform.position, hand.transform.rotation);
        rock.transform.localScale = new Vector3(0.007f, 0.007f, 0.007f);
        rock.transform.SetParent(hand.transform);
    }
}
