using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreelancerWork
{
	static List<string> WorkNames = new List<string>()
	{
		"Web lapa PHP",
		"Flash reklāma",
		"Web freimworks NodeJS",
		"Reklāmas projekts"
	};

	public static FreelancerWork Create()
	{
		int money = Random.Range(10, 20);

		FreelancerWork work = new FreelancerWork()
		{
			Name = WorkNames.Random(),
			Hours = Random.Range(100,300),
			Money = money,
			IQ = 30 + (money - 10) * 2.5f
		};

		return work;
	}

	public string Name;
	public int Money;
	public float Hours;
	public float IQ;
}

public class OfficeFreelancer : Office
{

	private FreelancerWork Work;



}
