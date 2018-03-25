using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationView : MonoBehaviour
{
	[SerializeField] private Button LocationButton;
	[SerializeField] private Text OldHackAttemptsLabel;
	[SerializeField] private Text ActiveHackAttemptsLabel;
	[SerializeField] private Text LocationName;

	private Location _connectedLocation;

	public void UpdateData(Location connectedLoc)
	{
		if (_connectedLocation == null)
		{
			_connectedLocation = connectedLoc;
		}
		_connectedLocation.UpdateData(connectedLoc);
		RefreshView();
	}

	private void RefreshView()
	{
		var hackers = _connectedLocation.GetNumberOfDiscoveredHackers();
		if (hackers > 0)
		{
			LocationButton.image.color = Color.red;
		}
		else
		{
			LocationButton.image.color = Color.white;
		}
		ActiveHackAttemptsLabel.text = hackers.ToString();
		OldHackAttemptsLabel.text = _connectedLocation.RecognizedHackers.Count.ToString();
		LocationName.text = _connectedLocation.Id;
	}


	public void RecognizeHackers()
	{
		_connectedLocation.RecognizeDiscoveredHackers();
		RefreshView();
	}
}