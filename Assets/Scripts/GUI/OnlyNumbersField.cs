using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnlyNumbersField : MonoBehaviour {


	public void Start()
	{
		this.gameObject.GetComponent<InputField>().onValidateInput += delegate(string input, int charIndex, char addedChar) { return MyValidate( addedChar ); };
	}

	private char MyValidate(char charToValidate)
	{
		if (!char.IsDigit(charToValidate))
		{
			charToValidate = '\0';
		}
		return charToValidate;
	}
}
