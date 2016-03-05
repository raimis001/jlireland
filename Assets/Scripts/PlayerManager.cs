using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
	private static PlayerManager _instance = null;
	public static PlayerManager Instance { get { return _instance; } }


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
	}
}
