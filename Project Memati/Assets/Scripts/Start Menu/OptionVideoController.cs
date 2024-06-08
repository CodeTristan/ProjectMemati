using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown resDropdown;
    public TMP_Dropdown windowDropdown;

    private List<Resolution> resolutions;
    private int currentResolutionIndex = 0;

    private void Start()
    {
        LoadResolutions();
        LoadWindowModes();
        SetInitialSettings();
    }

    private void LoadResolutions()
    {
        
        resolutions = new List<Resolution>()
        {
            //Resolution dropdown'ýna eklenecek resolution seçeneklerinin listesi
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1280, height = 768 },
            new Resolution { width = 1360, height = 768 },
            new Resolution { width = 1366, height = 768 },
            new Resolution { width = 1280, height = 800 },
            new Resolution { width = 1680, height = 1050 },
            new Resolution { width = 1440, height = 1080 },
            new Resolution { width = 1600, height = 900 },
            new Resolution { width = 1280, height = 960 },
            new Resolution { width = 1280, height = 1024 },
            new Resolution { width = 1600, height = 1024 },
            new Resolution { width = 1920, height = 1080 }
        };

        //Resolution Dropdown içerisinde halihazýrda bulunan optionlarýnýn silinmesi
        resDropdown.ClearOptions();

        List<string> optionResolutions = new List<string>();

        //Resolution dropdown içerisine eklenecek resolution deðerlerinin optionResolutions'a eklenmesi
        for (int i = 0; i < resolutions.Count; i++)
        {

            
            string option = resolutions[i].width + " x " + resolutions[i].height;
            optionResolutions.Add(option);

            // 1920x1080 çözünürlüðünün indeksini bulun
            if (resolutions[i].width == 1920 && resolutions[i].height == 1080)
            {
                currentResolutionIndex = i;
            }
        }

        //Resolution dropdown içerisine resolution deðerlerinin eklenmesi
        resDropdown.AddOptions(optionResolutions);
        resDropdown.onValueChanged.AddListener(delegate { SetResolution(resDropdown.value); });
    }

    private void LoadWindowModes()
    {
        //Window Mode dropdown içerisine seçeneklerin eklenmesi
        List<string> windowOptions = new List<string> { "Windowed", "Full Screen", "Borderless Window" };
        windowDropdown.ClearOptions();
        windowDropdown.AddOptions(windowOptions);
        windowDropdown.onValueChanged.AddListener(delegate { SetWindowMode(windowDropdown.value); });
    }

    // Baþlangýç ayarlarýnýn yüklenmesi
    private void SetInitialSettings()
    {
        
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        int savedWindowMode = PlayerPrefs.GetInt("WindowMode", 1);

        resDropdown.value = savedResolutionIndex;
        resDropdown.RefreshShownValue();
        SetResolution(savedResolutionIndex);

        windowDropdown.value = savedWindowMode;
        windowDropdown.RefreshShownValue();
        SetWindowMode(savedWindowMode);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    //Seçilen Window Mode'a göre ekran güncellemesinin yapýlmasý
    public void SetWindowMode(int windowModeIndex)
    {
        switch (windowModeIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
        PlayerPrefs.SetInt("WindowMode", windowModeIndex);
    }
}
