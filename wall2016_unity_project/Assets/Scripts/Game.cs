﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public UIManager UIManager;
	public SpawnManager Spawner;
	public ServerConnection Connection;

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

	public string UserID;
	public bool UserIsHacker = false;
	private bool HackerStatusReceived = false;
	public string LocationID = "Location1";

	private void Awake()
	{
		UserID = LoadUserId();
	}

	// Use this for initialization
	void Start()
	{
		CurrentLifeAmount = MaxLifeAmount;
	}

	private string LoadUserId()
	{
		if (PlayerPrefs.HasKey("UserDeviceID"))
		{
			return PlayerPrefs.GetString("UserDeviceID");
		}
		
		var rnd = new System.Random(DateTime.UtcNow.Millisecond + Guid.NewGuid().GetHashCode());
		var id = rnd.Next(10000000, 99999999);
		var userId = string.Format("{0}", id);
		PlayerPrefs.SetString("UserDeviceID", userId);
		return userId;
	}

	// Update is called once per frame
	void Update()
	{
		if (CurrentHackTime >= HackGoalTime)
		{
			UIManager.TargetGameState = UIManager.GameStates.InformationScreen;
			WinGame();
		}
		else
		{
			if (_gameRunning) CurrentHackTime += Time.deltaTime;
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
		yield return GetHackerStatusOfPlayer();
		while (!HackerStatusReceived)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (UserIsHacker)
		{
			//Go to tracking state
			LoseGame();
		}
		else
		{
			yield return new WaitForSeconds(IntroShowDuration);
			//Start game normally. 
			UIManager.TargetGameState = UIManager.GameStates.Ingame;
			yield return new WaitForSeconds(GameStartDelay);
			StartSpawning();
		}
	}

	private IEnumerator GetHackerStatusOfPlayer()
	{
		yield return Connection.GetLocations(data =>
		{
			var mission = Connection.GetMissionDataFromResponse(data);
			foreach (Location loc in mission.Values)
			{
				if (loc.HackAttempts.ContainsKey(UserID) && loc.HackAttempts[UserID] > 0)
				{
					UserIsHacker = true;
				}
			}
			HackerStatusReceived = true;
		});
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
		if (CurrentLifeAmount <= 0)
		{
			LoseGame();
		}
	}

	private void WinGame()
	{
		StopGame();
	}

	private void LoseGame()
	{
		UIManager.TargetGameState = UIManager.GameStates.LostGameScreen;
		StopGame();
		StartCoroutine("TrackHack");
	}

	public IEnumerator TrackHack()
	{
		yield return new WaitForSeconds(1);
		LocationAction locAction = new LocationAction();
		locAction.LocationID = LocationID;
		locAction.UserID = UserID;
		yield return Connection.AttackLocation(locAction, (n) => { });
	}

	private void StopGame()
	{
		_gameRunning = false;
		Spawner.StopSpawning();
	}

	public void SetInformation(string InfoID)
	{
		UIManager.WonGameUI.SetInformation(InfoID);
	}
}