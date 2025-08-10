using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class LogicAudio : MonoBehaviour
{
    public Slider volumeSlider, musicSlider, sfxSlider, voiceSlider;

    [SerializeField] private AudioMixer masterMixer;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const string VoiceVolumeKey = "VoiceVolume";

    void Start()
    {
        InitializeOptions();

        volumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    private float NormalizeToDecibels(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
    }

    private void SetMasterVolume(float volume)
    {
        float db = NormalizeToDecibels(volume);
        masterMixer.SetFloat("MasterVolume", db);
        PlayerPrefs.SetFloat(MasterVolumeKey, volume);
    }

    private void SetMusicVolume(float volume)
    {
        float db = NormalizeToDecibels(volume);
        masterMixer.SetFloat("MusicVolume", db);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    private void SetSFXVolume(float volume)
    {
        float db = NormalizeToDecibels(volume);
        masterMixer.SetFloat("SFXVolume", db);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    private void SetVoiceVolume(float volume)
    {
        float db = NormalizeToDecibels(volume);
        masterMixer.SetFloat("VoiceVolume", db);
        PlayerPrefs.SetFloat(VoiceVolumeKey, volume);
    }

    private void InitializeOptions()
    {
        // Valores predeterminados
        float defaultMasterVol = 1.0f;
        float defaultMusicVol = 0.0f;
        float defaultVoiceVol = 0.5f;
        float defaultSfxVol = 0.5f;

        float masterVol = PlayerPrefs.HasKey(MasterVolumeKey) ? PlayerPrefs.GetFloat(MasterVolumeKey) : defaultMasterVol;
        masterMixer.SetFloat("MasterVolume", NormalizeToDecibels(masterVol));
        volumeSlider.value = masterVol;
    }

}
