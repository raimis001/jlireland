using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{

	public GameObject[] Steps;

	int CurrentStep;

	// Use this for initialization
	void Start()
	{

		for (int i = 0; i < Steps.Length; i++)
		{
			Steps[i].SetActive(i == 0);
		}

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Restart()
	{
		CurrentStep = 0;
		for (int i = 0; i < Steps.Length; i++)
		{
			Steps[i].SetActive(i == 0);
		}
		gameObject.SetActive(true);
	}

	public void NextStep()
	{
		if (CurrentStep >= Steps.Length - 1)
		{
			HideTutorial();
			return;
		}
		Steps[CurrentStep].SetActive(false);
		CurrentStep++;
		Steps[CurrentStep].SetActive(true);
	}

	public void HideTutorial()
	{
		gameObject.SetActive(false);
	}
}
