using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class guiDialogBusiness : guiDialog
{

	public GameObject ActivePanel;
	public GameObject NoActivePanel;

	public Text BuyText;
	public Text UpgradeText;
	public Text InvestmentText;

	public Text HoursText;
	public Text[] IncomeText;
	public Text[] CostText;

	public Text[] ParamsTexts;



	[Header("DEPT")]
	public GameObject DeptLine;
	public Text DeptText;

	// Use this for initialization
	void Start()
	{

	}

	void OnEnable()
	{
		UpdateValues();
	}

	private void UpdateValues()
	{
		Office office = (Office)GameManager.SelectedBuilding;
		if (office == null)
		{
			return;
		}

		bool active = GameManager.BusinessList.Contains(office);

		NoActivePanel.SetActive(!active);
		ActivePanel.SetActive(active);

		HoursText.text = office.WorkString;

		if (!active)
		{
			NoteText.text = office.Description;
			BuyText.text = office.BuyCost.ToString();
			IncomeText[0].text = office.InitIncome[0].ToString();
			CostText[0].text = office.InitCost[0].ToString();
		}
		else
		{
			IncomeText[1].text = office.InitIncome[0].ToString();
			CostText[1].text = office.InitCost[0].ToString();
			UpgradeText.text = office.UpgradeCost[office.Level].ToString();

			ParamsTexts[0].text = office.TiredLevels[office.Level].ToString();
			ParamsTexts[1].text = office.HealthLevels[office.Level].ToString();
			ParamsTexts[2].text = office.ComfortLevels[office.Level].ToString();

			DeptLine.SetActive(office.Debt > 0);
			DeptText.text = office.Debt.ToString();

			InvestmentText.text = office.InvestmentCost.ToString();
		}

		CaptionText.text = office.Name;
	}

	public void BuyBusiness()
	{
		GameManager.Instance.BuyBusiness();
		UpdateValues();
	}

	public void Invest()
	{
		Office office = (Office)GameManager.SelectedBuilding;
		if (office == null)
		{
			return;
		}
		
		office.Invest();
		UpdateValues();

	}

}
