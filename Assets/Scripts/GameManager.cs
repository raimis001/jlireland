using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ParamsClass
{
	private float _value;
	internal float Value {
		set {
			if (MaxValue > 0)
			{
				_value = Mathf.Clamp(value, 0, MaxValue);
			}
			else
			{
				_value = value;
			}
			
		}
		get {
			return _value;
		}
	}
	internal string ValueString {
		get {
			return Value.ToString();
		}
	}
	internal float MaxValue;

	internal float Progress {
		get {
			return Value / MaxValue;
		}
	}
}

public class Parameters
{
	private Dictionary<ParamsKind, ParamsClass> Params = new Dictionary<ParamsKind, ParamsClass>();

	internal ParamsClass this[ParamsKind index] {
		get {
			return Params[index];
		}
	}

	public Parameters()
	{
		Params.Add(ParamsKind.TIRED, new ParamsClass() { Value = 0, MaxValue = 100 });
		Params.Add(ParamsKind.HEALTH, new ParamsClass() { Value = 100, MaxValue = 100 });
		Params.Add(ParamsKind.MONEY, new ParamsClass() { Value = 5000, MaxValue = -1 });

	}
}


public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	public static GameManager Instance { get { return _instance; } }

	private static bool _gamePaused = false;
	public static bool GamePaused {
		get {
			return _gamePaused;
		}
		set
		{
			bool oldPause = _gamePaused;
			_gamePaused = value;
			_instance.PauseButton.isOn = !value;

			if (!_gamePaused && oldPause)
			{
				CurrentHour.Inc(1, CurrentHour.HourTime);
			}
		}
	}

	public static bool GameBreak;

	private static Building _selectedBuilding;
	public static Building SelectedBuilding {
		get {
			return _selectedBuilding;
		}
		set {
			if (_selectedBuilding) _selectedBuilding.CLose();
			_selectedBuilding = value;
			if (_selectedBuilding)
			{
				GamePaused = true;
				_instance.MainCamera.gameObject.SetActive(false);
				_selectedBuilding.Open();
			} else
			{
				GamePaused = false;
				_instance.MainCamera.gameObject.SetActive(true);
			}
		}
	}

	public delegate void HourChange(int hours);
	public static event HourChange OnHourChange;

	public static DayClass CurrentHour = new DayClass();
	public static Building CurrentWork;

	public static PlayerStatus PlayerStatus = PlayerStatus.NONE;

	internal static Parameters Parameters = new Parameters();

	public static List<Office> BusinessList = new List<Office>();
		
	[Header("Interface")]
	public Toggle PauseButton;

	public float HourTime = 2.5f;
	public Camera MainCamera;

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start()
	{
		CurrentHour.Time = 7;
		DoHourChange();
		CurrentHour.HourTime = HourTime;
		CurrentHour.Inc(1, CurrentHour.HourTime);
	}

	// Update is called once per frame
	void Update()
	{
		CurrentHour.HourTime = HourTime;
	}

	public void OnPauseGame(bool button)
	{
		bool oldPause = _gamePaused;
		_gamePaused = !button;
		if (!_gamePaused && oldPause)
		{
			CurrentHour.Inc(1, CurrentHour.HourTime);
		}
	}

	public static void DoHourChange()
	{

		Parameters[ParamsKind.TIRED].Value += 4;

		foreach (Office office in BusinessList)
		{
			if (office.Debt > 0)
			{
				if (Parameters[ParamsKind.MONEY].Value >= office.Debt)
				{
					Parameters[ParamsKind.MONEY].Value -= office.Debt;
					office.Debt = 0;
				}
				else
				{
					office.Debt -= Parameters[ParamsKind.MONEY].Value;
					Parameters[ParamsKind.MONEY].Value = 0;
				}
				continue;
			}

			if (CurrentHour.WeekChanged)
			{
				if (Parameters[ParamsKind.MONEY].Value >= office.Cost)
				{
					Parameters[ParamsKind.MONEY].Value -= office.Cost;
				}
				else
				{
					office.Debt += office.Cost;
				}
			}

			if (!office.CanWork() || office.Debt > 0)
			{
				continue;
			}
			Parameters[ParamsKind.TIRED].Value += office.TiredModifier;
			Parameters[ParamsKind.HEALTH].Value += office.HealthModifier;
			Parameters[ParamsKind.MONEY].Value += office.Income;

			//TODO ieviest comforta modifieri
		}


		if (SelectedBuilding)
		{
			Parameters[ParamsKind.TIRED].Value += SelectedBuilding.TiredModifier;

			if (!SelectedBuilding.CanVisit())
			{
				SelectedBuilding.ShowCloseDialog();
				SelectedBuilding = null;
				if (OnHourChange != null) OnHourChange(1);
				return;
			}
		}

		if (PlayerStatus == PlayerStatus.BAGGER)
		{
			Parameters[ParamsKind.MONEY].Value += SelectedBuilding.WorkSalary;
			Parameters[ParamsKind.HEALTH].Value += SelectedBuilding.WorkHealth;
		}

		if (CurrentWork)
		{
			if (CurrentWork.NeedToWork())
			{
				if (!CurrentWork.EqualsHash(SelectedBuilding))
				{
					//TODO: need goto work
					GameBreak = true;
					GamePaused = true;
					GUImain.ShowDialog(DialogKind.GOTO_WORK);
					if (OnHourChange != null) OnHourChange(1);
					return;
				}
			}

			if (CurrentWork.EqualsHash(SelectedBuilding))
			{
				if (SelectedBuilding.CanWork())
				{
					if (PlayerStatus != PlayerStatus.WORKING)
					{
						Instance.StartWork();
					}
					Parameters[ParamsKind.MONEY].Value += CurrentWork.WorkSalary;
				}
				else
				{
					PlayerStatus = PlayerStatus.NONE;
					GamePaused = true;
					GUImain.CloseAllDialogs();
				}
			}
		}

		if (OnHourChange != null) OnHourChange(1);
	}

	public static int DoSleep(int hours)
	{
		if (!(SelectedBuilding is BuildingHome))
		{
			return -1;
		}
		if (Parameters[ParamsKind.TIRED].Value < 10)
		{
			//return -2;
		}
		if (PlayerStatus != PlayerStatus.NONE)
		{
			return -3;
		}

		GamePaused = true;
		GameBreak = false;
		PlayerStatus = PlayerStatus.SLEEP;

		GUImain.ShowDialog(DialogKind.SLEEPING);
		CurrentHour.Inc(hours, 1);

		return 1;
	}

