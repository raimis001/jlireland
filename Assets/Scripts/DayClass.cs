using System;
using System.Collections;
using UnityEngine;

public class DayClass
{
	

	internal static bool WeekChanged = false;

	private static int weekHours = 168;
	private static int dayHours = 24;

	private static int _time;

	internal static int Time {
		get { return _time; }
		set
		{
			int oldWeek = Week;
			_time = value;
			//Debug.Log("Time changed:" + _time);
			WeekChanged = Week != oldWeek;
			//HourSmooth = Hour;
			GameManager.DoHourChange();

		}
	}
	internal static float TimeSmooth;

	internal static float HourSmooth
	{
		get { return TimeSmooth - Mathf.FloorToInt(TimeSmooth) + Hour; }
	}
	internal static int Hour {
		get {
			return Time - Week * weekHours - (Day - 1) * dayHours;
		}
	}
	internal static int Day {
		get {
			return (Time - Week * weekHours) / dayHours + 1;
		}
	}
	internal static int Week {
		get {
			return Time / weekHours;
		}
	}

	internal static bool CloseLights()
	{
		return Hour == 7;
	}

	internal static bool OpenLights()
	{
		return Hour == 19;
	}

	internal static bool DayLights()
	{
		return Hour > 7 && Hour < 21;
	}

	internal static void Update()
	{
		if (GameManager.GamePaused) return;

		TimeSmooth += UnityEngine.Time.smoothDeltaTime / GameManager.Instance.HourTime;

		int h = Mathf.FloorToInt(TimeSmooth);
		if (h != Time)
		{
			//Debug.Log("Time changed:" + h);
			Time = h;
		}
	}

	internal static void IncHour()
	{
		TimeSmooth += 1;
		Time = Mathf.FloorToInt(TimeSmooth);
	}

	internal static void Init(int InitHour)
	{
		TimeSmooth = InitHour;
		Time = InitHour;
	}
}

