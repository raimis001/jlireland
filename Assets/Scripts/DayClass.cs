using System.Collections;
public class DayClass
{
	internal float HourTime = 2;
	internal bool WeekChanged = false;

	private const int weekHours = 168;
	private const int dayHours = 24;

	private int _time;

	internal int Time {
		get { return _time; }
		set
		{
			int oldWeek = Week;
			_time = value;

			WeekChanged = Week != oldWeek;
			HourSmooth = Hour;
			GameManager.DoHourChange();

		}
	}
	internal float HourSmooth;
	internal int Hour {
		get {
			return Time - Week * weekHours - (Day - 1) * dayHours;
		}
	}
	internal int Day {
		get {
			return (Time - Week * weekHours) / dayHours + 1;
		}
	}
	internal int Week {
		get {
			return Time / weekHours;
		}
	}

	internal bool CloseLights()
	{
		return Hour == 7;
	}

	internal bool OpenLights()
	{
		return Hour == 19;
	}

	internal bool DayLights()
	{
		return Hour > 7 && Hour < 19;
	}

	public void Inc(int hours, float time)
	{
		if (time == 0)
		{
			Time += hours;
			return;
		}
		GameManager.Instance.StartCoroutine(UpdateHour(hours, time));
	}


	IEnumerator UpdateHour(int hours, float time)
	{

		for (int i = 0; i < hours; i++)
		{
			yield return WaitForHour(time);
			if (GameManager.GameBreak || GameManager.GamePaused)
			{
				yield break;
			}
		}

		if (!GameManager.GamePaused)
		{
			Inc(1, HourTime);
		}
	}

	IEnumerator WaitForHour(float time)
	{
		float delta = time;
		while (delta > 0)
		{
			delta -= UnityEngine.Time.smoothDeltaTime;
			HourSmooth += (1f / time) * UnityEngine.Time.smoothDeltaTime;
			if (GameManager.GameBreak || GameManager.GamePaused)
			{
				yield break;
			}
			yield return null;
		}
		Time++;

	}
}

