using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guiCourse : MonoBehaviour
{
	public Text CourseText;
	public Text MoneyText;
	public Text IqText;
	public Text HourText;
	public Text TimeText;


	public Button AcceptButton;
	public Text ButtonText;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetValues(Course course)
	{
		if (CourseText) CourseText.text = course.Name;
		if (MoneyText) MoneyText.text = course.Money.ToString();
		if (IqText) IqText.text = course.IQ.ToString();
		if (HourText) HourText.text = (course.Hours - course.PrecessedHours).ToString();
		if (TimeText) TimeText.text = course.Working.WorkString;
	}
	public void SetValues(Treat course)
	{
		if (CourseText) CourseText.text = course.Name;
		if (MoneyText) MoneyText.text = course.Money.ToString();
		if (IqText) IqText.text = course.Heath.ToString();
		if (HourText) HourText.text = (course.Hours - course.PrecessedHours).ToString();
		if (TimeText) TimeText.text = "";// course.Working.WorkString;
	}

}
