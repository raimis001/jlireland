using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class jlButton : MonoBehaviour
{

	public GameObject IconObject;

	private bool _interactable = true;

	void Start()
	{
		Button b = GetComponent<Button>();
		if (!b) return;

		_interactable = b.interactable;
		IconObject.SetActive(_interactable);
	}

	// Update is called once per frame
	void Update()
	{
		Button b = GetComponent<Button>();
		if (!b) return;

		if (b.interactable != _interactable && IconObject)
		{
			_interactable = b.interactable;
			IconObject.SetActive(_interactable);
		}
	}
}
