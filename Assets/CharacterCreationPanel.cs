using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterCreationPanel : AbstractPanelController {

	private Player newPlayer;
	int step = 0;

	public GameObject characterImage;
	private int currentImage = 0;

	private Sprite[] characterImages;
	private GameObject[] figurinePrefabs;


	void OnEnable()
	{
		newPlayer = new Player ();
		step = 0;
		UpdateCharacterImage ();
	}

	// Use this for initialization
	void Start () {
		characterImages = Resources.LoadAll<Sprite> ("CharacterImages");
		figurinePrefabs = Resources.LoadAll<GameObject> ("Figurines/Models");
		GenerateContent ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<string> ValidateStep0()
	{
		return null;
	}

	private void UpdateCharacterImage()
	{
		Debug.Log (characterImages [currentImage].name); 
		characterImage.GetComponent<Image> ().sprite = characterImages [currentImage];
	}

	public void NextImage()
	{
		if (currentImage < characterImages.Length - 1) {
			currentImage++;
		} else {
			currentImage = 0;
		}

		UpdateCharacterImage ();
	}

	public void PreviousImage()
	{
		if (currentImage > 0) {
			currentImage--;
		} else {
			currentImage = characterImages.Length - 1;
		}

		UpdateCharacterImage ();
	}


	#region implemented abstract members of AbstractPanelController
	protected override List<string> GetOptions ()
	{
		List<string> names = new List<string> ();

		foreach (GameObject model in figurinePrefabs) {
			names.Add (model.name);
		}

		return names;
	}
	public override void Load ()
	{
		
	}
	#endregion
}
