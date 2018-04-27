using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFillWithAnimation : MonoBehaviour
{


    public float TargetFillState = 1f;
    private float CurrentFillState = 1f;

    private float MaxValue;
    private float MinValue = 0f;


    public Image Progress0;
    public Image Progress1;
    public Image Progress2;
    public Image Progress3;
    public Image Progress4; 

    // Use this for initialization
    void Start()
    {
        CurrentFillState = TargetFillState;
    }

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        CurrentFillState = TargetFillState;

       // FillAnimation.clip.SampleAnimation(FillAnimation.gameObject, MaxValue * TargetFillState);
        if (TargetFillState > 0)
        {
            Progress0.fillAmount = Mathf.Clamp(TargetFillState, 0, 0.2f).Remap(0, 0.2f, 0, 1);
        }
        if (TargetFillState > 0.2)
        {
            Progress0.fillAmount = 1; 
            Progress1.fillAmount = Mathf.Clamp(TargetFillState, 0, 0.4f).Remap(0.2f, 0.4f, 0, 1);
        }
        if (TargetFillState > 0.4)
        {
            Progress1.fillAmount = 1; 
            Progress2.fillAmount = Mathf.Clamp(TargetFillState, 0, 0.6f).Remap(0.4f, 0.6f, 0, 1);
        }
        if (TargetFillState > 0.6)
        {
            Progress2.fillAmount = 1; 
            Progress3.fillAmount = Mathf.Clamp(TargetFillState, 0, 0.8f).Remap(0.6f, 0.6f, 0, 1);
        }
        
        if (TargetFillState > 0.8)
        {
            Progress3.fillAmount = 1; 
            Progress4.fillAmount = Mathf.Clamp(TargetFillState, 0, 1f).Remap(0.8f, 1f, 0, 1);
        }

        //FillObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,MaxValue * TargetFillState); 
    }
    
   
}

public static class ExtensionMethods {
 
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
   
}
