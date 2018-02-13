using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCodeBlock : MonoBehaviour
{
    private GameObject ParentObject; 

    // Use this for initialization
    void Start()
    {
        ParentObject = this.GetComponentInParent<Transform>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        this.ScoreBlock();
    }

    public void ScoreBlock()
    {

        ParentObject.SetActive(false);
        GameObject.Destroy(ParentObject);
    }

}
