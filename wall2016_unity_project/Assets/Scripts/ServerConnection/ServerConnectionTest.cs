using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ServerConnectionTest  {
	private ServerConnection _testConnection;

	[SetUp]
	public void RunBeforeEveryTest()
	{
		_testConnection = new GameObject("ServerConnection").AddComponent<ServerConnection>();
	}

	[TearDown]
	public void RunAfterEveryTest()
	{
		Object.Destroy(_testConnection.gameObject);
	}

	[UnityTest]
	public IEnumerator AddLocationToServerShouldStoreLocationOnServer()
	{
		yield return _testConnection.LoadMission("test.mf");
		yield return _testConnection.GetLocations();
	}
}
