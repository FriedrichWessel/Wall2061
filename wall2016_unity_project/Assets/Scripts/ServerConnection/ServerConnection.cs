using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnection : MonoBehaviour
{

	public string ServerIP = "http://localhost:8080/";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void AddLocation()
	{
	
	}

	public IEnumerator LoadMission(string fileName)
	{
		var action = new FileAction(); 
		action.FileName = fileName;
		var data = JsonUtility.ToJson(action);
		Debug.Log("Mission: " + data);
		yield return Post(this.ServerIP + "loadMission", data);
	}

	private IEnumerator Send(string route, string data)
	{
		
		var request = UnityWebRequest.Post(this.ServerIP+route,data );
		request.SetRequestHeader("Content-Type", "application/json");
		
		yield return request.SendWebRequest();

		if (request.isNetworkError || request.isHttpError)
		{
			Debug.Log(request.error);
		}
		else
		{
			Debug.Log("Form upload complete!");
		}
		
	}
	
	IEnumerator Post(string url, string bodyJsonString)
	{
		var request = new UnityWebRequest(url, "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
		request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
 
		yield return request.Send();
 
		Debug.Log("Status Code: " + request.responseCode);
	}

	public IEnumerator GetLocations()
	{
		yield return Get(this.ServerIP + "getLocations");
	}
	
	IEnumerator Get(string url)
	{
		var request = new UnityWebRequest(url, "GET");
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.Send();
 
		Debug.Log("Status Code: " + request.responseCode);
		Debug.Log("Data: " + request.downloadHandler.isDone);
		var json = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
		Debug.Log("Data: " + json);
		var mission = LitJson.JsonMapper.ToObject<Mission>(json);
		Debug.Log("M" + mission);
	}
}
[Serializable]
public class FileAction
{
	public string FileName;
}

[Serializable]
public class Mission : Dictionary<string, Location>
{
	
}

[Serializable]
public class Location
{
	public string Id;
	public Dictionary<string, int> HackAttempts;
}
