using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeSnippetLibrary : MonoBehaviour {

    //private List<string> _library = new List<string>();
    List<string> _randomCoddeWrods = new List<string>(new string[] {
        "if",
        "else",
        "ssh", 
        ");",
        "{  }",
        "",
        "Var=1",
        "Var=23",
        "List",
        "new",
        "public",
        "Hack",
        "void",
        "string",
        "int",
        "float",
        "connect",
        "IP 23.41",
        "(foobar",
        "Init()",
        "Exit()",
        "Meth4()",
    });


    public int MaxNumberOfLines = 5;
    public int MaxNumberOfWords = 3;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string getRandomCodeSnippet()
    {
        string randomString = "";
        for(int i=0; i < MaxNumberOfLines; i +=1 )
        {
            for(int j=0; j<MaxNumberOfWords; j += 1)
            {
                randomString += _randomCoddeWrods[(int)Random.Range(0, _randomCoddeWrods.Count)] +" ";
            }
            randomString += "\n";
        }
        return randomString;
    }


}
