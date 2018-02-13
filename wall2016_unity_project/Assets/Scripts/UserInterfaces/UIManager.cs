using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public Game GameManager; 

    public GameUI GameUI = null;
    public MenuUI MenuUI = null;
    public LostGameUI LostGameUI = null;
    public InformationScreenUI WonGameUI = null;

    public GameStates CurrentGameState = GameStates.None;
    public GameStates TargetGameState = GameStates.MainMenu;

    public enum GameStates
    {
        None,
        MainMenu, 
        Ingame, 
        InformationScreen,
        LostGameScreen
    }

	// Use this for initialization
	void Start () {
        GameManager.LifeLost += OnLifeLost;

    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentGameState != TargetGameState) SwitchToGameState(TargetGameState);
	}

    private void SwitchToGameState(GameStates targetGameState)
    {
        switch (targetGameState)
        {
            case GameStates.MainMenu:
                SetUIToMenu();
                break;
            case GameStates.Ingame:
                SetUIToGame();
                break;
            case GameStates.InformationScreen:
                SetUIToWon();
                break;
            case GameStates.LostGameScreen:
                SetUIToLost();
                break;
        }
    }

    private void SetUIToWon()
    {
        GameUI.gameObject.SetActive(false);
        MenuUI.gameObject.SetActive(false);
        LostGameUI.gameObject.SetActive(false);
        WonGameUI.gameObject.SetActive(true);
    }

    private void SetUIToGame()
    {
        GameUI.gameObject.SetActive(true);
        MenuUI.gameObject.SetActive(false);
        LostGameUI.gameObject.SetActive(false);
        WonGameUI.gameObject.SetActive(false);
    }

    private void SetUIToMenu()
    {
        GameUI.gameObject.SetActive(false);
        MenuUI.gameObject.SetActive(true);
        LostGameUI.gameObject.SetActive(false);
        WonGameUI.gameObject.SetActive(false);
    }

    private void SetUIToLost()
    {
        GameUI.gameObject.SetActive(false);
        MenuUI.gameObject.SetActive(false);
        LostGameUI.gameObject.SetActive(true);
        WonGameUI.gameObject.SetActive(false);
    }

    public void OnLifeLost()
    {
        GameUI.Healthbar.TargetFillState = (1 / (float)GameManager.MaxLifeAmount) * (float)GameManager.CurrentLifeAmount;
    }
}
