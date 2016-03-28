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

public static class Parameters
{
	private static Dictionary<ParamsKind, ParamsClass> Params = new Dictionary<ParamsKind, ParamsClass>();

	internal static ParamsClass get(ParamsKind index) {
			return Params[index];
	}

	public static void Init()
	{
		Params.Add(ParamsKind.TIRED, new ParamsClass() { Value = 0, MaxValue = 100 });
		Params.Add(ParamsKind.HEALTH, new ParamsClass() { Value = 100, MaxValue = 100 });
		Params.Add(ParamsKind.MONEY, new ParamsClass() { Value = 5000, MaxValue = -1 });
		Params.Add(ParamsKind.IQ, new ParamsClass() { Value = 0, MaxValue = 180 });
		Params.Add(ParamsKind.WELL, new ParamsClass() {Value = 0, MaxValue = 100 });
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
			_gamePaused = value;
			_instance.PauseButton.isOn = !value;
		}
	}

	private static Building _selectedBuilding;
	public static Building SelectedBuilding {
		get {
			return _selectedBuilding;
		}
		set {

			if (Parameters.get(ParamsKind.TIRED).Value >= 90)
			{
				if (_selectedBuilding is BuildingHome)
				{
					GUImain.ShowTiredMessage();
					return;
				}

				if (_selectedBuilding) _selectedBuilding.CLose();

				_selectedBuilding = _instance.HomeBuilding;
				_selectedBuilding.Open();
				GUImain.ShowTiredMessage();
				return;
			}

			if (_selectedBuilding) _selectedBuilding.CLose();
			_selectedBuilding = value;

			if (_selectedBuilding)
			{
				GamePaused = true;
				_selectedBuilding.Open();
			}
			else
			{
				_selectedBuilding = _instance.MapBuilding;
				_selectedBuilding.Open();
			}
		}
	}

	public delegate void HourChange(int hours);
	public static event HourChange OnHourChange;

	public static Building CurrentWork;

	public static PlayerStatus PlayerStatus = PlayerStatus.NONE;

	public static List<Office> BusinessList = new List<Office>();
		
	[Range(0.5f,5f)]
	public float HourTime = 2.5f;
	public Building MapBuilding;
	public Building HomeBuilding;

	[Header("Interface")]
	public Toggle PauseButton;

	void Awake()
	{
		_instance = this;
		Parameters.Init();
	}

	// Use this for initialization
	void Start()
	{
		SelectedBuilding = MapBuilding;
		DayClass.Init(7);
	}

	// Update is called once per frame
	void Update()
	{
		DayClass.Update();
	}

	public void OnPauseGame(bool button)
	{
		_gamePaused = !button;
	}

	public static void DoHourChange()
	{

		Parameters.get(ParamsKind.TIRED).Value += 4;
		Parameters.get(ParamsKind.WELL).Value -= 1;

		foreach (Office office in BusinessList)
		{
			office.CalculateOffice();
		}

		if (Parameters.get(ParamsKind.TIRED).Value >= 90)
		{
			Parameters.get(ParamsKind.HEALTH).Value -= 10;
			if (!(SelectedBuilding is BuildingHome))
			{
				SelectedBuilding.CLose();
				GamePaused = true;

				SelectedBuilding = _instance.HomeBuilding;
			}
			if (PlayerStatus != PlayerStatus.NONE && PlayerStatus != PlayerStatus.SLEEP)
			{
				PlayerStatus = PlayerStatus.NONE;
			}
		} 


		if (SelectedBuilding)
		{
			SelectedBuilding.Calculate();
		}

		if (OnHourChange != null) OnHourChange(1);
	}

	public static void Continue(float hourSpeed)
	{
		Instance.HourTime = hourSpeed;
		GamePaused = false;
	}

#region MONEY
	public static bool CheckMoney(int money)
	{
		return money > 0 || Parameters.get(ParamsKind.MONEY).Value >= Mathf.Abs(money);
	}

	public static bool AddMoney(int money)
	{
		if (!CheckMoney(money))
		{
			GUImain.ShowMessage("UZMANĪBU!","Tev nepietiek naudas, lai veiktu šo darbību.");
			return false;
		}

		Parameters.get(ParamsKind.MONEY).Value += money;
		return true;
	}
#endregion

#region WORKING
	public void GotoWork()
	{
		PlayerStatus = PlayerStatus.NONE;
		SelectedBuilding = CurrentWork;
		DayClass.IncHour();
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
