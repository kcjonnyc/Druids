using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshairTexture; // the texture of the crosshair - the indicator in the middle of the screen
	// the texture is public so we can specify the Texture to be used in the inspector
	private Rect position; // position of the crosshair
	private static bool display = true; // indicates if the crosshair should be displayed

	void Start() {
		// on start, the position of the crosshair will be determined (rectangle position) - we only need to do this once
		// we will need to take into acount width and height along with the width and height of the actual texture/image
		position = new Rect ((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height);
		Cursor.visible = false; // do not show the cursor when in the program
		// we can also set the cursor to true in the case we want to show it again
	}

	// the OnGUI method handles all GUI drawing
	void OnGUI() {
		if (display == true) {
			// if the crosshair should be displayed, draw the crosshair texture
			GUI.DrawTexture(position, crosshairTexture);
		}
	}

    public static void DisplayCrosshair(bool toDisplay)
    {
        display = toDisplay;
    }
}
