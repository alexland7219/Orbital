using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletObject;
    public GameObject topSlice;
    public GameObject midSlice;
    public GameObject lowSlice;

    private float rotateTopTimer;
    private float rotateMidTimer;
    private float rotateLowTimer;

    private bool rotationStartedTop;
    private bool rotationStartedMid;
    private bool rotationStartedLow;

    private bool flipTop;
    private bool flipMid;
    private bool flipLow;

    private const float rotSpeed = 40.0f;
    private const float totalTime = 180.0f / 40.0f;
    Health_Bar healthBarComponent;
    private int nCubes;
    public int hp;
    public int shield;
    // Start is called before the first frame update

    public Player player; // Script of the player
    private bool spawned;
    public int level; // IMPORTANT: TO CHANGE IN THE EDITOR


    void Start()
    {
        rotateTopTimer = 5.0f;
        rotateMidTimer = 4.0f;
        rotateLowTimer = 8.0f;

        rotationStartedTop = false;
        rotationStartedMid = false;
        rotationStartedLow = false;
        nCubes = 4;
        hp = 100;
        shield = 100;

        flipLow = true;
        flipMid = true;
        flipTop = false;

        spawned = false;

        healthBarComponent = gameObject.transform.Find("HealthCanvas/HealthBar").gameObject.GetComponent<Health_Bar>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            if (player.getLevel() >= level) spawned = true;
            else return;
        }


        if (hp != healthBarComponent.health) healthBarComponent.health = hp;
        if (shield != healthBarComponent.shield) healthBarComponent.shield = shield;
        if (hp <= 0) die();

        rotateLowTimer -= Time.deltaTime;
        rotateMidTimer -= Time.deltaTime;
        rotateTopTimer -= Time.deltaTime;

        if (rotateLowTimer < 0 && !rotationStartedLow){ rotationStartedLow = true; rotateLowTimer = totalTime;}
        if (rotateMidTimer < 0 && !rotationStartedMid){ rotationStartedMid = true; rotateMidTimer = totalTime;}
        if (rotateTopTimer < 0 && !rotationStartedTop){ rotationStartedTop = true; rotateTopTimer = totalTime;}

        if (rotationStartedTop){
            topSlice.transform.Rotate(Vector3.up, rotSpeed*Time.deltaTime);
            if (rotateTopTimer < 0)
            {
                // Shoot
                Vector3 spawnPosition = lowSlice.transform.position;
                spawnPosition.y += 0.1f;
                spawnPosition.x += 0.5f;
                GameObject bullObj;
                bullObj = Instantiate(bulletObject, spawnPosition, transform.rotation);


                Bullet script = bullObj.GetComponent<Bullet>();
                script.setEnemyBullet(true);

                if (flipTop) script.negateSpeed();
                else RotateObjectAroundY(bullObj, 180);

                flipTop = !flipTop;

                rotationStartedTop = false;
                rotateTopTimer = 5.0f;
            }
        } 

        if (rotationStartedMid){
            midSlice.transform.Rotate(Vector3.up, rotSpeed*Time.deltaTime);
            if (rotateMidTimer < 0)
            {                
                
                Vector3 spawnPosition = topSlice.transform.position;
                spawnPosition.y += 0.9f;
                GameObject bullObj;
                bullObj = Instantiate(bulletObject, spawnPosition, transform.rotation);


                Bullet script = bullObj.GetComponent<Bullet>();
                script.setEnemyBullet(true);

                if (flipMid) script.negateSpeed();
                else RotateObjectAroundY(bullObj, 180);

                flipMid = !flipMid;

                rotationStartedMid = false;
                rotateMidTimer = 5.0f;
            }
        } 

        if (rotationStartedLow){
            lowSlice.transform.RotateAround(topSlice.transform.position, Vector3.up, rotSpeed*Time.deltaTime);
            if (rotateLowTimer < 0)
            {
                // Shoot
                Vector3 spawnPosition = topSlice.transform.position;
                GameObject bullObj;
                bullObj = Instantiate(bulletObject, spawnPosition, transform.rotation);


                Bullet script = bullObj.GetComponent<Bullet>();
                script.setEnemyBullet(true);

                if (flipLow) script.negateSpeed();
                else RotateObjectAroundY(bullObj, 180);

                flipLow = !flipLow;

                rotationStartedLow = false;
                rotateLowTimer = 5.0f;
            }
        } 

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {

            Bullet otherBull = other.GetComponent<Bullet>();
            if (!otherBull.getIsEnemy()) {
                if (shield <= 0) hp -= 20;
                else shield -= 20;
                Destroy(other.gameObject);
            }
        }
    }

    void RotateObjectAroundY(GameObject obj, float angle)
    {
        // Get the current rotation of the object
        Vector3 currentRotation = obj.transform.rotation.eulerAngles;

        // Set the new Y-axis rotation
        currentRotation.y = angle;

        // Apply the new rotation to the object
        obj.transform.rotation = Quaternion.Euler(currentRotation);
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

        cube.transform.localScale = 3 * transform.localScale / nCubes;
        Vector3 fstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
        cube.transform.position = fstCube + Vector3.Scale(coordinates, cube.transform.localScale);

        cube.transform.parent = GameObject.Find("Level").transform;
        cube.layer = LayerMask.NameToLayer("UI");

        miniCube mc = cube.AddComponent<miniCube>();

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(200f , transform.position, 2.0f);
    }

}
