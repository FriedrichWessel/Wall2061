using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarFill : MonoBehaviour {

    public GameObject FillObject = null;

    public float TargetFillState = 1f;
    private float CurrentFillState = 1f;

    private float MaxValue;
    private float MinValue = 0f;

	// Use this for initialization
	void Start () {
        MaxValue = this.GetComponentInParent<RectTransform>().rect.width;
        CurrentFillState = TargetFillState;
	}
	
	// Update is called once per frame
	void Update () {
        CurrentFillState = TargetFillState;

        FillObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,MaxValue * TargetFillState); 
    }
}
