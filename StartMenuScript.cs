using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

    [SerializeField]
    public string startScene;
    public Canvas exitMenu;
    public Canvas settingsMenu;
    public Button startText;
    public Button settingsText;
    public Button exitText;

	// Use this for initialization
	void Start ()
    { 
        exitMenu = exitMenu.GetComponent<Canvas>();
        settingsMenu = settingsMenu.GetComponent<Canvas>();
        // Grab the button component attached to the text objects
        startText = startText.GetComponent<Button>();
        settingsText = settingsText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        exitMenu.enabled = false;
        settingsMenu.enabled = false;
	}

    public void StartGame()
    {
        // Change scene to specified start scene
        SceneManager.LoadScene(startScene);
    }

    public void ExitPress()
    {
        // Enable quit menu and disable other buttons
        exitMenu.enabled = true;
        startText.enabled = false;
        settingsText.enabled = false;
        exitText.enabled = false;
    }

    public void ExitNoPress()
    {
        // Close quit menu and enable other buttons
        exitMenu.enabled = false;
        startText.enabled = true;
        settingsText.enabled = true;
        exitText.enabled = true;
    }

    public void ExitYesPress()
    {
        // Quit the application
        Application.Quit();
    }

    public void SettingsPress()
    {
        settingsMenu.enabled = true;
        startText.enabled = false;
        settingsText.enabled = false;
        exitText.enabled = false;
    }

    public void SettingsExit()
    {
        settingsMenu.enabled = false;
        startText.enabled = true;
        settingsText.enabled = true;
        exitText.enabled = true;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
