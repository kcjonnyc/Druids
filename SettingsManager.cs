using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    #region Constants

    /// <summary>
    /// The quality level index for "custom"
    /// </summary>
    public const int CUSTOM_QUALITY_LEVEL = 6;

    #endregion

    #region Instance Variables

    /// <summary>
    /// Indicates if we need to switch to the "custom" quality level
    /// </summary>
    private bool changeToCustomQualityLevel = false;

    /// <summary>
    /// Array of screen resolutions available to be used
    /// </summary>
    private Resolution[] resolutions;

    /// <summary>
    /// GameSettings object to store current game settings
    /// </summary>
    private GameSettings gameSettings;

    #endregion

    #region Graphics Control Variables

    /// <summary>
    /// Graphics control objects
    /// </summary>
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown defaultGraphicsDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown anisotropicTexturesDropdown;
    public Dropdown antiAliasingDropdown;
    public Toggle softParticlesToggle;
    public Toggle realtimeReflectionToggle;
    public Dropdown shadowsDropdown;
    public Dropdown shadowResolutionDropdown;
    public Dropdown vSyncDropdown;

    #endregion

    #region Controls Control Variables

    /// <summary>
    /// Controls control objects
    /// </summary>
    public Toggle invertedToggle;
    public Slider sensitivitySlider;

    #endregion

    #region Audio Control Variables

    /// <summary>
    /// Audio control objects
    /// </summary>
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundEffectsSlider;
    public Slider voiceSlider;

    /// <summary>
    /// AudioSource objects
    /// </summary>
    public AudioSource musicSource;
    public AudioSource soundEffectsSource;
    public AudioSource voiceSource;

    #endregion

    #region Additional Control Variables

    /// <summary>
    /// Button to save default settings
    /// </summary>
    public Button applyButton;

    #endregion

    /// <summary>
    /// Function when the settings menu is enabled
    /// </summary>
    private void OnEnable()
    {
        gameSettings = new GameSettings();

        //// Set up events - subscribe methods to events
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        defaultGraphicsDropdown.onValueChanged.AddListener(delegate 
        {
            changeToCustomQualityLevel = false;
            OnDefaultGraphicsChange();
            // When we change the default graphics we want to set the game settings
            this.SetGameSettingsFromQualitySettings();
            this.changeToCustomQualityLevel = true;
        });
        textureQualityDropdown.onValueChanged.AddListener(delegate { GoToCustomQualityLevel(); OnTextureQualityChange(); });
        anisotropicTexturesDropdown.onValueChanged.AddListener(delegate { GoToCustomQualityLevel(); OnAnisotropicTexturesChange(); });
        antiAliasingDropdown.onValueChanged.AddListener(delegate { GoToCustomQualityLevel(); OnAntiAliasingChange(); });
        softParticlesToggle.onValueChanged.AddListener(delegate { GoToCustomQualityLevel(); OnSoftParticlesToggle(); });
        realtimeReflectionToggle.onValueChanged.AddListener(delegate { GoToCustomQualityLevel(); OnRealTimeReflectionToggle(); });
        shadowsDropdown.onValueChanged.AddListener(delegate { GoToCustomQualityLevel(); OnShadowsChange(); });
        shadowResolutionDropdown.onValueChanged.AddListener(delegate { GoToCustomQualityLevel(); OnShadowResolutionChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { GoToCustomQualityLevel(); OnVSyncChange(); });
        invertedToggle.onValueChanged.AddListener(delegate { OnInvertedToggle(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitivityChange(); });
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        soundEffectsSlider.onValueChanged.AddListener(delegate { OnSoundEffectsVolumeChange(); });
        voiceSlider.onValueChanged.AddListener(delegate { OnVoiceVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        // Obtain list of screen resolutions and add to dropdown
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
    }

    #region Event Handlers

    /// <summary>
    /// Switch to the "custom" quality level when needed
    /// </summary>
    public void GoToCustomQualityLevel()
    {
        if (QualitySettings.GetQualityLevel() != CUSTOM_QUALITY_LEVEL && this.changeToCustomQualityLevel)
        {
            defaultGraphicsDropdown.value = CUSTOM_QUALITY_LEVEL;
        }
    }

    /// <summary>
    /// Fullscreen toggle event handler
    /// </summary>
    public void OnFullscreenToggle()
    {
        Screen.fullScreen = gameSettings.fullScreen = fullscreenToggle.isOn;
    }

    /// <summary>
    /// Resolution change event handler
    /// </summary>
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    /// <summary>
    /// Default graphics change event handler
    /// </summary>
    public void OnDefaultGraphicsChange()
    {
        QualitySettings.SetQualityLevel(defaultGraphicsDropdown.value);
        gameSettings.defaultGraphics = defaultGraphicsDropdown.value;
    }

    /// <summary>
    /// Texture quality change event handler
    /// </summary>
    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }

    /// <summary>
    /// Anisotropic textures change event handler
    /// </summary>
    public void OnAnisotropicTexturesChange()
    {
        QualitySettings.anisotropicFiltering = gameSettings.anisotropicTextures = (AnisotropicFiltering)anisotropicTexturesDropdown.value;
    }

    /// <summary>
    /// Antialiasing change event handler
    /// </summary>
    public void OnAntiAliasingChange()
    {
        // Antialiasing requires a power of 2
        QualitySettings.antiAliasing = gameSettings.antiAliasing = (int)Mathf.Pow(2f, antiAliasingDropdown.value);
    }

    /// <summary>
    /// Soft particles toggle event handler
    /// </summary>
    public void OnSoftParticlesToggle()
    {
        QualitySettings.softParticles = gameSettings.softParticles = softParticlesToggle.isOn;
    }

    /// <summary>
    /// Realtime reflection toggle event handler
    /// </summary>
    public void OnRealTimeReflectionToggle()
    {
        QualitySettings.realtimeReflectionProbes = gameSettings.realtimeReflection = realtimeReflectionToggle.isOn;
    }

    /// <summary>
    /// Shadows change event handler
    /// </summary>
    public void OnShadowsChange()
    {
        QualitySettings.shadows = gameSettings.shadows = (ShadowQuality)shadowsDropdown.value;
    }

    /// <summary>
    /// Shadow resolution change event handler
    /// </summary>
    public void OnShadowResolutionChange()
    {
        QualitySettings.shadowResolution = gameSettings.shadowResolution = (ShadowResolution)shadowResolutionDropdown.value;
    }

    /// <summary>
    /// vSync change event handler
    /// </summary>
    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    /// <summary>
    /// Inverted toggle event handler
    /// </summary>
    public void OnInvertedToggle()
    {
        // Add to player preferences so they can be set in movement script later on
        PlayerPrefs.SetInt("invertedControls", Convert.ToInt32(invertedToggle.isOn));
        gameSettings.inverted = invertedToggle.isOn;
    }

    /// <summary>
    /// Sensitivity change event handler
    /// </summary>
    public void OnSensitivityChange()
    {
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
        gameSettings.sensitivity = sensitivitySlider.value;
    }

    /// <summary>
    /// Master volume change event handler
    /// </summary>
    public void OnMasterVolumeChange()
    {
        AudioListener.volume = gameSettings.masterVolume = masterVolumeSlider.value;
    }

    /// <summary>
    /// Music volume change event handler
    /// </summary>
    public void OnMusicVolumeChange()
    {
        // Music volume is changed for specified audio source
        gameSettings.musicVolume = musicVolumeSlider.value;
        if (musicSource != null)
        {
            musicSource.volume = musicVolumeSlider.value;
        }
    }

    /// <summary>
    /// Sound effects volume change event handler
    /// </summary>
    public void OnSoundEffectsVolumeChange()
    {
        gameSettings.soundEffectsVolume = soundEffectsSlider.value;
        if (soundEffectsSource != null)
        {
            soundEffectsSource.volume = soundEffectsSlider.value;
        }
    }
    
    /// <summary>
    /// Voice volume change event handler
    /// </summary>
    public void OnVoiceVolumeChange()
    {
        gameSettings.voiceVolume = voiceSlider.value;
        if (voiceSource != null)
        {
            voiceSource.volume = voiceSlider.value;
        }
    }

    /// <summary>
    /// Apply/save button click event handler
    /// </summary>
    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Method to serialize settings to file
    /// </summary>
    public void SaveSettings()
    {
        // Serialize game settings to Json
        string gameSettingsJson = JsonUtility.ToJson(gameSettings, true);
        // Write to app data folder
        File.WriteAllText(Application.persistentDataPath + "/gameSettings.json", gameSettingsJson);
    }

    /// <summary>
    /// Method to load settings or initialize menu options
    /// </summary>
    public void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/gameSettings.json"))
        {
            // File exists
            // Deserialize game settings json into GameSettings object
            gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gameSettings.json"));

            // Set toggles, dropdowns and sliders
            // This will trigger the events and physically set the unity settings

            #region Graphics
            if (gameSettings.defaultGraphics == CUSTOM_QUALITY_LEVEL)
            {
                this.changeToCustomQualityLevel = true;
            }
            defaultGraphicsDropdown.value = gameSettings.defaultGraphics;
            fullscreenToggle.isOn = gameSettings.fullScreen;
            Screen.fullScreen = gameSettings.fullScreen;
            resolutionDropdown.value = gameSettings.resolutionIndex;
            this.SetQualitySettingsFromGameSettings();
            #endregion

            #region Controls
            invertedToggle.isOn = gameSettings.inverted;
            sensitivitySlider.value = gameSettings.sensitivity;
            #endregion

            #region Audio
            masterVolumeSlider.value = gameSettings.masterVolume;
            musicVolumeSlider.value = gameSettings.musicVolume;
            soundEffectsSlider.value = gameSettings.soundEffectsVolume;
            voiceSlider.value = gameSettings.voiceVolume;
            #endregion
        }
        else
        {
            // File does not exist
            #region Graphics
            this.changeToCustomQualityLevel = false;
            defaultGraphicsDropdown.value = QualitySettings.GetQualityLevel();
            fullscreenToggle.isOn = Screen.fullScreen;
            int resolutionIndex = 0;
            foreach (Resolution resolution in resolutions)
            {
                if (Screen.currentResolution.Equals(resolution))
                {
                    break;
                }
                resolutionIndex++;
            }
            resolutionDropdown.value = resolutionIndex;
            this.SetGameSettingsFromQualitySettings();
            #endregion

            #region Controls
            invertedToggle.isOn = false;
            sensitivitySlider.value = 0.5f;
            #endregion

            #region Audio
            masterVolumeSlider.value = 1.0f;
            musicVolumeSlider.value = 1.0f;
            soundEffectsSlider.value = 1.0f;
            voiceSlider.value = 1.0f;
            #endregion

        }

        // Resolution dropdown needs refresh as it does not have a default value
        resolutionDropdown.RefreshShownValue();
        // From this point on, any settings made regarding graphics quality will require a move to custom quality level
        this.changeToCustomQualityLevel = true;
    }

    /// <summary>
    /// Sets the game settings and menu options
    /// </summary>
    public void SetGameSettingsFromQualitySettings()
    {
        textureQualityDropdown.value = gameSettings.textureQuality = QualitySettings.masterTextureLimit;
        anisotropicTexturesDropdown.value = (int)(gameSettings.anisotropicTextures = QualitySettings.anisotropicFiltering);
        antiAliasingDropdown.value = gameSettings.antiAliasing = QualitySettings.antiAliasing;
        softParticlesToggle.isOn = gameSettings.softParticles = QualitySettings.softParticles;
        realtimeReflectionToggle.isOn = gameSettings.realtimeReflection = QualitySettings.realtimeReflectionProbes;
        shadowsDropdown.value = (int)(gameSettings.shadows = QualitySettings.shadows);
        shadowResolutionDropdown.value = (int)(gameSettings.shadowResolution = QualitySettings.shadowResolution);
        vSyncDropdown.value = gameSettings.vSync = QualitySettings.vSyncCount;
    }

    /// <summary>
    /// Sets the quality settings from game settings
    /// </summary>
    public void SetQualitySettingsFromGameSettings()
    {
        textureQualityDropdown.value = gameSettings.textureQuality;
        anisotropicTexturesDropdown.value = (int)gameSettings.anisotropicTextures;
        antiAliasingDropdown.value = gameSettings.antiAliasing;
        softParticlesToggle.isOn = gameSettings.softParticles;
        realtimeReflectionToggle.isOn = gameSettings.realtimeReflection;
        shadowsDropdown.value = (int)gameSettings.shadows;
        shadowResolutionDropdown.value = (int)gameSettings.shadowResolution;
        vSyncDropdown.value = gameSettings.vSync;
    }

    #endregion
}
