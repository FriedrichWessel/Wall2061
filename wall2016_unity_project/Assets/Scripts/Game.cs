using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public UIManager UIManager;


    public List<Spawner> Spawners = new List<Spawner>();
    public float StartFrequency = 5f;
    public float FrequencyDecrease = 0.1f;
    public GameObject GoodBlockObject;
    public GameObject BadBlockObject;

    public int MaxLifeAmount = 5;
    public int CurrentLifeAmount = 5;

    public float ChanceForBadBlock = 0.9f;
    public float ChanceForBadBlockIncrease = 0.05f;
    public float ChanceForBadBlockMax = 1f;

    private float _currentFrequency = 2f;
    private float _spawnTimer = 0;
    private bool _gameRunning = false;

    public delegate void LifeLostEvent(); 
    public event LifeLostEvent LifeLost = () => { };

	// Use this for initialization
	void Start () {
        _currentFrequency = StartFrequency;
        CurrentLifeAmount = MaxLifeAmount;
	}
	
	// Update is called once per frame
	void Update () {
        if (_gameRunning)
        {
            if (_spawnTimer >= _currentFrequency)
            {
                _spawnTimer = 0;
                int RandomSpawnerNumber = (int) Math.Floor((double)UnityEngine.Random.Range(0, Spawners.Count));
                float RandomBlock = UnityEngine.Random.value;
                GameObject ObjectToSpawn = GoodBlockObject;
                if (RandomBlock <= ChanceForBadBlock)
                {
                    ObjectToSpawn = BadBlockObject;
                    ChanceForBadBlock += ChanceForBadBlockIncrease;
                    ChanceForBadBlock = Mathf.Min(ChanceForBadBlock, ChanceForBadBlockMax);
                }

                Spawners[RandomSpawnerNumber].SpawnObject(ObjectToSpawn);
                _currentFrequency = Mathf.Max(1, _currentFrequency - FrequencyDecrease); 
            }else
            {
                _spawnTimer += Time.deltaTime;
            }
        }
	}

    public void StartGame()
    {
        _gameRunning = true;
        UIManager.SetUIToGame();
    }

    public void BadBlockMissed()
    {
        //This function is called by the Destroyer when a Bad Code BLock is missed. 
        CurrentLifeAmount -= 1;
        LifeLost();
    }
}
