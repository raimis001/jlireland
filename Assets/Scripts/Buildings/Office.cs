using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Office : Building
{
	[Header("BUSSINESS")]
	public int BuyCost;
	public int[] InitIncome = new int[3];
	public int[] InitCost = new int[3];
	public int[] UpgradeCost = new int[2];

	public int InvestmentCost;

	[Range(0, 2)]
	public int Level;

	[Header("Parameters")]
	public int[] TiredLevels = new int[3];
	public int[] HealthLevels = new int[3];
	public int[] ComfortLevels = new int[3];

	[Header("Game Objects")]
	public GameObject[] Levels;

	[HideInInspector]
	public float Debt;

	[HideInInspector]
	public float Investment;

	public override float TiredModifier
	{
		get { return TiredLevels[Level]; }
	}

	public float HealthModifier
	{
		get { return HealthLevels[Level]; }
	}

	public float Income
	{
		get { return InitIncome[Level]; }
	}
	public float Cost {
		get { return InitCost[Level]; }
	}

	public int CurrentLevel
	{
		get { return Level; }
		set
		{
			Level = value;
			for (int i = 0; i < Levels.Length; i++)
			{
				Levels[i].SetActive(i == Level);
			}
		}
	}

	private int _level = -1;


	protected override void Update()
	{
		base.Update();
		if (!Application.isPlaying && _level != Level)
		{
			_level = Level;

			CurrentLevel = _level;

		}
	}

	public virtual void Invest()
	{
		
		if (!GameManager.AddMoney(-InvestmentCost))
		{
			return;
		}
		Investment += InvestmentCost;
	}

	public virtual void Buy()
	{
		if (!GameManager.AddMoney(-BuyCost))
		{
			return;
		}

		GameManager.BusinessList.Add(this);
	}


	public virtual void Upgrade()
	{
		if (Level < Levels.Length - 1)
		{
			if (!GameManager.AddMoney(-UpgradeCost[Level]))
			{
				return;
			}

				CurrentLevel++;
		}
	}

	override public void Calculate()
	{
		if (Debt > 0)
		{
			if (Parameters.get(ParamsKind.MONEY).Value >= Debt)
			{
				Parameters.get(ParamsKind.MONEY).Value -= Debt;
				Debt = 0;
			}
			else
			{
				Debt -= Parameters.get(ParamsKind.MONEY).Value;
				Parameters.get(ParamsKind.MONEY).Value = 0;
			}
			return;
		}

		if (DayClass.WeekChanged)
		{
			if (Parameters.get(ParamsKind.MONEY).Value >= Cost)
			{
				Parameters.get(ParamsKind.MONEY).Value -= Cost;
			}
			else
			{
				Debt += Cost;
			}
		}

		if (!CanWork() || Debt > 0)
		{
			return;
		}
		Parameters.get(ParamsKind.TIRED).Value += TiredModifier;
		Parameters.get(ParamsKind.HEALTH).Value += HealthModifier;
		Parameters.get(ParamsKind.MONEY).Value += Income;
	}
}
