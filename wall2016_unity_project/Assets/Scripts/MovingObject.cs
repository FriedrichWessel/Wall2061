using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {
    public Vector3 MovingSpeed = new Vector3(0, 0.1f, 0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(MovingSpeed);
	}
}
