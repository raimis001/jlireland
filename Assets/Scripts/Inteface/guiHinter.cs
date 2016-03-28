using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof (EventTrigger))]
public class guiHinter : MonoBehaviour
{
	public string Caption;

	[TextArea(3, 10)]
	public string Description;

	// Use this for initialization
	void Start()
	{
		EventTrigger trigger = GetComponentInParent<EventTrigger>();

		EventTrigger.Entry entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
		entry.callback.AddListener((eventData) => { OnShowHint(); });
		trigger.triggers.Add(entry);

		entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
		entry.callback.AddListener((eventData) => { OnHideHint(); });
		trigger.triggers.Add(entry);

	}
	// Update is called once per frame
	void Update()
	{

	}

	void OnHideHint()
	{
		GUImain.Instance.HideHint();
	}

	void OnShowHint()
	{
		GUImain.Instance.ShowHint(transform.position, Caption, Description);
	}
}
