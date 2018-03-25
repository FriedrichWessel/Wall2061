using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LocationTest  {
	private Location _testLocation;

	[SetUp]
	public void RunBeforeEveryTest()
	{
		_testLocation = new Location();
		_testLocation.Id = "TestLocation"; 
		_testLocation.HackAttempts = new Dictionary<string, int>();
	}

	[Test]
	public void GetNumberOfDiscoveredHackersShouldReturnCorrectNumber()
	{
		_testLocation.HackAttempts.Add("Hacker1", 3);
		_testLocation.HackAttempts.Add("Hacker2", 2);
		_testLocation.HackAttempts.Add("Hacker3", 3);
		var hackers = _testLocation.GetNumberOfDiscoveredHackers();
		Assert.AreEqual(2,hackers);
	}

	[Test]
	public void RecognizeHackersShouldAddKnownHackersToRecognizedHackers()
	{
		_testLocation.HackAttempts.Add("Hacker1", 3);
		_testLocation.HackAttempts.Add("Hacker2", 2);
		_testLocation.HackAttempts.Add("Hacker3", 3);
		_testLocation.RecognizeDiscoveredHackers();
		Assert.Contains("Hacker1", _testLocation.RecognizedHackers);
		Assert.Contains("Hacker3", _testLocation.RecognizedHackers);
		Assert.AreEqual(1, _testLocation.HackAttempts.Count);
	}

	[Test]
	public void UpdateToNewLocationDataShouldKeeprecognizedHackers()
	{
		_testLocation.HackAttempts.Add("Hacker1", 3);
		_testLocation.HackAttempts.Add("Hacker2", 2);
		_testLocation.HackAttempts.Add("Hacker3", 3);
		_testLocation.RecognizeDiscoveredHackers();
		var l2 = new Location();
		l2.HackAttempts = new Dictionary<string, int>();
		l2.HackAttempts.Add("Hacker1", 3);
		l2.HackAttempts.Add("Hacker2", 2);
		l2.HackAttempts.Add("Hacker3", 3);
		l2.HackAttempts.Add("Hacker4", 3);
		_testLocation.UpdateData(l2);
		Assert.AreEqual(2, _testLocation.RecognizedHackers.Count);
		Assert.AreEqual(1, _testLocation.GetNumberOfDiscoveredHackers());
	}

}
