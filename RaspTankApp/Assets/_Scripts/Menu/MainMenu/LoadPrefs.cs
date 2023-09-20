using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour {

    [Header("General Setting")]
    
    // Per indicare se vogliamo caricare o meno le Prefs
    public bool canUse = false;
    public MenuController menuController;

    [Header("Volume Setting")]
    public TMP_Text volumeTextValue = null;
    public Slider volumeSlider = null;

    [Header("Brightness Setting")]
    public Slider brightnessSlider = null;
    public TMP_Text brightnessTextValue = null;

    [Header("Quality Level Setting")]
    public TMP_Dropdown qualityDropDown;

    [Header("Fullscreen Setting")]
    public Toggle fullScreenToggle;

    [Header("Sensitivity Setting")]
    public TMP_Text mouseSensitivityTextValue = null;
    public Slider mouseSensitivitySlider = null;

    [Header("Invert Mouse Setting")]
    public Toggle invertMouseToggle = null;

    // Si avvia anche prima di Start, serve per inizializzare
    private void Awake() {

        if(canUse){
            
            //
            // Setto i parametri salvati precedentemente
            //

            //
            // Volume
            //
            if(PlayerPrefs.HasKey("masterVolume")){

                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else{
                menuController.ResetButton("Audio");
            }

            //
            // Qualità
            //
            if(PlayerPrefs.HasKey("masterQuality")){
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropDown.value = localQuality;

                QualitySettings.SetQualityLevel(localQuality);
            }
            else{
                menuController.ResetButton("Graphics");
            }

            //
            // Fullscreen
            //
            if(PlayerPrefs.HasKey("masterFullscreen")){
                
                int localFullScreen = PlayerPrefs.GetInt("masterFullScreen");

                if(localFullScreen == 1){
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }else {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }

            }

            //
            // Luminosità
            //
            if(PlayerPrefs.HasKey("masterBrightness")){
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;

                // TODO: Cambia la luminsoità qui

            }

            //
            // Sensitività
            //
            if(PlayerPrefs.HasKey("masterSensitivity")){
                float localSensitivity = PlayerPrefs.GetFloat("masterSensitivity");

                mouseSensitivityTextValue.text = localSensitivity.ToString("0");
                mouseSensitivitySlider.value = localSensitivity;

                menuController.mainMouseSensitivity = Mathf.RoundToInt(localSensitivity);

            }

            //
            // Inversione Mouse
            //
            if(PlayerPrefs.HasKey("masterInvertMouse")){

                if(PlayerPrefs.GetInt("masterInvertMouse") == 1){
                    invertMouseToggle.isOn = true;
                }else{
                    invertMouseToggle.isOn = false;
                }
            }


        }

    }


}
