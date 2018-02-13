using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public Game GameManager; 

    public GameUI GameUI = null;
    public MenuUI MenuUI = null;

	// Use this for initialization
	void Start () {
        GameManager.LifeLost += OnLifeLost;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUIToGame()
    {
        GameUI.gameObject.SetActive(true);
        MenuUI.gameObject.SetActive(false);
    }

    public void SetUIToMenu()
    {
        GameUI.gameObject.SetActive(false);
        MenuUI.gameObject.SetActive(true);
    }

    public void OnLifeLost()
    {
        GameUI.Healthbar.TargetFillState = (1 / (float)GameManager.MaxLifeAmount) * (float)GameManager.CurrentLifeAmount;
    }
}
