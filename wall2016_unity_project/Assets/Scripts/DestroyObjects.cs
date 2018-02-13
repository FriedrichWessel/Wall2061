using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour {

    public Game GameManager = null; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<DestroyBlockOnTouchdown>() != null)
        {
            if(other.gameObject.GetComponentInParent<ScoreCodeBlock>() != null)
            {
                //We got a bad Code Block, let the Game Manager know!
                GameManager.BadBlockMissed();
                GameObject.Destroy(other.transform.parent.gameObject);
            }else
            {
                GameObject.Destroy(other.gameObject);
            }
        }
    }
}
