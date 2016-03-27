using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	public void LoadCreator()
	{
		SceneManager.LoadScene (1);
	}

	public void LoadGameplay()
	{
		SceneManager.LoadScene (2);
	}

}
