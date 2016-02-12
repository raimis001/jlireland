using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	public static GameManager Instance { get { return _instance; } }

	public delegate void HourChange(int hours);
	public static event HourChange OnHourChange;

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
		_hourTime -= Time.smoothDeltaTime;
		if (_hourTime <= 0)
		{
			if (OnHourChange != null)
			{
				OnHourChange(1);
			}
			_hourTime = HourTime;
		}
	}
}
