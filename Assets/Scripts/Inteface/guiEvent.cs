using UnityEngine;
using System.Collections;

public abstract class guiEvent : MonoBehaviour
{
	void OnEnable()
	{
		GameManager.OnHourChange += UpdateHour;
	}
	void OnDisable()
	{
		GameManager.OnHourChange -= UpdateHour;
	}

	protected abstract void UpdateHour(int hurs);

}
