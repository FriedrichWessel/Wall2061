using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnObject(GameObject prefab)
    {
        GameObject newObject = GameObject.Instantiate(prefab, this.transform, false);
        newObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
