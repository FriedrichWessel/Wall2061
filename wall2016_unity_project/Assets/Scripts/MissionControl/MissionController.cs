using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{

	public ServerConnection Connection;
	public LocationView LocationViewPrefab;

	public InputField MissionName;
	public InputField UserId;
	public InputField LocationId;
	
	private WaitForSeconds Wait;

	private Dictionary<string, LocationView> LocationViews = new Dictionary<string, LocationView>(); 
	
	// Use this for initialization
	void Start ()
	{
		Wait = new WaitForSeconds(2);
		StartCoroutine(PollData()); 
	}

	void OnDestroy()
	{
		StopAllCoroutines();
	}

	public void LoadMission()
	{
		StartCoroutine(Connection.LoadMission(MissionName.text, (d) => { }));
	}

	public void ClearUser()
	{
		var locationAction = new LocationAction();
		locationAction.LocationID = LocationId.text;
		locationAction.UserID = UserId.text;
		StartCoroutine(Connection.FinishAttack(locationAction, (d) => { }));
	}

	public void SaveMission()
	{
		StartCoroutine(Connection.SaveMission(MissionName.text, (d) => { }));
	}

	private IEnumerator PollData()
	{
		while (true)
		{
			bool called = false;
			yield return Connection.GetLocations(data =>
			{
				called = true;
				var mission = Connection.GetMissionDataFromResponse(data);
				RefreshUI(mission);
			});
			while (!called)
			{
				yield return null; 
			}
			yield return Wait;
		}
	}

	private void RefreshUI(Mission mission)
	{
		foreach (var location in mission)
		{
			if (!LocationViews.ContainsKey(location.Key))
			{
				AddLocationView(location.Value);
			}
			else
			{
				LocationViews[location.Key].UpdateData(location.Value);
			}
		}
	}

	private void AddLocationView(Location location)
	{
		var view = Object.Instantiate(LocationViewPrefab, this.transform);
		view.UpdateData(location);
		LocationViews.Add(location.Id, view);
	}
}
