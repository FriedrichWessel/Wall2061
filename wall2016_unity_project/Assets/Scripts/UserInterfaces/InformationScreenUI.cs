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
        {"CommissarInfo", "Name: Stefan Bechtel\nOccupation: KoSi Commissar\n\nPersonality Analysis: Obedient, Quiet, Violent\n\nCharges: Multiple Murder - removed\nCurrent Status: Patroling in Kreuzberg\n\n---------------------------\nName: Avril Wrike\nOccupation: KoSi Commisar\n\nPersonality Analysis: Sadistic, Loyal, Manipulative\n\nCharges: None\n\nCurrent Status: Patroling in Kreuzberg" },
        {"VolkovInfo", "Name: Dr. Gabriela Volkov\nOccupation: KoSi Scientist\n\nPersonality Analysis: Sadistic, Manipulative, Intelligent\n\nCharges: Illegal Experiments - removed\n\nCurrent Status: Working in KoSi Lab X56" },
        {"AgentMcKenzie", "Name: Agent McKenzie\n\nOccupation: KoSi Obserer at Checkpoint Alpha\n\nPersoanlity Analysis: Focused, Hardliner, Stoic\n\nNotes: Family is marked for \"Safekeeping\" in case of crisis" },
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
