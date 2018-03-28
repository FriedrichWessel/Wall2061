using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationScreenUI : GUIBase {

    private string InformationToShow = null;

    private Dictionary<string, string> InfoDatabase = new Dictionary<string, string>()
    {
        {"CripplewareInfo", "Project Entry: Crippleware\nGoal: Control dissident citizens effectively\n\nIn Charge: Dr. Gabriela Volkov\n\nOverview: Dr. Volkov has created an implant that controls neurological pathways and makes subjects more suspectible to questioning and authority\n\nCurrent Status: First Testrun ongoing, awaiting results" },
        {"CommissarInfo", "Not here yet" },
        {"VolkovInfo", "Not here yet" },
        {"AgentMcKenzie", "Not here yet" },
    };

    public Text Informationtext = null;

	// Use this for initialization
	void Start () {

	}


    // Update is called once per frame
    void Update () {
		
	}

    public void SetInformation(string infoID)
    {
        InfoDatabase.TryGetValue(infoID, out InformationToShow);
        Informationtext.text = InformationToShow;
    }
}
