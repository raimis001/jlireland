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
		GameManager.Instance.Bagger();
	}
}
