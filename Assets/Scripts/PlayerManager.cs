using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
	private static PlayerManager _instance = null;
	public static PlayerManager Instance { get { return _instance; } }

	private float _tired = 0;

	public float Tired
	{
		get
		{
			return _tired;
		}
		set
		{
			_tired = value;
			if (_tired > 100)
			{
				_tired = 100;
			}
		}

	}

	public float TiredMidifier
	{
		get
		{
			float result = 7f;
			if (GameManager.SelectedBuilding)
			{
				result += GameManager.SelectedBuilding.TiredModifier;
			}
			return result;
		}
	}

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start()
	{

	}

	void OnEnable()
	{
		GameManager.OnHourChange += OnHourChange;
	}

	void OnDisable()
	{
		GameManager.OnHourChange -= OnHourChange;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnHourChange(int hours)
	{
		Tired += TiredMidifier;
	}
}
