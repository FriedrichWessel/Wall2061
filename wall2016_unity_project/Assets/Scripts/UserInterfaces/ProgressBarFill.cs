using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFill : MonoBehaviour {

    public Image FillObject = null;

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

        FillObject.fillAmount = TargetFillState;

        //FillObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,MaxValue * TargetFillState); 
    }
}
