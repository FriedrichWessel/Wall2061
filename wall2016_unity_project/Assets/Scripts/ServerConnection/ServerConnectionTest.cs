using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ServerConnectionTest  {
	private ServerConnection _testConnection;
	private string _testMission;

	[SetUp]
	public void RunBeforeEveryTest()
	{
		_testConnection = new GameObject("ServerConnection").AddComponent<ServerConnection>();
		_testMission = Resources.Load<TextAsset>("TestMission").text;
	}

	[TearDown]
	public void RunAfterEveryTest()
	{
		Object.Destroy(_testConnection.gameObject);
	}

	[Test]
	public void GetMissionDataFromResponseShouldReturnFittingMission()
	{
		var bytes = Encoding.UTF8.GetBytes(_testMission);
		var testResponse = new ResponseData();
		testResponse.Data = bytes;
		var loadedMission = _testConnection.GetMissionDataFromResponse(testResponse);
		Assert.AreEqual("Location1", loadedMission["Location1"].Id);
	}

	[Test]
	public void GetMissonFromNonJsonDataShouldReturnNull()
	{
		var testResponse = new ResponseData();
		testResponse.Data = Encoding.UTF8.GetBytes("");
		var loadedMission = _testConnection.GetMissionDataFromResponse(testResponse);
		Assert.IsNull(loadedMission);
	}
	
	[Test]
	public void GetMissonFromNonMissionDataShouldReturnNull()
	{
		var testResponse = new ResponseData();
		testResponse.Data = Encoding.UTF8.GetBytes("{\"Test\":1}");
		var loadedMission = _testConnection.GetMissionDataFromResponse(testResponse);
		Assert.IsNull(loadedMission);
	}
	
	[Test]
	public void GetMissonFromEmptyJsonShouldReturnEmptyMission()
	{
		var testResponse = new ResponseData();
		testResponse.Data = Encoding.UTF8.GetBytes("{}");
		var loadedMission = _testConnection.GetMissionDataFromResponse(testResponse);
		Assert.IsNotNull(loadedMission);
		Assert.AreEqual(0, loadedMission.Count);
	}

	[UnityTest]
	public IEnumerator GetLocationsShouldReturnAllActiveLocations()
	{
		yield return LoadTestMission();
		bool called = false;
		yield return _testConnection.GetLocations(data =>
		{
			called = true;
			var mission = _testConnection.GetMissionDataFromResponse(data);
			Assert.AreEqual("Location1", mission["Location1"].Id);
		});
		Assert.IsTrue(called);
	}

	[UnityTest]
	public IEnumerator LoadMissionOnServerShouldActivateMissionOnServer()
	{
		bool called = false; 
		yield return _testConnection.LoadMission("test.mf", data =>
		{
			called = true;
			var mission = _testConnection.GetMissionDataFromResponse(data);
			Assert.AreEqual("Location1", mission["Location1"].Id);
		});
		Assert.IsTrue(called);
	}
	
	[UnityTest]
	public IEnumerator AttackLocationShouldIncreaseAttackCounterForUser()
	{
		yield return LoadTestMission();
		var testAction = new LocationAction();
		testAction.LocationID = "Location1";
		testAction.UserID = "TestUser";
		bool called = false;
		yield return _testConnection.AttackLocation(testAction, data =>
		{
			called = true;
			var mission = _testConnection.GetMissionDataFromResponse(data);
			Assert.AreEqual(1, mission["Location1"].HackAttempts["TestUser"]);
			
		});  
		Assert.IsTrue(called);
	}
	
	[UnityTest]
	public IEnumerator FinishAttackShouldResetAttackCounterForUser()
	{
		yield return LoadTestMission();
		var testAction = new LocationAction();
		testAction.LocationID = "Location1";
		testAction.UserID = "TestUser";
		yield return AttackLocation(testAction);
		yield return AttackLocation(testAction);
		bool called = false;
		yield return _testConnection.FinishAttack(testAction, data =>
		{
			called = true;
			var mission = _testConnection.GetMissionDataFromResponse(data);
			Assert.AreEqual(0, mission["Location1"].HackAttempts["TestUser"]);
			
		});  
		Assert.IsTrue(called);
	}

	private IEnumerator LoadTestMission()
	{
		bool missionLoaded = false;
		yield return _testConnection.LoadMission("test.mf", data =>
		{
			missionLoaded = true;
		});
		while (!missionLoaded)
		{
			yield return null;
		}
	}

	private IEnumerator AttackLocation(LocationAction action)
	{
		bool attackFinished = false;
		yield return _testConnection.AttackLocation(action, data =>
		{
			attackFinished = true;
		});
		while (!attackFinished)
		{
			yield return null;
		}
	}
}
