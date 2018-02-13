using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject ObjectToSpawn;
    public GameObject OverrideNextObjectToSpawn;

    public float SpeedVariance = 0.02f;

    public bool IsSpawning = false;

    private Vector3 MovingSpeed = new Vector3();

    private float BlockSize = 0.7f;

    private float nextSpawnCounter = 0f; 


	// Use this for initialization
	void Start () {
        MovingSpeed = ObjectToSpawn.GetComponent<MovingObject>().MovingSpeed;
        MovingSpeed.z += Random.value*SpeedVariance;
        nextSpawnCounter = (BlockSize / Mathf.Abs(MovingSpeed.z)) * Time.deltaTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (IsSpawning)
        {
		    if(nextSpawnCounter <= 0)
            {
                if (OverrideNextObjectToSpawn != null)
                {
                    SpawnObject(OverrideNextObjectToSpawn);
                    OverrideNextObjectToSpawn = null;
                }
                else
                {
                    SpawnObject(ObjectToSpawn);
                }            
            }else
            {
                nextSpawnCounter -= Time.deltaTime;
            }
        }
	}

    public void SpawnObject(GameObject prefab)
    {
        GameObject newObject = GameObject.Instantiate(prefab, this.transform, false);
        newObject.transform.localPosition = new Vector3(0, 0, 0);
        newObject.GetComponent<MovingObject>().MovingSpeed = MovingSpeed;

        nextSpawnCounter = (BlockSize / Mathf.Abs(MovingSpeed.z))*Time.deltaTime; 
    }
}
