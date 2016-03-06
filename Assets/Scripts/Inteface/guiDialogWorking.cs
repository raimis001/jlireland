using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiDialogWorking : guiDialog
{
	public GameObject StopButton;


	override public void Open()
	{
		DialogParams paramsDialog = GameManager.SelectedBuilding.WorkingInfo;

		Open(paramsDialog.Caption, paramsDialog.Description,paramsDialog.AutoClose);

		StopButton.SetActive(paramsDialog.ShowClose);
		ButtonText.text = paramsDialog.CloseText;
	}

	public void StopWorking()
	{
		GameManager.SelectedBuilding.StopWorking();
	}
}
