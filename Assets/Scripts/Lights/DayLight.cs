using System;
using UnityEngine;
using Random = UnityEngine.Random;


[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class DayLight : MonoBehaviour
{
	[System.Flags]
	private enum WeatherKind
	{
		Sunny = 1,
		Raining = 2,
		Foggy = 3
	}

	[Range(0,24)]
	public float CurrentHour;

	public Color CurrentColor;
	public float Intesity;

	[Header("Setup day")]
	public float MinIntensity = 0.2f;
	public Gradient nightDayFogColor;
	public AnimationCurve fogDensityCurve;

	[Header("Fog")]
	[Range(0f,0.005f)]
	public float fogDensity;
	public Color fogColor;
	public float currentFog;

	[Header("Weather")]
	public GameObject RainObject;
	private WeatherKind currentWeather = WeatherKind.Sunny;
	private float weatherTime;
	


	void Awake()
	{
	}

	// Use this for initialization
	void Start()
	{
		currentWeather = WeatherKind.Sunny;
		SetWeather();
	}

	// Update is called once per frame
	void Update()
	{
		if (Application.isPlaying)
		{
			CurrentHour = DayClass.HourSmooth;
		}

		float dot = CurrentHour/24f;
		CurrentColor = nightDayFogColor.Evaluate(dot);
		Intesity = fogDensityCurve.Evaluate(dot);

		//GetComponent<Light>().color = CurrentColor;
		RenderSettings.ambientLight = CurrentColor;

		RenderSettings.fogColor = fogColor;
		RenderSettings.fogDensity = currentFog;

		if (Math.Abs(currentFog - fogDensity) > 0.000005f)
		{
			currentFog = Mathf.Lerp(currentFog, fogDensity, 0.05f);
		}
		else
		{
			currentFog = fogDensity;
		}

		Light light = GetComponent<Light>();
		if (!light) return;

		light.intensity = Intesity + 0.2f;
		transform.eulerAngles = new Vector3(60f, 360f * dot, 0);

		if (GameManager.GamePaused)
		{
			return;
		}

		weatherTime += Time.deltaTime;
		if (weatherTime > 10f + Random.value*30f)
		{
			weatherTime = 0;
			ChangeWeather();
		}
	}

	private void ChangeWeather()
	{
		if (currentWeather == WeatherKind.Sunny && Random.value < 0.5f)
		{
			return;
		}
		WeatherKind old = currentWeather;
		currentWeather = Helper.GetRandomEnum<WeatherKind>();
		if (old == currentWeather)
		{
			return;
		}
		SetWeather();
	}

	private void SetWeather()
	{
		switch (currentWeather)
		{
			case WeatherKind.Sunny:
				fogDensity = 0;
				if (RainObject) RainObject.SetActive(false);
				break;
			case WeatherKind.Raining:
				fogDensity = 0.0035f;
				if (RainObject) RainObject.SetActive(true);
				break;
			case WeatherKind.Foggy:
				fogDensity = 0.0035f;
				if (RainObject) RainObject.SetActive(false);
				break;

		}
	}
}
