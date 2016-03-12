using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public enum CourceStatus
{
	None, 
	Active,
	Ended
}

[Serializable]
public class Course
{
	public string Name;
	public int Money;
	public int Hours;
	public int IQ;
	public WorkingHours Working;
	public CourceStatus Status = CourceStatus.None;

	[HideInInspector]
	public int PrecessedHours;
}

public class BuildingSchool : Building
{
	public Course[] courseStudies;
	public Course[] dailyStudies;
	public Course[] nightStudies;

	[Header("GUI")]
	public guiCourse courseLine;
	public guiCourse dailyLine;
	public guiCourse nightLine;

	private Course currentCourse;

	private Course courseStudie;
	private Course dailyStudie;
	private Course nightStudie;

	public override float TiredModifier
	{
		get
		{
			return GameManager.PlayerStatus == PlayerStatus.STUDIE ? WorkTired : base.TiredModifier;
		}
	}

	public override DialogParams WorkingInfo
	{
		get { return new DialogParams()
		{
			Caption = "MĀCĪBAS",
			Description = "Tu apgūsti - " + currentCourse.Name + " \n" + currentCourse.Working.WorkString + "\nAtlikušas " + (currentCourse.Hours - currentCourse.PrecessedHours) + " stundas.",
			ShowClose = true,
			CloseText = "Bastot"
		}; }
	}

	public override void StartWorking()
	{
		if (currentCourse.Status == CourceStatus.None)
		{
			if (!GameManager.AddMoney(-currentCourse.Money))
			{
				currentCourse = null;
				return;
			}
			currentCourse.Status = CourceStatus.Active;
		}

		GameManager.PlayerStatus = PlayerStatus.STUDIE;
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
		if (GameManager.PlayerStatus != PlayerStatus.STUDIE || currentCourse == null)
		{
			return;
		}

		if (currentCourse.Working.CanWork())
		{
			currentCourse.PrecessedHours ++;
			if (currentCourse.PrecessedHours >= currentCourse.Hours)
			{
				//Corse ended
				Parameters.get(ParamsKind.IQ).Value += currentCourse.IQ;
				currentCourse.Status = CourceStatus.Ended;
				currentCourse = null;
				StopWorking();
			}
			return;
		}

		StopWorking();
	}

	public void StartCourse()
	{
		currentCourse = courseStudie;
		StartWorking();
	}
	public void StartDaily()
	{
		currentCourse = dailyStudie;
		StartWorking();
	}
	public void StartNight()
	{
		currentCourse = nightStudie;
		StartWorking();
	}

	public override void OpenWorkDialog()
	{
		foreach (Course course in courseStudies)
		{
			if (course.Status != CourceStatus.Ended)
			{
				courseStudie = course;
				courseLine.SetValues(course);
				courseLine.AcceptButton.interactable = (currentCourse == null || currentCourse.Equals(course)) && course.Working.CanWork();
				courseLine.ButtonText.text = currentCourse == null ? "Apgūt" : currentCourse.Equals(course) ? "Turpināt" : "";
				break;
			}
		}
		foreach (Course course in dailyStudies)
		{
			if (course.Status != CourceStatus.Ended)
			{
				dailyStudie = course;
				dailyLine.SetValues(course);
				dailyLine.AcceptButton.interactable = (currentCourse == null || currentCourse.Equals(course)) && course.Working.CanWork();
				dailyLine.ButtonText.text = currentCourse == null ? "Apgūt" : currentCourse.Equals(course) ? "Turpināt" : "";
				break;
			}
		}
		foreach (Course course in nightStudies)
		{
			if (course.Status != CourceStatus.Ended)
			{
				nightStudie = course;
				nightLine.SetValues(course);
				nightLine.AcceptButton.interactable = (currentCourse == null || currentCourse.Equals(course)) && course.Working.CanWork();
				nightLine.ButtonText.text = currentCourse == null ? "Apgūt" : currentCourse.Equals(course) ? "Turpināt" : "";
				break;
			}
		}

		GUImain.ShowDialog(DialogKind.SCHOOL);
	}
}
