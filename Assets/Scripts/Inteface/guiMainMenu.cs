using UnityEngine;
using UnityEngine.SceneManagement;

public class guiMainMenu : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void StartGame()
	{
		SceneManager.LoadScene("Intro");
	}
}
