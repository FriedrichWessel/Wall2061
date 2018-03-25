using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Location
{
	public string Id;
	public Dictionary<string, int> HackAttempts = new Dictionary<string, int>();
	public List<string> RecognizedHackers = new List<string>(); 
	
	public int GetNumberOfDiscoveredHackers()
	{
		return GetDiscoveredHackersNames().Count;
	}

	public void RecognizeDiscoveredHackers()
	{
		var names = GetDiscoveredHackersNames();
		foreach (var name in names)
		{
			RecognizedHackers.Add(name);
			HackAttempts.Remove(name);
		}
	}

	private List<string> GetDiscoveredHackersNames()
	{
		var result = new List<string>();
		foreach (var attempt in HackAttempts)
		{
			if (attempt.Value >= 3)
			{
				result.Add(attempt.Key);
			}
		}
		return result;
	}

	public void UpdateData(Location l2)
	{
		var activeAttempts = new Dictionary<string, int>();
		foreach (var hacker in l2.HackAttempts)
		{
			if (!RecognizedHackers.Contains(hacker.Key))
			{
				activeAttempts.Add(hacker.Key, hacker.Value);
			}
		}
		this.HackAttempts = activeAttempts;
	}
}