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
	public guiHint Hint;
	public Tutorial Tutorial;
	public GameObject Gizmos;

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
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (GameManager.PlayerStatus == PlayerStatus.NONE)
			{
				PauseGame();
			}
		}

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

	public static void ShowTiredMessage()
	{
		ShowMessage("NOGURUMS", "Esi pārāk noguris, lai ko pasāktu.\nPaguli, tad vari turpināt!");
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

	public void ShowHint(int tag)
	{
		if (!Hint)
		{
			Debug.LogError("Nav definēts hints");
			return;
		}

		//Debug.Log("Showing hint tag:" + tag);
		Hint.Show("Maita","Rāda gan");
	}

	public void ShowHint(Vector3 pos, string caption, string description)
	{
		if (!Hint)
		{
			Debug.LogError("Nav definēts hints");
			return;
		}

		Hint.transform.position = new Vector3(pos.x, Hint.transform.position.y, Hint.transform.position.z);
		Hint.Show(caption, description);
	}

	public void HideHint()
	{
		if (!Hint)
		{
			Debug.LogError("Nav definēts hints");
			return;
		}
		Hint.Hide();
	}

	public void RestartTutorial()
	{
		if (Tutorial) Tutorial.Restart();
	}

	public void SwitchGizmos(bool state)
	{
		Gizmos.SetActive(state);
	}

}
