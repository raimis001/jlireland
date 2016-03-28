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
	public WorkingHours VisitingHours;

	[Header("Working")]
	public string WorkName;
	public WorkingHours WorkingHours;

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
			return WorkingHours.End - WorkingHours.Start;
		}
	}

	public string WorkString
	{
		get
		{
			return WorkingHours.WorkString;
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

	public virtual void OpenWorkDialog()
	{
		GUImain.ShowDialog(DialogKind.WORKINFO);
	}

	public virtual void OpenActionDialog()
	{
		
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

				if (DayClass.Hour == WorkingHours.End)
				{
					GameManager.PlayerStatus = PlayerStatus.NONE;
					GameManager.Instance.HourTime = 2f;
					GameManager.GamePaused = true;
					GUImain.CloseAllDialogs();
					Debug.Log("End working");
				}
			}
		}

		Debug.Log("Call calculate");
		OnCalculate();
	}

	protected virtual void OnCalculate()
	{
		
	}

	public bool CanVisit(bool travel = false)
	{
		return VisitingHours.CanWork(travel);
	}

	public bool CanWork()
	{
		return WorkingHours.CanWork();
	}

	public bool NeedToWork()
	{
		return DayClass.Hour == WorkingHours.Start - 1;
	}

	public void ShowCloseDialog()
	{
		StopWorking();
		GameManager.SelectedBuilding = null;
		GUImain.ShowMessage("AIZVĒRTS!", string.Format("Nevari apmeklēt {0}.\nApmeklējuma laiks no {1} līdx {2}", Name, VisitingHours.Start, VisitingHours.End));
	}

	public string GoToWorkDescription()
	{
		return string.Format("Darbs par {0}.\nJāstrādā no {1} līdz {2}.\nMaksa stundā {3}", WorkName, WorkingHours.Start, WorkingHours.End, WorkSalary);
	}

	void OnMouseUp()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;

		if (!CanVisit(true))
		{
			ShowCloseDialog();
			return;
		}

		if (!(this is MapBuilding))
		{
			DayClass.IncHour();
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
