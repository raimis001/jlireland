using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImain : MonoBehaviour
{
	public Text HourText;


	void OnEnable()
	{
		GameManager.OnHourChange += OnHourChange;
	}
	void OnDisable()
	{
		GameManager.OnHourChange -= OnHourChange;
	}
	// Use this for initialization
	void Start()
	{
		OnHourChange(0);
  }

	// Update is called once per frame
	void Update()
	{

	}


	void OnHourChange(int hours)
	{
		HourText.text = GameManager.CurrentHour.ToString();
	}
}
