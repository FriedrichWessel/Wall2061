using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserLabel : MonoBehaviour
{

	public Game _game;

	public Text _label;
	// Use this for initialization
	void Start ()
	{
		_label.text = _game.UserID;
	}
	
}