#region MONEY
	public static bool CheckMoney(int money)
	{
		return money > 0 || Parameters[ParamsKind.MONEY].Value >= Mathf.Abs(money);
	}

	public static bool AddMoney(int money)
	{
		if (!CheckMoney(money))
		{
			GUImain.ShowMessage("UZMANĪBU!","Tev nepietiek naudas, lai veiktu šo darbību.");
			return false;
		}

		Parameters[ParamsKind.MONEY].Value += money;
		return true;
	}
#endregion

#region WORKING
	public void GotoWork()
	{
		PlayerStatus = PlayerStatus.NONE;
		SelectedBuilding = CurrentWork;
		CurrentHour.Inc(1, 0);
		StartWork();
	}

	public void StartWork()
	{
		PlayerStatus = PlayerStatus.WORKING;
		GamePaused = false;
		GameBreak = false;
		GUImain.ShowDialog(DialogKind.WORKING);

		Debug.Log("Start working");
		CurrentHour.Inc(CurrentWork.WorkTime, 1);
	}
#endregion

#region UBAGOŠANA
	public void Bagger()
	{
		GameBreak = false;
		PlayerStatus = PlayerStatus.BAGGER;
		GUImain.ShowDialog(DialogKind.BAGGER);
		GamePaused = false;
	}
	public void StopBagger()
	{
		GameBreak = false;
		GamePaused = true;

		PlayerStatus = PlayerStatus.NONE;
		GUImain.CloseAllDialogs();
	}
#endregion

#region BIZNESS
	public void BuyBusiness()
	{
		Office office = (Office) SelectedBuilding;
		if (office == null)
		{
			return;
		}

		if (BusinessList.Contains(office))
		{
			return;
		}

		//TODO nopirkt biznesu, iztērēt naudu
		office.Buy();

	}

	public void UpgradeBuilding()
	{
		Office office = (Office)SelectedBuilding;
		if (office == null)
		{
			return;
		}

		//TODO uzlabot un iztērēt naudu
		office.Upgrade();
	}
#endregion
}
