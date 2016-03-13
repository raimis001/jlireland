using UnityEngine;
using System.Collections;
using System;

public enum TreatKind
{
	Onetime,
	Untilheal,
	Breakable
}

[Serializable]
public class Treat
{
	public string Name;
	public int Money;
	public int Hours;
	public int Heath;
	public WorkingHours Working;
	public TreatKind Kind;

	[HideInInspector]
	public int PrecessedHours;
}
public class BuildingHospital : Building
{
	public Treat TreatQuick;
	public Treat TreatMedium;
	public Treat TreatLong;

	[Header("GUI")]
	public guiCourse quickLine;
	public guiCourse mediumLine;
	public guiCourse longLine;

	private Treat CurrentTreat;
	public override float TiredModifier
	{
		get
		{
			return GameManager.PlayerStatus == PlayerStatus.HEALING ? -6 : 1;
		}
	}

	public override DialogParams WorkingInfo
	{
		get
		{
			return new DialogParams()
			{
				Caption = "ĀRSTĒŠANĀS!",
				Description = "Tu ārstējies - " + CurrentTreat.Name + "\nAtlikušas " + (CurrentTreat.Hours - CurrentTreat.PrecessedHours) + " stundas.",
				ShowClose = CurrentTreat.Kind == TreatKind.Breakable,
				CloseText = "Pārtraukt."
			};
		}
	}

	public override void OpenWorkDialog()
	{
		if (quickLine)
		{
			quickLine.SetValues(TreatQuick);
			quickLine.ButtonText.text = "Ārstēties.";
		}
		if (mediumLine)
		{
			mediumLine.SetValues(TreatMedium);
			mediumLine.ButtonText.text = "Ārstēties.";
		}
		if (longLine)
		{
			longLine.SetValues(TreatLong);
			longLine.ButtonText.text = "Ārstēties.";
		}

		GUImain.ShowDialog(DialogKind.HOSPITAL);
	}

	protected override void OnCalculate()
	{
		if (GameManager.PlayerStatus != PlayerStatus.HEALING || CurrentTreat == null)
		{
			return;
		}

		Parameters.get(ParamsKind.HEALTH).Value += CurrentTreat.Heath;

		if (CurrentTreat.Kind == TreatKind.Onetime)
		{
			StopWorking();
			return;
		}
		if (Parameters.get(ParamsKind.HEALTH).Value >= Parameters.get(ParamsKind.HEALTH).MaxValue)
		{
			StopWorking();
			return;
		}

		if (!GameManager.AddMoney(-CurrentTreat.Money))
		{
			StopWorking();
		}

	}

	public override void StopWorking()
	{
		CurrentTreat = null;
		GameManager.PlayerStatus = PlayerStatus.NONE;
		GameManager.GamePaused = true;
		GUImain.CloseAllDialogs();
	}
	public override void StartWorking()
	{

			if (!GameManager.AddMoney(-CurrentTreat.Money))
			{
				CurrentTreat = null;
				return;
			}


		GameManager.PlayerStatus = PlayerStatus.HEALING;
		GUImain.ShowDialog(DialogKind.WORKING);
		GameManager.Continue(1f);
	}

	public void StartTreat(int treatTag)
	{
		switch (treatTag)
		{
			case 0:
				CurrentTreat = TreatQuick;
				break;
			case 1:
				CurrentTreat = TreatMedium;
				break;
			case 2:
				CurrentTreat = TreatLong;
				break;
		}
		StartWorking();
	}
}

