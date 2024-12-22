using UnityEngine;

public static class PlayerPrefsManager
{
    private const string KEY_MUSICVOLUME = "MusicVolume";
    private const string KEY_SFXVOLUME = "SfxVolume";
    private const string KEY_LANGUAGEINDEX = "LanguageIndex";
    private const string KEY_QUALITYINDEX = "QualityIndex";
    private const string KEY_RESOLUTIONINDEX = "ResolutionIndex";
    private const string KEY_ISFULLSCREEN = "IsFullscreen";

    public static float GetMusicVolume => PlayerPrefs.GetFloat(KEY_MUSICVOLUME, 0f);
    public static float GetSfxVolume => PlayerPrefs.GetFloat(KEY_SFXVOLUME, 0f);
    public static int GetLanguageIndex =>  PlayerPrefs.GetInt(KEY_LANGUAGEINDEX, 0);
    public static int GetQualityIndex => PlayerPrefs.GetInt(KEY_QUALITYINDEX, QualitySettings.count-1);
    public static int GetResolutionIndex => PlayerPrefs.GetInt(KEY_RESOLUTIONINDEX, 0);
    public static bool GetIsFullscreen => PlayerPrefs.GetInt(KEY_ISFULLSCREEN, 1) == 1;

    public static void SaveMusicVolume (float volume) => PlayerPrefs.SetFloat(KEY_MUSICVOLUME, volume);

    public static void SaveSfxVolume (float volume) => PlayerPrefs.SetFloat(KEY_SFXVOLUME, volume);

    public static void SaveLanguageIndex (int index) =>  PlayerPrefs.SetInt(KEY_LANGUAGEINDEX, index);

    public static void SaveQualityIndex (int index) => PlayerPrefs.SetInt(KEY_QUALITYINDEX, index);

    public static void SaveResolutionIndex (int index) => PlayerPrefs.SetInt(KEY_RESOLUTIONINDEX, index);

    public static void SaveFullscreen (bool fullscreen) => PlayerPrefs.SetInt(KEY_ISFULLSCREEN, fullscreen ? 1 : 0);
}
