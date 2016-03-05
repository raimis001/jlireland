using UnityEngine;
using System.Collections;

public class BuildingUbags : Building
{
	public override float TiredModifier
	{
		get
		{
			Debug.Log("Ubags selected");
			if (GameManager.PlayerStatus == PlayerStatus.BAGGER)
			{
				return WorkTired;
			}
			return 0;
		}
	}

	public override void StartWorking()
	{
		GameManager.PlayerStatus = PlayerStatus.BAGGER;
		GUImain.ShowDialog(DialogKind.BAGGER);
		GameManager.Instance.HourTime = 2f;
		GameManager.GamePaused = false;

	}

	public void StopWorking()
	{
		GameManager.GamePaused = true;
		GameManager.PlayerStatus = PlayerStatus.NONE;
		GUImain.CloseAllDialogs();
	}

	protected override void OnCalculate()
	{
		if (GameManager.PlayerStatus != PlayerStatus.BAGGER) return;
		
		Parameters.get(ParamsKind.MONEY).Value += WorkSalary;
		Parameters.get(ParamsKind.HEALTH).Value += WorkHealth;
		
	}
}
