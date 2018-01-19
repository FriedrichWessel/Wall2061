using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public List<Spawner> Spawners = new List<Spawner>();
    public float StartFrequency = 5f;
    public float FrequencyDecrease = 0.1f;
    public GameObject GoodBlockObject;
    public GameObject BadBlockObject;

    public float ChanceForBadBlock = 0.1f;

    private float _currentFrequency = 5f;
    private float _spawnTimer = 0;
    private bool _gameRunning = false; 

	// Use this for initialization
	void Start () {
        _currentFrequency = StartFrequency;
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
    }
}
