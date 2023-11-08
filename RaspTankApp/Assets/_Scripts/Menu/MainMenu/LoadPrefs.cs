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

    [Header("Connection Setting")]
    public TMP_InputField ipAddressInput = null;
    public TMP_InputField portInput = null;


    // Si avvia anche prima di Start, serve per inizializzare
    private void Awake() {


        // Setto i parametri salvati precedentemente
        if(canUse){
        
            //
            // Volume
            //


            if(PlayerPrefs.HasKey("masterVolume")){

                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            } else {
                menuController.ResetButton("Audio");
            }


            //
            // IP
            //


            if(PlayerPrefs.HasKey("masterIP")) {
                string localIP = PlayerPrefs.GetString("masterIP");
                ipAddressInput.text = localIP;
            } else {
                menuController.ResetButton("Connection");
            }


            //
            // Port
            //


            if(PlayerPrefs.HasKey("masterPort")) {
                string localPort = PlayerPrefs.GetString("masterPort");
                portInput.text = localPort;
            } else {
                menuController.ResetButton("Connection");
            }
            

        }

    }


}
