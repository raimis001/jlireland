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
	}

	override public void Open()
	{
		if (GameManager.SelectedBuilding == null)
		{
			if (AcceptWork) AcceptWork.interactable = false;
			return;
		}

		if (TxtWorkName) TxtWorkName.text = GameManager.SelectedBuilding.WorkName;
		if (TxtWorkHours) TxtWorkHours.text = GameManager.SelectedBuilding.WorkString;
		if (TxtWorkSalary) TxtWorkSalary.text = GameManager.SelectedBuilding.WorkSalary.ToString();
		if (AcceptWork) AcceptWork.interactable = !GameManager.CurrentWork.EqualsHash(GameManager.SelectedBuilding);
		if (ButtonText) ButtonText.text = GameManager.SelectedBuilding.ButtonText;

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
