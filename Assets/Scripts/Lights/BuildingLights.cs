using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class BuildingLights : BuildingWindows
{
	public Building Building;

	private Light[] lights;

	private bool opened;

	protected override void Start()
	{
		base.Start();
		lights = GetComponentsInChildren<Light>();

		opened = true;
		CloseAll();
	}

	protected override void CloseAll()
	{
		if (!opened) return;

		opened = false;
		base.CloseAll();
		foreach (Light l in lights)
		{
			l.enabled = false;
		}
	}

	protected override void OpenAll()
	{
		if (opened) return;

		opened = true;
		base.OpenAll();
		foreach (Light l in lights)
		{
			l.enabled = true;
		}
	}

	protected override void OnHourChange(int hour)
	{
		if (GameManager.CurrentHour.DayLights() || GameManager.CurrentHour.CloseLights())
		{
			CloseAll();
			return;
		}

		if (Building is Office && !Building.CanVisit() || !Building.CanWork())
		{
			CloseAll();
		}
		else
		{
			OpenAll();
		}

	}
}
