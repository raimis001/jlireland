using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiBuildingInfo : guiDialog
{


	public override void Open()
	{
		if (GameManager.SelectedBuilding)
		{
			if (CaptionText) CaptionText.text = GameManager.SelectedBuilding.Name;
			if (NoteText) NoteText.text = GameManager.SelectedBuilding.Description;
		}
	}
}
