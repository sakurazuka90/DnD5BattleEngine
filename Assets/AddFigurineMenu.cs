using UnityEngine;
using System.Collections;

public class AddFigurineMenu : MonoBehaviour {

	public GameObject CreateCharacterPanel;

	private Player newPlayer;

	void OnEnable()
	{
		Debug.Log("NEW?!?!");
	}

	public void OpenCreateCharacter()
	{
		CreateCharacterPanel.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
