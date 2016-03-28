using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class guiHint : MonoBehaviour
{

	public Text CaptionText;
	public Text DescrText;


	public void Show(string caption, string description)
	{
		CaptionText.text = caption;
		DescrText.text = description;


		gameObject.SetActive(true);
	}

	public void Hide()
	{
		//GetComponent<Image>().enabled = false;
		gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start()
	{
		Hide();
	}



}
