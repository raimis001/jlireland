using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class guiDate : guiEvent
{

	public Text HourText;
	public Text DayText;
	public Text WeekText;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	protected override void UpdateHour(int hurs)
	{
		HourText.text = GameManager.CurrentHour.Hour.ToString();
		DayText.text = GameManager.CurrentHour.Day.ToString();
		WeekText.text = GameManager.CurrentHour.Week.ToString();
	}
}
