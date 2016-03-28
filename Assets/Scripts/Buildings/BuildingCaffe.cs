using UnityEngine;
using System.Collections;

public class BuildingCaffe : Building
{


	public void Process(int tag)
	{
		int money = 0;
		int well = 0;

		switch (tag)
		{
			case 0:
				money = 100;
				well = 5;
				break;
			case 1:
				money = 200;
				well = 7;
				break;
			case 2:
				money = 1000;
				well = 15;
				break;
		}

		if (GameManager.AddMoney(-money))
		{
			Parameters.get(ParamsKind.WELL).Value += well;
			DayClass.IncHour();
		}

	}

	public override void OpenWorkDialog()
	{
		GUImain.ShowDialog(DialogKind.BAR);
	}

}
