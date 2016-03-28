using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

	public Slider Progres;

	// Use this for initialization
	void Start()
	{
		Progres.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void LoadMainScene()
	{
		Progres.gameObject.SetActive(true);
		StartCoroutine(LoadNewScene());
	}

	IEnumerator LoadNewScene()
	{

		// This line waits for 3 seconds before executing the next line in the coroutine.
		// This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
		yield return new WaitForSeconds(3);

		// Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
		//AsyncOperation async = Application.LoadLevelAsync();


		AsyncOperation async = SceneManager.LoadSceneAsync("city");

		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone)
		{
			Progres.value = async.progress;
			yield return null;
		}

	}
}
