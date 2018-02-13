using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public List<Spawner> Spawners = new List<Spawner>();
    public GameObject GoodBlockObject;
    public GameObject BadBlockObject;

    public float BadBlockFrequency = 2f;
    public float BadBlockFrequencyDecrease = 0.1f;
    public float BadBlockFrequencyMin = 0.1f;

    private float _currentTimer = 0f;

    private bool _spawnerActive = false;


	// Use this for initialization
	void Start () {

	}

    // Update is called once per frame
    void Update()
    {
        if (_spawnerActive)
        {
            if(_currentTimer >= BadBlockFrequency)
            {
                //Spawn bad Block at random Spawner
                int _randomSpawner = (int) UnityEngine.Random.Range(0, Spawners.Count);
                Spawners[_randomSpawner].OverrideNextObjectToSpawn = BadBlockObject;
                _currentTimer = 0f;
                BadBlockFrequency -= BadBlockFrequencyDecrease;
                BadBlockFrequency = Mathf.Max(BadBlockFrequency, BadBlockFrequencyMin);
            }else
            {
                _currentTimer += Time.deltaTime; 
            }
        }
    }

    public void StartSpawner()
    {
        _spawnerActive = true;
        InitializeSpawners();
    }

    private void InitializeSpawners()
    {
        foreach (Spawner s in Spawners)
        {
            s.ObjectToSpawn = GoodBlockObject;
            s.IsSpawning = true;
        }
    }

    public void StopSpawning()
    {
        _spawnerActive = false;
        foreach (Spawner s in Spawners)
        {
            s.ObjectToSpawn = GoodBlockObject;
            s.IsSpawning = false;
            s.gameObject.SetActive(false);
        }
    }


}
