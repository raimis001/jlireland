using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class guiDate : guiEvent
{

	public Text HourText;
	public Text DayText;
	public Text WeekText;
	public Text FloatText;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (FloatText) FloatText.text = DayClass.HourSmooth.ToString("0.000");
	}

	protected override void UpdateHour(int hurs)
	{
		HourText.text = DayClass.Hour.ToString();
		DayText.text = DayClass.Day.ToString();
		WeekText.text = DayClass.Week.ToString();
	}
}
