using UnityEngine;
using System.Collections;

public class AddFigurineMenu : MonoBehaviour {

	public GameObject CreateCharacterPanel;


	public void OpenCreateCharacter()
	{
		CreateCharacterPanel.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
