using UnityEngine;
using System.Collections;

public class ShaderSwitcher : MonoBehaviour {

	public void SwitchOutlineOn()
	{
		foreach (Transform lvChild in this.transform.Find("Root")) {
			if (lvChild.CompareTag ("ObstacleMesh")){
				Renderer lvRend = lvChild.GetComponent<Renderer> ();
				foreach (Material lvMaterial in lvRend.materials) {
					lvMaterial.shader = Shader.Find ("Custom/ImageEffectShader");
				}	
				}
		}
	}

	public void SwitchOutlineOff()
	{
		foreach (Transform lvChild in this.transform.Find("Root")) {
			if (lvChild.CompareTag ("ObstacleMesh")){
				Renderer lvRend = lvChild.GetComponent<Renderer> ();
				foreach (Material lvMaterial in lvRend.materials) {
					lvMaterial.shader = Shader.Find ("Standard");
				}	
			}
		}
	}
}
