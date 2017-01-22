using UnityEngine;
using System.Collections;

using System.IO;

public class TextureSwitch : MonoBehaviour {

	FileInfo[] files;
	int index;
	ControlSurface surface;


	void Start()
	{
        surface = GetComponent<ControlSurface>();
        var lvlName = gameObject.scene.name;
	    switch (lvlName)
	    {
            case "Level1":
                surface.SetPanelTexture("PanelTextures/Panel_Hex");
                break;
            case "Level2":
                surface.SetPanelTexture("PanelTextures/Panel_Tri");
                break;
            case "Level3":
                surface.SetPanelTexture("PanelTextures/Panel_Quad");
                break;
            case "Level4":
                surface.SetPanelTexture("PanelTextures/Panel_Circle");
                break;
	    }


	 //   index = 0;

		//DirectoryInfo dir = new DirectoryInfo ("Assets/Resources/PanelTextures");
		//files = dir.GetFiles ("*.png");

		
	}
		

	void Update() {
		//if(Input.GetKeyDown(KeyCode.Space)){
		//	index = (index + 1) % files.Length;
		//	Debug.Log ("PanelTextures/" + files[index].Name);
		//	surface.SetPanelTexture ("PanelTextures/" + files[index].Name.Replace(".png", ""));
		//}
	}
}