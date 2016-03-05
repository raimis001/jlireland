﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class guiParameter : guiEvent
{

	public ParamsKind Parameter;

	public Text ValueText;
	public Slider Progress;

	float ParamValue = 0;

	// Use this for initialization
	void Start()
	{
		if (Progress) Progress.value = 0;
	}

	// Update is called once per frame
	void Update()
	{
		ParamsClass param = Parameters.get(Parameter);
		if (ParamValue != param.Value)
		{
			if (Progress) Progress.value = param.Progress;
			if (ValueText) ValueText.text = param.ValueString;
			ParamValue = param.Value;
		}
	}

	protected override void UpdateHour(int hurs)
	{
	}
}
