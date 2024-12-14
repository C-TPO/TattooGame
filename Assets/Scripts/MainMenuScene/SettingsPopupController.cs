using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsPopupController : MonoBehaviour
{
    [SerializeField, NotNull] private GameObject container = null;
    [SerializeField, NotNull] private Slider musicSlider = null;
    [SerializeField, NotNull] private Slider sfxSlider = null;
    [SerializeField, NotNull] private TMP_Dropdown languageDropdown = null;
    [SerializeField, NotNull] private TMP_Dropdown qualityDropdown = null;
    [SerializeField, NotNull] private TMP_Dropdown resolutionDropdown = null;
    [SerializeField, NotNull] private Toggle fullscreenToggle = null;
    [SerializeField, NotNull] private AudioMixer musicMixer = null;
    [SerializeField, NotNull] private AudioMixer sfxMixer = null;

    private Resolution[] resolutions = null;

    #region Unity Messages

    private void Start()
    {
        SetupSlider(musicSlider, PlayerPrefsManager.GetMusicVolume);
        SetupSlider(sfxSlider, PlayerPrefsManager.GetSfxVolume);
        SetDropdown(languageDropdown, PlayerPrefsManager.GetLanguageIndex);
        SetDropdown(qualityDropdown, PlayerPrefsManager.GetQualityIndex);
        ResolutionSetup();
        fullscreenToggle.isOn = PlayerPrefsManager.GetIsFullscreen;
    }

    #endregion

    #region Public API

    public void Show()
    {
        container.SetActive(true);
    }

    public void Hide()
    {
        container.SetActive(false);
    }

    public void OnLanguageSelected(int languageIndex)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
        PlayerPrefsManager.SaveLanguageIndex(languageIndex);
    }

    public void OnGraphicsSelected(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefsManager.SaveQualityIndex(qualityIndex);
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("Volume", volume);
        PlayerPrefsManager.SaveMusicVolume(volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxMixer.SetFloat("Volume", volume);
        PlayerPrefsManager.SaveSfxVolume(volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefsManager.SaveFullscreen(isFullscreen);
    }

    public void OnResolutionSelected(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefsManager.SaveResolutionIndex(resolutionIndex);
    }

    #endregion

    #region Implementation

    private void ResolutionSetup()
    {
        resolutions = Screen.resolutions;

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = PlayerPrefsManager.GetResolutionIndex;
        for(int i = 0; i < resolutions.Length; i++)
        {
            resolutionOptions.Add(resolutions[i].width + " x " + resolutions[i].height);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
            
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        SetDropdown(resolutionDropdown, currentResolutionIndex);
    }

    private void SetupSlider(Slider slider, float value)
    {
        slider.value = value;
    }

    private void SetDropdown(TMP_Dropdown dropdown, int index)
    {
        dropdown.value = index;
        dropdown.RefreshShownValue();
    }

    #endregion
}
