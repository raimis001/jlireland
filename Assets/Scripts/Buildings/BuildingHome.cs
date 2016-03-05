using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingHome : Building
{

	[Header("Interface")]
	public Text HourText;

	[Header("Furnitures")]
	[Space(2)]
	[Range(0, 4)] public int CurrentBed = 0;
	[Range(0, 4)] public int CurrentShelf = 0;
	[Range(0, 4)] public int CurrentFreezer = 0;
	[Range(0, 4)] public int CurrentCenter = 0;

	[Header("Arrays")]
	public Furniture[] Beds;
	public Furniture[] Shelfs;
	public Furniture[] Freezers;
	public Furniture[] Centers;

	private int SleepHour = 1;

	public override float TiredModifier {
		get {
			if (GameManager.PlayerStatus == PlayerStatus.SLEEP)
			{
				return -4 - RestModifier;
			}

			return 0f;
		}
	}

	public float RestModifier {
		get {

			float result = 6;
			Furniture furniture;

			furniture = Beds[CurrentBed];
			result += furniture.Tired / 2;

			furniture = Shelfs[CurrentShelf];
			result += furniture.Tired / 2;

			furniture = Freezers[CurrentFreezer];
			result += furniture.Tired / 2;

			furniture = Centers[CurrentCenter];
			result += furniture.Tired / 2;

			return result;
		}

	}


	protected override void SelectBuilding()
	{
		if (HourText) HourText.text = SleepHour.ToString() + "h";
	}

	public void ChangeSleepHour(int hour)
	{
		int tired = Mathf.CeilToInt(GameManager.Parameters[ParamsKind.TIRED].Value / RestModifier);

		//Debug.Log("Change sleeping hour tired:" + GameManager.Parameters[ParamsKind.TIRED].Value.ToString("0.00") + " modifier:" + RestModifier.ToString("0.00"));
		if (hour != 0)
		{
			SleepHour = Mathf.Clamp(SleepHour + hour, 1, tired);
		}
		else
		{
			SleepHour = tired; 
		}

		if (HourText) HourText.text = SleepHour.ToString() + "h";
	}

	public void Sleep()
	{

		GameManager.DoSleep(SleepHour);

		SleepHour = 1;
		if (HourText) HourText.text = SleepHour.ToString() + "h";
	}
}
