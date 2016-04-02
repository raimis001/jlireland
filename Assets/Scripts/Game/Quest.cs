using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[Serializable]
public class QuestItem
{
	public GameObject QuestLine;
	public ParamsKind RewardKind;
	public int RewardValue;
}

public class Quest : MonoBehaviour
{
	private static Quest _instance;

	public static Quest Instance
	{
		get { return _instance; }
	}


	public GameObject QuestTab;
	public Sprite QuestSprite;
	public Sprite ExecuteSprite;
	public QuestItem[] Items;
	public Image Icon;

	private int _currentQuest;

	public int CurrentQuest
	{
		get { return _currentQuest; }
		set
		{
			Items[_currentQuest].QuestLine.SetActive(false);
			_currentQuest = value;
			if (_currentQuest >= Items.Length)
			{
				return;
			}
			Items[_currentQuest].QuestLine.SetActive(true);
		}
	}

	private bool ShowTab
	{
		get
		{
			return QuestTab.activeSelf;
			
		}
		set
		{
			QuestTab.SetActive(value);
		}

	}

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	private void Start()
	{

		for (int i = 0; i < Items.Length; i++)
		{
			Items[i].QuestLine.SetActive(i == 0);
		}
		//Icon.sprite = ExecuteSprite;
		GetComponent<Animator>().SetTrigger("Activate");

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SwitchTab()
	{
		ShowTab = !ShowTab;
		//Icon.sprite = QuestSprite;
		GetComponent<Animator>().SetTrigger("Stop");

	}

	public void CloseTab()
	{
		ShowTab = false;
	}

	public void ExecuteQuest()
	{
		if (CurrentQuest >= Items.Length)
		{
			gameObject.SetActive(false);
			return;
		}
		QuestItem item = Items[CurrentQuest];

		Parameters.get(item.RewardKind).Value += item.RewardValue;
		CurrentQuest++;

		if (CurrentQuest >= Items.Length)
		{
			gameObject.SetActive(false);
		}
		else
		{
			//Icon.sprite = ExecuteSprite;
			GetComponent<Animator>().SetBool("Active",true);
		}

	}

	

	public static void Execute(Building building)
	{
		if (!_instance) return;

		switch (_instance.CurrentQuest)
		{
			case 0:
				if (building is BuildingHome)
				{
					_instance.ExecuteQuest();
				}
			break;
				
		}
	}

	public static void ExecuteParams(ParamsKind paramKind)
	{
		if (!_instance) return;
		ParamsClass param = Parameters.get(paramKind);

		switch (_instance.CurrentQuest)
		{
			case 2:
				if (paramKind == ParamsKind.IQ && param.Value >= 10)
				{
					_instance.ExecuteQuest();
				} 
				break;
			case 3:
				if (paramKind == ParamsKind.WELL && param.Value >= 10)
				{
					_instance.ExecuteQuest();
				}
				break;
		}


	}

	public static void ExecuteWorking(Building building)
	{
		if (!_instance) return;

		switch (_instance.CurrentQuest)
		{
			case 1:
				if (building is BuildingMcdonald)
				{
					_instance.ExecuteQuest();
				}
				break;
		}
	}

}
