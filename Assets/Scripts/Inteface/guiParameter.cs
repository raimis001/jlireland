using UnityEngine;
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
		Progress.value = 0;
	}

	// Update is called once per frame
	void Update()
	{
		ParamsClass param = GameManager.Parameters[Parameter];
		if (ParamValue != param.Value)
		{
			Progress.value = param.Progress;
			ValueText.text = param.ValueString;
			ParamValue = param.Value;
		}
	}

	protected override void UpdateHour(int hurs)
	{
	}
}
