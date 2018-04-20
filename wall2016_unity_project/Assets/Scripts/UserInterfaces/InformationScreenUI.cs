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
        {"AgentMcKenzie", "Name: Agent McKenzie\n\nOccupation: KoSi Observer at Checkpoint Alpha\n\nPersoanlity Analysis: Focused, Hardliner, Stoic\n\nNotes: Family is marked for \"Safekeeping\" in case of crisis" },
        {"VeronikaCatlow", "Name: Veronika Catlow\n\nOccupation: CEO of Megacorp Security Black Ops\n\nPersonality Analysis: Proud, Ambitous, Ruthless\n\nStatus: Overseeing Operation Rubikon in Berlin" },
        { "MartinHolbach", "Name: Martin Holbach\n\nOccupation: PR Director\n\nPersonality Analysis: Manipulative, Friendly, Secretive\n\nStatus: Executing team-internal obvervations" },
        {"LaraHolbach", "Name: Lara Holbach\n\nOccupation: PR Director of Recruitment\n\nPersonality Analysis: Open, Stone-hearted, Unforgiving\n\nStatus: Recruiting additional assets"},
        {"ZimitirMedina", "Name: Zimitir Medina\n\nOccupation: Head of Security\n\nPersonality Analysis: Brutal, Choleric, Solitary\n\nStatus: Unknown"},
        {"IwanSoto", "Name: Iwan Soto\n\nOccupation: Head of Cyber Security\n\nPersonality Analysis: Introvert, Intelligent, Iritable\n\nStatus: Cyberspace Security" },
        {"HaydenBerret","Name: Hayden Berret\n\nOccupation: Managing Director\n\nPersonality Analysis: Loyal, Two-Faced, Logical\n\nStatus: Keep up the farce" },
        {"RubikonEntry01", "Statusreport 27.04.2061 - Operation Rubikon\nSent by: Veronika Catlow\n\nArrived in Berlin for the final preperations - Exertnal assets have been aquired and stand at the ready. PonR estimated at 04/28 7pm" },
        {"RubikonEntry02", "Entry titled \"Rubikon Exit\"\n\n In case of unfixable associaton between Nova and the cause we have to make sure to find Aunt Sally\n\nPossible Candidates: Hayden Berret, Zimitir Medina, Martin Holbach, Iwan Soto" },
        {"RubikonEntry03", "Rubikon Meaning: In 49BC Julius Caesar crossed the river rubicon, that formed the northern boundary of Italy. This was seen as a declaration of war against the roman senate. With that, Romes imperial age began." },
        {"DemocraticHistory01", "Database Entry 23x2 - History of the Democratic Union\n\nThe democratic union originates in student protests at the free university over 15 years ago. A defining moment was when a student leader Yves Toten was found hanged and students believed the supposed suicide to be a murder by the Kombinat." },
        {"DemocraticHistory02", "24th June 2049\n\nEllie Summers, Treasurer of the Party, gives an interview, detailing how the party took money from foreign coporations to harm Megacorp Security. Party Leader Johann Patrovich gets arrested, soon after the party falls into chaos. " },
        {"DemocraticCurrent", "Current Analysis of Democratic Union Activities:\n\nThe party seems to rally under Johann Patrovich again, who was just released from prison. " },
        {"FreeFrontOrigin", "04th November 2058 - A series of robberies, attacks, bombings, known as the \"Robin Raids\" happen. Several small gangs are absoreded and united under a \"Red Turner\". The Free Front is formed.\n\n 02nd February 2059 The Free Front is designated as a domestic Terrorist group." },
        {"FreeFrontTurner", "Red Turner was a nobody, a trucker. Somehow he united gangs and found a fellowship that adore him. Reports indicated that Turner is weary of being a leader and might soon step down." },
        {"UtopionHistory","The Church of Techno Utopion was offically founded in December 2057, around a group of devout followers that did all the offical paperwork. Sources claim they follow an AI that supposedly became concious." },
        {"UtopionLeader", "The registered High Priest and head of the Church is Josephine Wu, a literature lecturer at Humboldt university, who quit her job and started the cult and following of the church. There are no reports on her motivations" }

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
