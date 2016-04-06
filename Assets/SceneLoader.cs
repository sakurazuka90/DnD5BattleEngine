using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	public static SceneLoader instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	public void LoadCreator()
	{
		SceneManager.LoadScene (1);
	}

	public void LoadGameplay()
	{
		SceneManager.LoadScene (2);
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene (0);
	}

}
