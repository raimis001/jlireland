using UnityEngine;
using System.Collections;


public class BuildingWindows : MonoBehaviour
{

	private SpriteRenderer[] windows;

	protected virtual void Start()
	{
		windows = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer win in windows)
		{
			win.gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		GameManager.OnHourChange += OnHourChange;
	}

	void OnDisabe()
	{
		GameManager.OnHourChange -= OnHourChange;
	}

	protected virtual void CloseAll()
	{
		foreach (SpriteRenderer win in windows)
		{
			win.gameObject.SetActive(false);
		}
	}

	protected virtual void OpenAll()
	{
		foreach (SpriteRenderer win in windows)
		{
			win.gameObject.SetActive(true);
		}
	}

	protected virtual void OnHourChange(int hour)
	{
		if (GameManager.CurrentHour.CloseLights())
		{
			CloseAll();
			return;
		}
		if (GameManager.CurrentHour.DayLights())
		{
			return;
		}

		foreach (SpriteRenderer win in windows)
		{
			if (win.gameObject.activeSelf && GameManager.CurrentHour.Hour > 0) continue;

			win.color = Random.ColorHSV();
			win.gameObject.SetActive(Random.value < 0.1f);
		}

	}


}
