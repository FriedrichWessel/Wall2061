using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCodeBlock : MonoBehaviour
{
    private GameObject ParentObject;
    public GameObject DestroyFx; 

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
        GameObject particleSys = Instantiate(DestroyFx, this.transform.position, new Quaternion());
        particleSys.transform.SetPositionAndRotation(this.transform.position, new Quaternion());
        GameObject.Destroy(ParentObject);
    }

}
