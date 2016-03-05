using UnityEngine;
using System.Collections;

public class guiSideMenu : MonoBehaviour
{

	public int CurrentDialog;

	public guiDialog[] Dialogs;

	public void SwitchDialog(int dialog)
	{

		for (int i = 0; i < Dialogs.Length; i++)
		{
			if (Dialogs[i] == null) continue;

			if (i == dialog)
			{
				Dialogs[i].Open(GUImain.Dialog(dialog));
				CurrentDialog = dialog;
			}
			else
			{
				Dialogs[i].Close();
			}
		}
	}

	public void CloseDialog(int dialog)
	{
		Dialogs[dialog].Close();

	}
	public void CloseAll()
	{
		for (int i = 0; i < Dialogs.Length; i++)
		{
			if (Dialogs[i] == null)
			{
				continue;
			}

			Dialogs[i].Close();
		}
	}

	// Use this for initialization
	void Start()
	{
		CloseAll();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
