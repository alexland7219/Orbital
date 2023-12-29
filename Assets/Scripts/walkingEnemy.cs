using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkingEnemy : MonoBehaviour
{
    public float rotSpeed;
    public int hp;
    public bool shieldOn;
    public float angle;

    private float invincible;
    private int nCubes;

    Health_Bar healthBarComponent;

    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 20.0f;
        hp = 100;
        shieldOn = true;
        nCubes = 4;
        angle = 0;
        healthBarComponent = gameObject.transform.Find("HealthCanvas/HealthBar").gameObject.GetComponent<Health_Bar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (angle > 180 || angle < 0) { rotSpeed = -rotSpeed; }
        transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed * Time.deltaTime);
        angle += rotSpeed * Time.deltaTime;

        if (hp != healthBarComponent.health) healthBarComponent.health = hp;

        if (Input.GetKey(KeyCode.G) && invincible <= 0)
        {
            hp -= 20;
            invincible = 0.5f;
        }
        if (invincible > 0) invincible -= Time.deltaTime;

        if (hp <= 0) die();
    }

    void die()
    {
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
        rb.AddExplosionForce(200f, transform.position, 2.0f);
    }
}
