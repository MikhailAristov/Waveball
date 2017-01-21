using UnityEngine;
using System.Collections;

using System.IO;

public class TextureSwitch : MonoBehaviour {

	FileInfo[] files;
	int index;
	ControlSurface surface;


	void Start() {
		index = 0;

		DirectoryInfo dir = new DirectoryInfo ("Assets/Resources/PanelTextures");
		files = dir.GetFiles ("*.png");

		surface = GetComponent<ControlSurface>(); 
	}
		

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)){
			index = (index + 1) % files.Length;
			Debug.Log ("PanelTextures/" + files[index].Name);
			surface.SetPanelTexture ("PanelTextures/" + files[index].Name.Replace(".png", ""));
		}
	}
}