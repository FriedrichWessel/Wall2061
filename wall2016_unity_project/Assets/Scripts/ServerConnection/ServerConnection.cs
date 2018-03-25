using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnection : MonoBehaviour
{
	public string ServerIP = "http://localhost:8080/";

	public Mission GetMissionDataFromResponse(ResponseData responseData)
	{
		var json = System.Text.Encoding.UTF8.GetString(responseData.Data);
		try
		{
			var mission = LitJson.JsonMapper.ToObject<Mission>(json);
			return mission;
		}
		catch (LitJson.JsonException e)
		{
			Debug.LogWarning(e.Message);
			return null;
		}
	}

	public IEnumerator LoadMission(string fileName, Action<ResponseData> responseCallback)
	{
		var action = new FileAction();
		action.FileName = fileName;
		var data = JsonUtility.ToJson(action);
		yield return Post(this.ServerIP + "loadMission", data, responseCallback);
	}
	
	public IEnumerator AttackLocation(LocationAction testAction, Action<ResponseData> action)
	{
		var json = JsonMapper.ToJson(testAction);
		yield return Post(ServerIP + "enterLocation", json, action);
	}

	public IEnumerator GetLocations(Action<ResponseData> responseCallback)
	{
		yield return Get(this.ServerIP + "getLocations", responseCallback);
	}
	
	public IEnumerator FinishAttack(LocationAction testAction, Action<ResponseData> action)
	{
		var json = JsonMapper.ToJson(testAction);
		yield return Post(this.ServerIP + "finishAttack", json, action);
	}


	private IEnumerator Post(string url, string bodyJsonString, Action<ResponseData> responseCallback)
	{
		var request = new UnityWebRequest(url, "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
		request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return Send(responseCallback, request);
	}

	private IEnumerator Send(Action<ResponseData> responseCallback, UnityWebRequest request)
	{
		yield return request.Send();
		while (!request.downloadHandler.isDone)
		{
			yield return null;
		}
		var responseData = new ResponseData();
		responseData.Data = request.downloadHandler.data;
		responseData.ResponsCode = responseData.ResponsCode;
		responseCallback(responseData);
	}


	private IEnumerator Get(string url, Action<ResponseData> responseCallback)
	{
		var request = new UnityWebRequest(url, "GET");
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return Send(responseCallback, request);
	}

	private IEnumerator Send(string route, string data)
	{
		var request = UnityWebRequest.Post(this.ServerIP + route, data);
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


	
}

[Serializable]
public class FileAction
{
	public string FileName;
}

[Serializable]
public class LocationAction
{
	public string LocationID;
	public string UserID;
}

public class ResponseData
{
	public byte[] Data;
	public long ResponsCode;
}