using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCodeSnippet : MonoBehaviour {

    private TextMesh _textComponent;

    public CodeSnippetLibrary Library;

	// Use this for initialization
	void Start () {
        Library = GameObject.Find("CodeSnippetLibrary").GetComponent<CodeSnippetLibrary>();

        _textComponent = this.GetComponent<TextMesh>();

        _textComponent.text = Library.getRandomCodeSnippet();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
