using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class guiParamMoney : guiParameter
{

	public Text MiljonText;

	protected override void ParamsChanged(ParamsClass param)
	{
		float money = param.Value;
		if (money < 1000000)
		{
			Debug.Log("Les than milj");
			if (ValueText) ValueText.text = param.ValueString;
			if (MiljonText) MiljonText.text = "0";
			return;
		}

		Debug.Log("Ower miljon");
		int milj = Mathf.FloorToInt(money/1000000);
		if (MiljonText) MiljonText.text = milj.ToString();

		money -= milj*1000000;
		if (ValueText) ValueText.text = money.ToString("0");

	}
}
