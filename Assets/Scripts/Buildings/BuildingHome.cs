﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingHome : Building
{

	[Header("Interface")]
	public Text HourText;
	public guiFurniture FurniturePanel;

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

	public override float TiredModifier {
		get
		{
			return GameManager.PlayerStatus == PlayerStatus.SLEEP ? -4 - RestModifier : -1; 
		}
	}

	public float RestModifier {
		get
		{

			float result = 6;
			Furniture furniture;

			if (Beds.Length > 0)
			{

				furniture = Beds[CurrentBed];
				result += furniture.Tired/2;
			}

			if (Shelfs.Length > 0)
			{
				furniture = Shelfs[CurrentShelf];
				result += furniture.Tired/2;
			}

			if (Freezers.Length > 0)
			{
				furniture = Freezers[CurrentFreezer];
				result += furniture.Tired/2;
			}

			if (Centers.Length > 0)
			{
				furniture = Centers[CurrentCenter];
				result += furniture.Tired/2;
			}

			return result;
		}

	}

	public override DialogParams WorkingInfo {
		get {
			return new DialogParams()
			{
				Caption = "TU GULI!",
				Description = "Saldus sapņus visiem maziem latvju bērniem\nIzkaisītiem pieaugušo pasalē!",
				AutoClose = false,
				ShowClose = true,
				CloseText = "Pamosties"
			};
		}
	}

	public override string ButtonText {
		get { return "Gulēt!"; }
	}

	public override void StartWorking()
	{
		GameManager.PlayerStatus = PlayerStatus.SLEEP;
		GameManager.Instance.HourTime = 1f;
		GameManager.GamePaused = false;
		GUImain.CloseAllDialogs();
		GUImain.ShowDialog(DialogKind.WORKING);
	}

	public override void StopWorking()
	{
		GameManager.PlayerStatus = PlayerStatus.NONE;
		GameManager.Instance.HourTime = 2f;
		GameManager.GamePaused = true;
		GUImain.CloseAllDialogs();
	}

	public override void OpenActionDialog()
	{
		Furniture furniture = Beds[CurrentBed];
		if (furniture == null)
		{
			return;
		}

		FurniturePanel.CaptionText.text = furniture.Name;
		FurniturePanel.NoteText.text = furniture.Description;
		FurniturePanel.TiredText.text = furniture.Tired.ToString();
		FurniturePanel.StatusText.text = "0%";

		GUImain.ShowDialog(DialogKind.FURNITURE);
	}

	public void SwitchFurniture(int toggle)
	{
		Furniture furniture = null;
		switch (toggle)
		{
			case 0:
				furniture = Beds[CurrentBed];
				break;
			case 1:
				furniture = Shelfs[CurrentShelf];
				break;
			case 2:
				furniture = Freezers[CurrentFreezer];
				break;
			case 3:
				furniture = Centers[CurrentCenter];
				break;
		}

		if (furniture == null)
		{
			return;
		}

		FurniturePanel.CaptionText.text = furniture.Name;
		FurniturePanel.NoteText.text = furniture.Description;
		FurniturePanel.TiredText.text = furniture.Tired.ToString();
		FurniturePanel.StatusText.text = "0%";

	}
}
