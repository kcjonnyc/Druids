using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshairTexture; // the texture of the crosshair - the indicator in the middle of the screen
	Rect position; // position of the crosshair
	static bool display = true; // if the crosshair should be displayed

	void Start() {
		// on start, the position of the crosshair will be determined (rectangle position)
		// we will need to take into acount width and height along with the width and height of the actual texture/image
		position = new Rect ((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height);
		Cursor.visible = false; // do not show cursor in this program
	}
		
	void OnGUI() {
		if (display == true) {
			GUI.DrawTexture(position, crosshairTexture);
		}
	}
}
