using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	public static GameManager Instance { get { return _instance; } }

	public static bool GamePaused = false;

	public static Building SelectedBuilding;

	public delegate void HourChange(int hours);
	public static event HourChange OnHourChange;

	public static int CurrentHour = 0;

	private float HourTime = 2;
	private float _hourTime;

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start()
	{
		_hourTime = HourTime;
	}

	// Update is called once per frame
	void Update()
	{
		if (GamePaused) return;

		_hourTime -= Time.smoothDeltaTime;
		if (_hourTime <= 0)
		{
			CurrentHour++;
			if (OnHourChange != null)
			{
				OnHourChange(1);
			}
			_hourTime = HourTime;
		}
	}
}
