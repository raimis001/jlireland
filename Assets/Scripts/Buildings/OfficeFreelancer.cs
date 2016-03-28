using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FreelancerWork
{
	static List<string> WorkNames = new List<string>()
	{
		"Web lapa PHP",
		"Flash reklāma",
		"Web freimworks NodeJS",
		"Reklāmas projekts"
	};

	public static FreelancerWork Create()
	{
		int money = Random.Range(10, 20);
		int hours = Random.Range(100, 300);

		FreelancerWork work = new FreelancerWork()
		{
			Name = WorkNames.Random(),
			Hours = hours,
			Money = money * hours,
			IQ = Mathf.Floor(30 + (money - 10) * 2.5f),
		};

		return work;
	}

	public string Name;
	public int Money;
	public int Hours;
	public float IQ;

	public int DueDate;
	public float DueHours;

	public bool Started = false;

	public int Days
	{
		get
		{
			return Mathf.CeilToInt(Hours/8f);
			
		}
	}
}

public class OfficeFreelancer : Office
{

	[Header("Interface")]
	public Text CaptionText;
	public Text IqText;
	public Text TimeText;
	public Text MoneyText;
	public GameObject NewWork;
	public GameObject Working;

	private FreelancerWork Work;

	public override DialogParams WorkingInfo {
		get {
			return new DialogParams()
			{
				Caption = Work.Name,
				Description = "IQ: "+ Work.IQ + " peļņa: " + Work.Money + " \nAtlicis padarīt: " + ((Work.DueHours / Work.Hours) * 100f).ToString("0.00") + "%\nJāpadara līdz:\n  " + DayClass.GetDate(Work.DueDate),
				ShowClose = true,
				CloseText = "Atpūsties."
			};
		}
	}


	protected override void Start()
	{
		base.Start();
		if (Work == null)
		{
			Work = FreelancerWork.Create();
		}


	}

	public override void OpenWorkDialog()
	{
		if (Work == null)
		{
			return;
		}
		if (NewWork)
		{
			NewWork.SetActive(!Work.Started);
		}

		if (Working)
		{
			Working.SetActive(Work.Started);
		}

		CaptionText.text = Work.Name;
		IqText.text = Work.IQ.ToString();
		TimeText.text = Work.Days.ToString();
		MoneyText.text = Work.Money.ToString();

		GUImain.ShowDialog(DialogKind.FREELANCER);
	}

	public override void StartWorking()
	{
		if (Work == null)
		{
			return;
		}

		Work.Started = true;
		if (NewWork)
		{
			NewWork.SetActive(false);
		}
		if (Working)
		{
			Working.SetActive(true);
		}

		Work.DueDate = DayClass.Time + Work.Hours;
		Work.DueHours = Work.Hours;

	}

	public void DoWorking()
	{
		if (Work == null || !Work.Started)
		{
			return;
		}
		GameManager.PlayerStatus = PlayerStatus.FREELANCER;
		GUImain.ShowDialog(DialogKind.WORKING);
		GameManager.Continue(1f);
	}

	public override void StopWorking()
	{
		GameManager.PlayerStatus = PlayerStatus.NONE;
		GameManager.GamePaused = true;
		GUImain.CloseAllDialogs();
	}

	protected override void OnCalculate()
	{
		if (GameManager.PlayerStatus != PlayerStatus.FREELANCER || Work == null)
		{
			return;
		}

		if (DayClass.Time >= Work.DueDate)
		{
			//TODO: Neizpildīja darbu laikā jāsamazina peļņa
			Work.Money -= Mathf.CeilToInt(Work.Money*0.05f);
			if (Work.Money < 0) Work.Money = 0;
		}

		Work.DueHours -= Parameters.get(ParamsKind.IQ).Value/Work.IQ;
		Debug.Log(Work.DueHours);

		if (Work.DueHours <= 0)
		{
			//Darbs izpildīts
			GameManager.AddMoney(Work.Money);
			Work = null;
			GameManager.PlayerStatus = PlayerStatus.NONE;
			GameManager.GamePaused = true;
			GUImain.CloseAllDialogs();
		}

	}
}
