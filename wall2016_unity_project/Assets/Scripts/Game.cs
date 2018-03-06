using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public UIManager UIManager;
    public SpawnManager Spawner;
    public float IntroShowDuration = 5.0f;
    public float GameStartDelay = 2.0f;

    public int MaxLifeAmount = 5;
    public int CurrentLifeAmount = 5;

    public float HackGoalTime = 20f;
    public float CurrentHackTime = 0f;

    private float _spawnTimer = 0;
    private bool _gameRunning = false;

    public delegate void LifeLostEvent(); 
    public event LifeLostEvent LifeLost = () => { };

	// Use this for initialization
	void Start () {
        CurrentLifeAmount = MaxLifeAmount;
	}

    // Update is called once per frame
    void Update()
    {
        if(CurrentHackTime >= HackGoalTime)
        {
            UIManager.TargetGameState = UIManager.GameStates.InformationScreen;
            WinGame();
        }else
        {
            if(_gameRunning) CurrentHackTime += Time.deltaTime;
        }
    }


    public void StartGame()
    {
        //Show Intro, then start game
        UIManager.TargetGameState = UIManager.GameStates.IntroScreen;

        StartCoroutine("InitializeGame");
    }

    public IEnumerator InitializeGame()
    {
        yield return new WaitForSeconds(IntroShowDuration);
        UIManager.TargetGameState = UIManager.GameStates.Ingame;
        yield return new WaitForSeconds(GameStartDelay);
        StartSpawning();
    }

    public void StartSpawning()
    {
        _gameRunning = true;
        Spawner.StartSpawner();
    }

    public void BadBlockMissed()
    {
        //This function is called by the Destroyer when a Bad Code BLock is missed. 
        CurrentLifeAmount -= 1;
        LifeLost();
        if(CurrentLifeAmount == 0)
        {
            //Game Lost - go to Loss Screen
            UIManager.TargetGameState = UIManager.GameStates.LostGameScreen;
            StopGame();
        }
    }

    private void WinGame()
    {
        StopGame();
    }

    private void StopGame()
    {
        _gameRunning = false;
        Spawner.StopSpawning();
    }

}
