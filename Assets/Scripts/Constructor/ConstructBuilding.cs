using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ConstructBuilding : MonoBehaviour
{

	public GameObject Basement;
	public GameObject Floor;
	public GameObject Windows;
	public GameObject Pilars;
	public GameObject Roof;
	public GameObject FloorObject;
	public GameObject DoorObject;

	private Material _floorMaterial;
	public Material FloorMaterial;

	private Material _roofMaterial;
	public Material RoofMaterial;

	private Material _baseMaterial;
	public Material BaseMaterial;

	private Material _doorMaterial;
	public Material DoorMaterial;

	[Range(1,10)]
	public int Floors = 0;

	private bool _lightsON = true;
	public bool LightsON = true;

	public Material MatLightOn;
	public Material MatLightOff;

	private int oldFloor = 0;
	private Dictionary<int, GameObject> floorObjects = new Dictionary<int, GameObject>();


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (oldFloor != Floors)
		{
			ClearFloors();
			for (int i = 0; i < Floors; i++)
			{
				AddFloor(i);
			}
			ChangeScale();
			oldFloor = Floors;
		}
		if (_floorMaterial != FloorMaterial)
		{
			_floorMaterial = FloorMaterial;
			RedrawMaterialFloor();
		}
		if (_roofMaterial != RoofMaterial)
		{
			_roofMaterial = RoofMaterial;
			RedrawMaterialRoof();
		}
		if (_baseMaterial != BaseMaterial)
		{
			_baseMaterial = BaseMaterial;
			RedrawMaterialBase();
		}
		if (_doorMaterial != DoorMaterial)
		{
			_doorMaterial = DoorMaterial;
			RedrawMaterialDoor();
		}
		if (_lightsON != LightsON)
		{
			_lightsON = LightsON;
			SwitchLights();
		}

	}

	void SwitchLights()
	{
		for (int i = 0; i < Windows.transform.childCount; i++)
		{
			Windows.transform.GetChild(i).GetComponent<Renderer>().material = LightsON ? MatLightOn : MatLightOff;
		}
	}

	void ClearFloors()
	{
		while (FloorObject.transform.FindChild("addFloor"))
		{
			DestroyImmediate(FloorObject.transform.FindChild("addFloor").gameObject);
		}
		floorObjects.Clear();
	}

	void AddFloor(int floor)
	{
		GameObject obj = Instantiate(Floor);
		obj.name = "addFloor";
		obj.transform.SetParent(FloorObject.transform);
		obj.transform.localPosition = new Vector3(0,0.1f + (floor + 1) * 0.3f,0);
		obj.transform.localScale = Vector3.one;



		if (floorObjects.ContainsKey(floor))
		{
			floorObjects.Remove(floor);
		}

		floorObjects.Add(floor, obj);

		RedrawMaterialFloor();
	}

	void RedrawMaterialDoor()
	{
		for (int i = 0; i < DoorObject.transform.childCount; i++)
		{
			DoorObject.transform.GetChild(i).GetComponent<Renderer>().material = DoorMaterial;
		}
	}

	void RedrawMaterialBase()
	{
		for (int i = 0; i < Basement.transform.childCount; i++)
		{
			Basement.transform.GetChild(i).GetComponent<Renderer>().material = BaseMaterial;
		}
		Transform child = Roof.transform.FindChild("Base");

		for (int i = 0; i < child.transform.childCount; i++)
		{
			child.transform.GetChild(i).GetComponent<Renderer>().material = BaseMaterial;
		}
	}

	void RedrawMaterialRoof()
	{
		Transform child = Roof.transform.FindChild("Roof");

		for (int i = 0; i < child.transform.childCount; i++)
		{
			child.transform.GetChild(i).GetComponent<Renderer>().material = RoofMaterial;
		}

	}

	void RedrawMaterialFloor()
	{
		for (int i = 0; i < FloorObject.transform.childCount; i++)
		{
			Transform child = FloorObject.transform.GetChild(i);
			for (int j = 0; j < child.childCount; j++)
				child.GetChild(j).GetComponent<Renderer>().material = FloorMaterial;
		}


		for (int i = 0; i < Pilars.transform.childCount; i++)
		{
			Pilars.transform.GetChild(i).GetComponent<Renderer>().material = FloorMaterial;
		}
	}

	void ChangeScale()
	{
		Windows.transform.localScale = new Vector3(Windows.transform.localScale.x, 0.4f + Floors * 0.725f, Windows.transform.localScale.z);
		Pilars.transform.localScale = new Vector3(Pilars.transform.localScale.x, 0.4f + Floors * 0.725f, Pilars.transform.localScale.z);
		Roof.transform.localPosition = new Vector3(Roof.transform.localPosition.x, 0.15f + Floors * 0.3f, Roof.transform.localPosition.z);
	}

}
