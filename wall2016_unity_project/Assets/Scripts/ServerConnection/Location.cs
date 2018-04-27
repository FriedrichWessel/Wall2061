using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Location
{
	private const int AllowedHackAttempts = 1;
	public string Id;
	public Dictionary<string, int> HackAttempts = new Dictionary<string, int>();
	public Dictionary<string, int> RecognizedHackers = new Dictionary<string, int>();

	public int GetNumberOfDiscoveredHackers()
	{
		return GetDiscoveredHackersNames().Count;
	}

	public void RecognizeDiscoveredHackers()
	{
		var names = GetDiscoveredHackersNames();
		
		foreach (var name in names)
		{
			if (!RecognizedHackers.ContainsKey(name.Key))
			{
				RecognizedHackers.Add(name.Key, name.Value);
			}
			
			RecognizedHackers[name.Key] = name.Value;
		}
	}

	private Dictionary<string, int> GetDiscoveredHackersNames()
	{
		var result = new Dictionary<string, int>();
		foreach (var attempt in HackAttempts)
		{
			if (attempt.Value >= AllowedHackAttempts)
			{
				if (!RecognizedHackers.ContainsKey(attempt.Key))
				{
					result.Add(attempt.Key, attempt.Value);
				}
				else if (this.HackAttempts.ContainsKey(attempt.Key)
				         && RecognizedHackers[attempt.Key] < this.HackAttempts[attempt.Key])
				{
					result.Add(attempt.Key, attempt.Value);
				}
			}
		}
		return result;
	}

	public void UpdateData(Location l2)
	{
		var activeAttempts = new Dictionary<string, int>();
		foreach (var hacker in l2.HackAttempts)
		{
			activeAttempts.Add(hacker.Key, hacker.Value);
			if (hacker.Value == 0 && RecognizedHackers.ContainsKey(hacker.Key))
			{
				RecognizedHackers.Remove(hacker.Key);
			}
		}
		this.HackAttempts = activeAttempts;
	}
}