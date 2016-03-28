using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class WorkingHours
{
	public int Start;
	public int End = 24;
	public bool[] Days = {true, true, true, true, true, true, true};

	public bool CanWork(bool travel = false)
	{
		return (DayClass.Hour >= (travel ? Start - 1 : Start) && DayClass.Hour <= End && Days[DayClass.Day - 1]);
	}
	public string WorkString
	{
		get
		{
			string week = string.Format("{0} {1} {2} {3} {4} {5} {6}",
							(Days[0] ? "P" : "<color=red>P:-</color>"),
							(Days[1] ? "O" : "<color=red>O:-</color>"),
							(Days[2] ? "T" : "<color=red>T:-</color>"),
							(Days[3] ? "C" : "<color=red>C:-</color>"),
							(Days[4] ? "Pk" : "<color=red>Pk:-</color>"),
							(Days[5] ? "Se" : "<color=red>Se:-</color>"),
							(Days[6] ? "Sv" : "<color=red>Sv:-</color>")
							);
			return string.Format("{0} - {1} d: {2}", Start, End, week);
		}
	}

}


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

	internal static string GetDate(float time)
	{
		int week = (int)(time / weekHours);
		int day = (int)((time - week * weekHours) / dayHours + 1);
		int hour = (int)(time - week * weekHours - (day - 1) * dayHours);

		return string.Format("nedēļa:{0} diena:{1} stunda:{2} ", week, day, hour);
	}

}

