using UnityEngine;
using System.Collections;

public class conBuilding : MonoBehaviour
{

	public Building ControlBuilding;

	void Start()
	{

	}

	private void OnEnable()
	{
		GameManager.OnHourChange += OnHourChange;
	}

	void OnDisabe()
	{
		GameManager.OnHourChange -= OnHourChange;
	}

	protected virtual void OnHourChange(int hour)
	{
		if (DayClass.DayLights())
		{
			Lights(false);
			return;
		}

		if (!ControlBuilding)
		{
			return;
		}
		Lights(ControlBuilding.CanVisit());
	}

	private void Lights(bool turn)
	{
		Constructor build = GetComponent<Constructor>();
		if (!build)
		{
			return;
		}
		build.LightsON = turn;
	}

}
