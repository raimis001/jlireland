using UnityEngine;
using System.Collections;

public class BuildingUbags : Building
{
	public override string ButtonText
	{
		get { return "Ubagot!"; }
	}

	public override DialogParams WorkingInfo
	{
		get
		{
			return new DialogParams()
			{
				Caption = "UBAGOŠANA!",
				Description = "Ziedojiet, ziedojiet, ziedojiet!\nJo vienīgais patiesais ubags es esmu!\nUn ziedojums pienākas man!",
				AutoClose = false,
				ShowClose = true,
				CloseText = "Pārtraukt"
			};
		}
	}
	public override void StartWorking()
	{
		GameManager.PlayerStatus = PlayerStatus.BAGGER;
		GUImain.ShowDialog(DialogKind.WORKING);
		GameManager.Instance.HourTime = 3f;
		GameManager.GamePaused = false;

	}

	public override void StopWorking()
	{
		GameManager.GamePaused = true;
		GameManager.Instance.HourTime = 2f;
		GameManager.PlayerStatus = PlayerStatus.NONE;
		GUImain.CloseAllDialogs();
	}

	protected override void OnCalculate()
	{
		if (GameManager.PlayerStatus != PlayerStatus.BAGGER) return;

		Parameters.get(ParamsKind.TIRED).Value += WorkTired;
		Parameters.get(ParamsKind.MONEY).Value += WorkSalary;
		Parameters.get(ParamsKind.HEALTH).Value += WorkHealth;
		
	}
}
