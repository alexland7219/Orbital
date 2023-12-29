using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEnemy : MonoBehaviour
{
    public float rotSpeed;
    public int hp;
    public bool shieldOn;

    private int nCubes;

    public float amplitude = 1.5f;  // Amplitude of the sine wave
    public float frequency = 1.5f;  // Frequency of the sine wave

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 20.0f;
        hp = 100;
        shieldOn = true;
        nCubes = 4;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - startTime));

        transform.position = new Vector3(transform.position.x, yPos + 5, transform.position.z);
        transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);

        if (hp <= 0) die();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hp -= 20;
        }
    }

    void die(){
        for (int x = 0; x < nCubes; ++x)
            for (int y = 0; y < nCubes; ++y)
                for (int z = 0; z < nCubes; ++z)
                    CreateCube(new Vector3(x, y, z));

        Destroy(gameObject);
    }

    void CreateCube(Vector3 coordinates)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Renderer rd = cube.GetComponent<Renderer>();
        rd.material = GetComponent<Renderer>().material;

        cube.transform.localScale = transform.localScale / nCubes;
        Vector3 fstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
        cube.transform.position = fstCube + Vector3.Scale(coordinates, cube.transform.localScale);

        cube.transform.parent = GameObject.Find("Level").transform;

        miniCube mc = cube.AddComponent<miniCube>();

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(200f , transform.position, 2.0f);
    }
}
