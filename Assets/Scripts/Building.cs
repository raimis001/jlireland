using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEditor;
using UnityEngine.EventSystems;


[Serializable]
public class WorkWeek
{
	public WEEKDAYS day;
	public bool working = true;
}

public class Building : MonoBehaviour
{

	public GameObject Prefab;
	public GameObject UI;

	public string Name;
	[TextArea(3, 10)]
	public string Description;

	[Header("Visiting")]
	public int VisitStart = 0;
	public int VisitEnd = 24;

	[Header("Working")]
	public string WorkName;
	public int WorkStart = 0;
	public int WorkEnd = 24;

	[Header("Parameters")]
	public int WorkSalary;
	public float WorkTired = 2;
	public float WorkHealth = 0;

	
	public virtual string ButtonText
	{
		get { return "Pieņemt darbu!"; }
	}

	public virtual DialogParams WorkingInfo
	{
		get
		{
			return new DialogParams() {Caption = "Tu strādā!", Description = GoToWorkDescription(), AutoClose = false };
				
		}
	}


	public WorkWeek[] WeekDays =
	{
		new WorkWeek() { day = WEEKDAYS.PIRMDIENA, working = true },
		new WorkWeek() { day = WEEKDAYS.OTRDIENA, working = true },
		new WorkWeek() { day = WEEKDAYS.TRESDIENA, working = true },
		new WorkWeek() { day = WEEKDAYS.CETURTDIENA, working = true },
		new WorkWeek() { day = WEEKDAYS.PIEKTDIENA, working = true },
		new WorkWeek() { day = WEEKDAYS.SESTDIENA, working = true },
		new WorkWeek() { day = WEEKDAYS.SVETDIENA, working = true },
	};


	public virtual float TiredModifier
	{
		get
		{
			float result = GameManager.SelectedBuilding.EqualsHash(this) ? 1 : 0;
			return result;
		}
	}

	public int WorkTime
	{
		get
		{
			return WorkEnd - WorkStart;
		}
	}

	public string WorkString
	{
		get
		{
			string week = string.Format("{0} {1} {2} {3} {4} {5} {6}",
				(WeekDays[0].working ? "P" : "<color=red>P:-</color>"),
				(WeekDays[1].working ? "O" : "<color=red>O:-</color>"),
				(WeekDays[2].working ? "T" : "<color=red>T:-</color>"),
				(WeekDays[3].working ? "C" : "<color=red>C:-</color>"),
				(WeekDays[4].working ? "Pk" : "<color=red>Pk:-</color>"),
				(WeekDays[5].working ? "Se" : "<color=red>Se:-</color>"),
				(WeekDays[6].working ? "Sv" : "<color=red>Sv:-</color>")
				);
			return string.Format("{0} - {1} d: {2}", WorkStart, WorkEnd, week);
		}
	}

	// Use this for initialization
	protected virtual void Start()
	{

	}

	// Update is called once per frame
	protected virtual void Update()
	{

	}

	public virtual void Open()
	{

		if (Prefab) Prefab.SetActive(true);
		if (UI) UI.SetActive(true);
	}

	public virtual void CLose()
	{
		if (Prefab) Prefab.SetActive(false);
		if (UI) UI.SetActive(false);
	}

	public virtual void Calculate()
	{
		Parameters.get(ParamsKind.TIRED).Value += TiredModifier;

		//Check visiting
		if (!CanVisit())
		{
			ShowCloseDialog();
			return;
		}

		if (GameManager.CurrentWork)
		{
			if (!GameManager.CurrentWork.EqualsHash(this) && GameManager.CurrentWork.NeedToWork())
			{
				Debug.Log("Need goto work");
				GameManager.GamePaused = true;
				GUImain.ShowDialog(DialogKind.GOTO_WORK);
				return;
			}
		}

		if (GameManager.CurrentWork.EqualsHash(this))
		{
			if (CanWork())
			{
				if (GameManager.PlayerStatus != PlayerStatus.WORKING)
				{
					GameManager.PlayerStatus = PlayerStatus.WORKING;
					GameManager.Instance.HourTime = 0.5f;
					GameManager.GamePaused = false;
					GUImain.ShowDialog(DialogKind.WORKING);
					Debug.Log("Start working");
				}

				Parameters.get(ParamsKind.MONEY).Value += WorkSalary;
				Parameters.get(ParamsKind.TIRED).Value += WorkTired;
				Parameters.get(ParamsKind.HEALTH).Value += WorkHealth;

				if (DayClass.Hour == WorkEnd)
				{
					GameManager.PlayerStatus = PlayerStatus.NONE;
					GameManager.Instance.HourTime = 2f;
					GameManager.GamePaused = true;
					GUImain.CloseAllDialogs();
					Debug.Log("End working");
				}
			}
		}

		OnCalculate();
	}

	protected virtual void OnCalculate()
	{
		
	}

	public bool CanVisit()
	{
		return (DayClass.Hour >= VisitStart && DayClass.Hour <= VisitEnd);
	}

	public bool CanWork()
	{
		return (DayClass.Hour >= WorkStart && DayClass.Hour <= WorkEnd && WeekDays[DayClass.Day - 1].working);
	}

	public bool NeedToWork()
	{
		return DayClass.Hour == WorkStart - 1;
	}

	public void ShowCloseDialog()
	{
		GameManager.SelectedBuilding = null;
		GUImain.ShowMessage("AIZVĒRTS!", string.Format("Nevari apmeklēt {0}.\nApmeklējuma laiks no {1} līdx {2}", Name, VisitStart, VisitEnd));
	}

	public string GoToWorkDescription()
	{
		return string.Format("Darbs par {0}.\nJāstrādā no {1} līdz {2}.\nMaksa stundā {3}", WorkName, WorkStart, WorkEnd, WorkSalary);
	}

	void OnMouseUp()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;

		if (!CanVisit())
		{
			ShowCloseDialog();
			return;
		}

		Prefab.SetActive(true);
		GameManager.SelectedBuilding = this;
		SelectBuilding();
	}

	public virtual void StartWorking()
	{
		GameManager.CurrentWork = this;
	}

	public virtual void StopWorking()
	{
	}

	public virtual void QuitWorking()
	{
		GameManager.CurrentWork = null;
	}

	protected virtual void SelectBuilding()
	{
	}

}
