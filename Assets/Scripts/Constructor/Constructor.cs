using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class MaterialClass
{
	private Material _material;
	public Material Material;

	public bool Changed()
	{
		bool result = _material != Material;
		_material = Material;
		return result;
	}
}

[ExecuteInEditMode]
public class Constructor : MonoBehaviour
{

	public Transform FloorHolder;
	public GameObject FloorPrefab;

	public GameObject RoofObject;
	public GameObject BaseObject;

	[Range(1,10)]
	public int MaxFloor = 1;

	[Header("Materials")]
	public MaterialClass MaterialWalls;
	public MaterialClass MaterialBase;
	public MaterialClass MaterialCeil;
	public MaterialClass MaterialDoor;
	public MaterialClass MaterialWindows;
	public MaterialClass MaterialElements;
	public MaterialClass MaterialBasement;

	[Header("Lights")]
	public MaterialClass MaterialLightsOn;
	public MaterialClass MaterialLightsOff;
	public bool LightsON;

	private bool _lightON;

	private int _maxFloor = 1;
	private Dictionary<string, MaterialClass> MaterialList = new Dictionary<string, MaterialClass>();

	// Use this for initialization
	void Start()
	{
		MaterialList.Add("Base",MaterialBase);
		MaterialList.Add("Basement", MaterialBasement);
		MaterialList.Add("Walls", MaterialWalls);
		MaterialList.Add("Ceils", MaterialCeil);
		MaterialList.Add("Elements", MaterialElements);
	}

	// Update is called once per frame
	void Update()
	{
		if (MaterialList.Count < 1)
		{
			Start();
		}

		if (_maxFloor != MaxFloor)
		{
			CreateFloors();
			_maxFloor = MaxFloor;
		}

		foreach (KeyValuePair<string, MaterialClass> value in MaterialList )
		{
			if (value.Value.Changed())
			{
				SetMaterial(value.Key, value.Value);
			}
		}

		if (MaterialWindows.Changed())
		{
			SetPropertyMaterial("Windows", "Frame", BaseObject.transform, MaterialWindows);
			ChangeFloorProperty("Windows", "Frame", MaterialWindows);

		}
		if (MaterialDoor.Changed())
		{
			SetPropertySimple("Door", "Frame", BaseObject.transform, MaterialDoor);
		}
		if (_lightON != LightsON)
		{
			_lightON = LightsON;
			SwitchLights();
		}

	}

	void ClearFloors()
	{
		while (FloorHolder.childCount > 0)
		{
			DestroyImmediate(FloorHolder.GetChild(0).gameObject);
		}
	}

	private void CreateFloors()
	{
		ClearFloors();
		if (!FloorPrefab)
		{
			return;
		}
		for (int i = 0; i < MaxFloor - 1; i++)
		{
			GameObject floor = Instantiate(FloorPrefab);
			floor.name = "floor";
			floor.transform.SetParent(FloorHolder);
			floor.transform.localScale = Vector3.one;
			floor.transform.localPosition = new Vector3(0,i * 4,0);
		}

		if (RoofObject)
		{
			RoofObject.transform.localPosition = new Vector3(0,MaxFloor * 4 + 1,0);
		}
		foreach (KeyValuePair<string, MaterialClass> value in MaterialList)
		{
			SetMaterial(value.Key, value.Value);
		}
		ChangeFloorProperty("Windows", "Frame", MaterialWindows);
		ChangeFloorProperty("Windows", "Glass", LightsON ? MaterialLightsOn : MaterialLightsOff);

	}

	private void SwitchLights()
	{
		MaterialClass material = LightsON ? MaterialLightsOn : MaterialLightsOff;
		SetPropertySimple("Door", "Glass", BaseObject.transform, material);
		SetPropertyMaterial("Windows", "Glass", BaseObject.transform, material);
		ChangeFloorProperty("Windows", "Glass", material);
	}

	private void SetMaterial(string childName, MaterialClass material)
	{
		Transform child = BaseObject.transform.FindChild(childName);
		if (child)
		{
			for (int i = 0; i < child.childCount; i++)
			{
				Renderer r = child.transform.GetChild(i).GetComponent<Renderer>();
				if (r) r.material = material.Material;
			}
		}
		child = RoofObject.transform.FindChild(childName);
		if (child)
		{
			for (int i = 0; i < child.childCount; i++)
			{
				Renderer r = child.transform.GetChild(i).GetComponent<Renderer>();
				if (r) r.material = material.Material;
			}
		}

		for (int i = 0; i < FloorHolder.transform.childCount; i++)
		{
			child = FloorHolder.transform.GetChild(i).FindChild(childName);
			if (child)
			{
				for (int j = 0; j < child.childCount; j++)
				{
					Renderer r = child.transform.GetChild(j).GetComponent<Renderer>();
					if (r) r.material = material.Material;
				}
			}
		}
	}

	private void SetPropertySimple(string propName, string propDtail, Transform property, MaterialClass material)
	{
		Transform child = property.FindChild(propName);
		if (!child)
		{
			return;
		}
		child = child.FindChild(propDtail);
		if (!child)
		{
			return;
		}
		for (int i = 0; i < child.childCount; i++)
		{
			Renderer r = child.GetChild(i).GetComponent<Renderer>();
			if (r) r.material = material.Material;
		}
	}

	private void SetPropertyMaterial(string propName, string propDtail, Transform property, MaterialClass material)
	{
		Transform child = property.FindChild(propName);
		if (!child)
		{
			return;
		}
		for (int i = 0; i < child.childCount; i++)
		{
			Transform window = child.GetChild(i).FindChild(propDtail);
			if (window)
			{
				for (int j = 0; j < window.childCount; j++)
				{
					Renderer r = window.GetChild(j).GetComponent<Renderer>();
					if (r) r.material = material.Material;
				}
			}
		}
	}

	private void ChangeFloorProperty(string propName, string propDtail, MaterialClass material)
	{
		for (int i = 0; i < FloorHolder.transform.childCount; i++)
		{
			Transform child = FloorHolder.transform.GetChild(i);
			if (child)
			{
				SetPropertyMaterial(propName, propDtail, child, material);
			}
		}

	}

}
