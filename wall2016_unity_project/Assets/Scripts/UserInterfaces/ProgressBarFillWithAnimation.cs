using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFillWithAnimation : MonoBehaviour
{

    public Animation FillAnimation = null;

    public float TargetFillState = 1f;
    private float CurrentFillState = 1f;

    private float MaxValue;
    private float MinValue = 0f;

    // Use this for initialization
    void Start()
    {
        MaxValue = FillAnimation.clip.length;
        CurrentFillState = TargetFillState;
    }

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        CurrentFillState = TargetFillState;

        FillAnimation.clip.SampleAnimation(FillAnimation.gameObject, MaxValue * TargetFillState);

        //FillObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,MaxValue * TargetFillState); 
    }
}
