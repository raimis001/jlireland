using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImain : MonoBehaviour
{

	static GUImain _instance = null;
	public static GUImain Instance {
		get { return _instance; }
	}

	public Toggle PauseButton;

	public guiDialog InfoDialog;

	[HideInInspector]
	public guiSideMenu DialogMenu;

	void Awake()
	{
		_instance = this;
		DialogMenu = GetComponent<guiSideMenu>();
		if (DialogMenu) Debug.Log("Dialog is seted");
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
	}

	public void PauseGame()
	{
		GameManager.GamePaused = !GameManager.GamePaused;
	}

	public void GotoCity()
	{
		if (DialogMenu) DialogMenu.CloseAll();
		GameManager.SelectedBuilding = null;
	}

	public void ShowInfo()
	{
		
	}

	public void OpenWorkDialog()
	{
		if (GameManager.SelectedBuilding)
		{
			GameManager.SelectedBuilding.OpenWorkDialog();		
		}
	
		//DialogMenu.SwitchDialog((int)DialogKind.WORKINFO);
	}

	public void CloseWorkDialog()
	{
		DialogMenu.CloseDialog((int)DialogKind.WORKINFO);
	}

	public void OpenActionDialog()
	{
		if (GameManager.SelectedBuilding)
		{
			GameManager.SelectedBuilding.OpenActionDialog();
		}
	}

	public static void ShowMessage(string caption, string note)
	{
		_instance.InfoDialog.Open(new DialogParams() {Caption = caption, Description = note, AutoClose = true});
	}

	public static DialogParams Dialog(int dialog, int hours = 0)
	{
		switch (dialog)
		{
			case 0:
				return new DialogParams() {Caption = "Tev jādodas uz darbu!", Description = GameManager.CurrentWork.GoToWorkDescription() , AutoClose = false};
			case 2:
				return new DialogParams() { Caption = "Saldus sapņus...", Description = string.Format("Nolēmi pagulēt {0} stundas. ", hours), AutoClose = false };
		}
		return null;
	}

	public void GoToBuilding(Building building)
	{
		GameManager.SelectedBuilding = building;
	}

	public static void ShowDialog(DialogKind dialog)
	{
		_instance.DialogMenu.SwitchDialog((int)dialog);
	}

	public static void CloseAllDialogs()
	{
		_instance.DialogMenu.CloseAll();
	}

}
