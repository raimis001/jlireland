﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class CityLight : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		GetComponent<Light>().enabled = (GameManager.CurrentHour.Hour < 8 || GameManager.CurrentHour.Hour > 19);
	}
}