using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogParams
{
	public string Caption;
	public string Description;
	public bool AutoClose;
}

public class guiDialog : MonoBehaviour
{

	public Text CaptionText;
	public Text NoteText;
	public Text ButtonText;

	float unitilClose = 0;

	void Awake()
	{
	}
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (unitilClose > 0)
		{
			unitilClose -= Time.deltaTime;
			if (unitilClose <= 0)
			{
				gameObject.SetActive(false);
				unitilClose = 0;
			}
		}
	}

	public void Open(DialogParams param)
	{
		if (param != null)
		{
			Open(param.Caption, param.Description, param.AutoClose);
		}
		else
		{
			gameObject.SetActive(true);
		}
	}

	public void Open(string caption, string note, bool autoclose)
	{
		if (CaptionText) CaptionText.text = caption;
		if (NoteText) NoteText.text = note;

		unitilClose = autoclose ? 4 : 0;
		
		gameObject.SetActive(true);
	}

	public void Close()
	{
		unitilClose = 0;
		gameObject.SetActive(false);
	}

}
