using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiDialogWorking : guiDialog
{
	public GameObject StopButton;

	void OnEnable()
	{
		GameManager.OnHourChange += OnHourChanged;
	}

	void OnDisable()
	{
		GameManager.OnHourChange -= OnHourChanged;
	}

	void OnHourChanged(int hour)
	{
		Redraw();
	}

	override public void Open()
	{
		DialogParams paramsDialog = GameManager.SelectedBuilding.WorkingInfo;

		Open(paramsDialog.Caption, paramsDialog.Description,paramsDialog.AutoClose);

		Redraw();
	}

	void Redraw()
	{
		DialogParams paramsDialog = GameManager.SelectedBuilding.WorkingInfo;

		if (CaptionText) CaptionText.text = paramsDialog.Caption;
		if (NoteText) NoteText.text = paramsDialog.Description;

		StopButton.SetActive(paramsDialog.ShowClose);
		ButtonText.text = paramsDialog.CloseText;

	}

	public void StopWorking()
	{
		GameManager.SelectedBuilding.StopWorking();
	}
}
