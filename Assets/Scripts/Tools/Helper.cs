using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public enum ParamsKind
{
	HEALTH,
	TIRED,
	MONEY,
	COMFORT,
	IQ
}

public enum WEEKDAYS
{
	PIRMDIENA = 1,
	OTRDIENA = 2,
	TRESDIENA = 3,
	CETURTDIENA = 4,
	PIEKTDIENA = 5,
	SESTDIENA = 6,
	SVETDIENA = 7
}

public enum PlayerStatus
{
	NONE,
	SLEEP,
	WORKING,
	BAGGER,
	STUDIE
}

[Flags]
public enum DialogKind
{
	NONE = -1,
	GOTO_WORK = 0,
	WORKING = 1,
	SLEEPING = 2,
	WORKINFO = 3,
	BAGGER = 6,
	SCHOOL = 4
}

public static class Helper
{
	static List<int> _random = new List<int>();

	public static void Shuffle<T>(this List<T> list)
	{
		list.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
	}

	public static T Pull<T>(this List<T> list)
	{
		if (list.Count < 1)
		{
			return default(T);
		}
		T result = list[0];
		list.RemoveAt(0);
		return result;
	}

	public static int GetHash(this Building building)
	{
		if (!building)
		{
			if (_random.Count < 1)
			{
				for (int i = 0; i < 5000; i++) _random.Add(-i);
				_random.Shuffle();
			}
			return _random.Pull();
		}
		return building.GetHashCode();
	}

	public static bool EqualsHash(this Building building, Building tobuilding)
	{
		return building.GetHash() == tobuilding.GetHash();
	}

	public static T GetRandomEnum<T>()
	{
		Array A = Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(Random.Range(0, A.Length));
		return V;
	}
}


