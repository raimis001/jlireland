using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiBuildingInfo : MonoBehaviour
{

	public Text Caption;
	public Text Description;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.SelectedBuilding)
		{
			if (Caption) Caption.text = GameManager.SelectedBuilding.Name;
			if (Description) Description.text = GameManager.SelectedBuilding.Description;
		}
	}
}
