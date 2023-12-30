using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float randNumber = Random.Range(0f, 2f);

        Invoke("Death", randNumber);
    }

    void Death()
    {
        Destroy(gameObject);
    }

}
