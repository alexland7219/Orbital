using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    Rigidbody body;
    GameObject hand;
    GameObject player;
    public GameObject empty;
    private bool released;
    private GameObject target;
    private float speed;
    private Vector3 dir;
    private float minDistance;
    private bool hasarrived;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        hand = GameObject.Find("RockPoint");
        player = GameObject.Find("T-Pose");
        released = false;
        speed = 10.0f;
        minDistance = 1.0f;
        hasarrived = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!released) return;
        if (!hasarrived) {
            dir = ((target.transform.position + new Vector3(0, 0.5f, 0)) - transform.position);
            float dist = dir.magnitude;
            if (dist < minDistance) hasarrived = true;
            dir.Normalize();
        }
        transform.position += dir*speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > 5.0f) Destroy(gameObject);
    }



    public void release() {
        released = true;
        transform.parent = null;
        //body.useGravity = true;
        target = Instantiate(empty, (player.transform.position+ new Vector3(0, 0.5f, 0)), Quaternion.identity);
        target.transform.parent = GameObject.Find("Level").transform;
        //Vector3 dir = ((player.transform.position + new Vector3(0,1f,0)) - transform.position);
        //dir.Normalize();
        //Debug.Log(dir);
        //body.AddForce(dir * 4000);
    }

    public void die()
    {
        for (int x = 0; x < 4; ++x)
            for (int y = 0; y < 4; ++y)
                for (int z = 0; z < 4; ++z)
                    CreateCube(new Vector3(x, y, z));

        Destroy(gameObject);
    }

    void CreateCube(Vector3 coordinates)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Renderer rd = cube.GetComponent<Renderer>();
        rd.material = GetComponent<Renderer>().material;

        cube.transform.localScale = transform.localScale*30;
        Vector3 fstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
        cube.transform.position = fstCube + Vector3.Scale(coordinates, cube.transform.localScale);

        cube.transform.parent = GameObject.Find("Level").transform;
        cube.layer = LayerMask.NameToLayer("UI");

        miniCube mc = cube.AddComponent<miniCube>();

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(200f, transform.position, 2.0f);
    }
}
