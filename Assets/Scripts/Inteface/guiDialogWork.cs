using UnityEngine;
using UnityEngine.UI;

public class guiDialogWork : guiDialog
{
	public Text TxtWorkName;
	public Text TxtWorkHours;
	public Text TxtWorkSalary;
	public Button AcceptWork;

	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnEnable()
	{
		Open();
	}
	public void Open()
	{
		if (GameManager.SelectedBuilding == null)
		{
			//Close();
			return;
		}

		if (TxtWorkName) TxtWorkName.text = GameManager.SelectedBuilding.WorkName;
		if (TxtWorkHours) TxtWorkHours.text = string.Format("{0} līdz {1}", GameManager.SelectedBuilding.WorkStart, GameManager.SelectedBuilding.WorkEnd);
		if (TxtWorkSalary) TxtWorkSalary.text = GameManager.SelectedBuilding.WorkSalary.ToString();
		if (AcceptWork) AcceptWork.interactable = GameManager.CurrentWork == null || GameManager.CurrentWork.GetHashCode() != GameManager.SelectedBuilding.GetHashCode();

		//gameObject.SetActive(true);
	}

	public void StartWork()
	{
		if (GameManager.SelectedBuilding == null)
		{
			return;
		}

		GameManager.SelectedBuilding.StartWorking();
		Open();
	}
}
