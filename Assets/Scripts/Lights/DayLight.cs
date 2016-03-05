using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class DayLight : MonoBehaviour
{

	[Range(0,24)]
	public float CurrentHour;

	public Color CurrentColor;
	public float Intesity;

	public Gradient nightDayFogColor;
	public AnimationCurve fogDensityCurve;


	void Awake()
	{
	}

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (EditorApplication.isPlaying)
		{
			CurrentHour = GameManager.CurrentHour.HourSmooth;
		}

		float dot = CurrentHour/24f;
		CurrentColor = nightDayFogColor.Evaluate(dot);
		Intesity = fogDensityCurve.Evaluate(dot);

		//GetComponent<Light>().color = CurrentColor;
		RenderSettings.ambientLight = CurrentColor;

		GetComponent<Light>().intensity = Intesity + 0.2f;
	}
}
