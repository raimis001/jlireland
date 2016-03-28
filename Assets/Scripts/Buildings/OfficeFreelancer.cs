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
		int iq = Random.Range(-10, 10) + (int)Parameters.get(ParamsKind.IQ).Value;
		int hours = Random.Range(100, 300);

		int money = Random.Range(10, 20) * (iq / 18) * hours ;
		money = (money/100)*100;

		FreelancerWork work = new FreelancerWork()
		{
			Name = WorkNames.Random(),
			Hours = hours,
			Money = money,
			IQ = iq
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
			return Mathf.CeilToInt(Hours/6f);
			
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
	public GameObject Empty;
	public GameObject Grid;

	private FreelancerWork Work;
	private int AvailableWorks = 5;

	public override float TiredModifier
	{
		get
		{
			return GameManager.PlayerStatus == PlayerStatus.FREELANCER ? WorkTired : 1;
		}
	}

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



	public override void OpenWorkDialog()
	{

		if (Work == null && AvailableWorks > 0)
		{
			Work = FreelancerWork.Create();
		}

		if (Work == null)
		{
			NoWorks();
			return;
		}
		RedrawData();

		GUImain.ShowDialog(DialogKind.FREELANCER);
	}

	void RedrawData()
	{
		if (NewWork) NewWork.SetActive(!Work.Started);
		if (Working) Working.SetActive(Work.Started);
		if (Empty) Empty.SetActive(false);
		if (Grid) Grid.SetActive(true);
		if (CaptionText) CaptionText.gameObject.SetActive(true);

		CaptionText.text = Work.Name;
		IqText.text = Work.IQ.ToString();
		TimeText.text = Work.Days.ToString();
		MoneyText.text = Work.Money.ToString();
	}

	public override void StartWorking()
	{
		if (Work == null)
		{
			NoWorks();
			return;
		}

		Work.Started = true;

		if (NewWork) NewWork.SetActive(false);
		if (Working) Working.SetActive(true);
		if (Empty) Empty.SetActive(false);
		if (Grid) Grid.SetActive(true);
		if (CaptionText) CaptionText.gameObject.SetActive(true);


		Work.DueDate = DayClass.Time + Work.Days * 24;
		Work.DueHours = Work.Hours;

	}

	void NoWorks()
	{
		if (NewWork) NewWork.SetActive(false);
		if (Working) Working.SetActive(false);
		if (Empty) Empty.SetActive(true);
		if (Grid) Grid.SetActive(false);
		if (CaptionText) CaptionText.gameObject.SetActive(false);
	}

	public void OtherWork()
	{
		if (AvailableWorks < 1)
		{
			NoWorks();
			return;
		}

		AvailableWorks --;
		Work = FreelancerWork.Create();
		RedrawData();

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
		if (DayClass.Hour == 1)
		{
			//Jauni darbi
			AvailableWorks = 5;
		}

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
