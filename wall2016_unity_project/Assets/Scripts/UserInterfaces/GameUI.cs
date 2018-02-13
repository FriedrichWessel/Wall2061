using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : GUIBase {

    public ProgressBarFill Healthbar;
    public ProgressBarFill HackProgress;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HackProgress.TargetFillState = (1/GameManager.HackGoalTime) * GameManager.CurrentHackTime;
	}
}
