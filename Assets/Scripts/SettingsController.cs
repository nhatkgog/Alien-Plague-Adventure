using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Toggle screen;
    public TMP_Dropdown resolution;
    public TMP_Dropdown graphic;
    public Slider volume;
    public AudioMixer audioMixer;
    Resolution[] resolutionArr;

    void Start()
    {

        //screen
        screen.isOn = Screen.fullScreen;

        //resolution
        resolutionArr = Screen.resolutions;
        resolution.ClearOptions();

        //start with first option
        int currentResIndex = 0;
        var options = new System.Collections.Generic.List<string>();
        for (int i = 0; i < resolutionArr.Length; i++)
        {
            string option = resolutionArr[i].width + " x " + resolutionArr[i].height;
            options.Add(option);

            if (resolutionArr[i].width == Screen.currentResolution.width && resolutionArr[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolution.AddOptions(options);
        resolution.value = currentResIndex;
        resolution.RefreshShownValue();

        //graphic
        graphic.value = QualitySettings.GetQualityLevel();
        graphic.RefreshShownValue();

        //volume
        float currentVolume;
        if (audioMixer.GetFloat("MasterVolume", out currentVolume))
        {
            volume.value = Mathf.InverseLerp(-80f, 0f, currentVolume);
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutionArr[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetGraphics(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetVolume(float volume)
    {
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("MasterVolume", dB);
    }

    public void SetSettings()
    {
        Debug.Log("Setting Saved!");
    }

    public void ResetToDefault()
    {
        screen.isOn = true;
        resolution.value = resolutionArr.Length - 1;
        graphic.value = 2;
        volume.value = 1;
        SetVolume(volume.value);
        Debug.Log("Settings reset to default");
    }
}
