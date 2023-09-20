using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Tutorial: https://www.youtube.com/watch?v=Cq_Nnw_LwnI&t=812s

//
// TODO: Da convertire i salvataggi del PlayerPrefs col nuovo metodo di salvataggio!
//

public class MenuController : MonoBehaviour {

    [Header("Menu Navigation")]

    [Header("Volume Settings")]
    public TMP_Text volumeTextValue = null;
    public Slider volumeSlider = null;
    public float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    public TMP_Text mouseSensitivityTextValue = null;
    public Slider mouseSensitivitySlider = null;
    public int defaultMouseSensitivity = 4;
    public int mainMouseSensitivity = 4;

    [Header("Toggle Settings")]
    public Toggle invertMouseToggle = null;

    [Header("Graphics Settings")]
    public Slider brightnessSlider = null;
    public TMP_Text brightnessTextValue = null;
    public float defaultBrightness = 1;

    [Space(10)]
    public TMP_Dropdown qualityDropDown;
    public Toggle fullScreenToggle;

    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;

    [Header("Quality Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("Confirmation")]
    public GameObject confirmationPrompt = null;
    

    [Header("Menus to activate/deactivate")]
    public GameObject noSavedGameDialog = null;


    private void Start() {

        resolutions = Screen.resolutions; // Ottengo le risoluzioni disponibili
        resolutionDropdown.ClearOptions(); // Ripulisco eventuali opzioni già presenti

        List<string> resolutionList = new List<string>(); // Tutte le risolizioni

        int currentResolutionIndex = 0;

        // Per ogni risoluzione, la vado ad aggiungere alla lista
        // questa lista serve solo per essere poi mostrata nel dropdown
        for (int i=0; i<resolutions.Length; i++) {

            string currentOption = resolutions[i].width + " x " + resolutions[i].height;
            resolutionList.Add(currentOption);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.width){

                // Salviamo la corrente risoluzione dello schermo
                currentResolutionIndex = i; 
            }
        }
        
        // Aggiungo al dropdown le risoluzioni trovate e setto la risoluzione dello schermo corrente
        resolutionDropdown.AddOptions(resolutionList);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    // Per settare la risoluzione dello schermo in base all'indice passato
    public void SetResolution(int index) {

        Resolution resolution = resolutions[index];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void ExitButton() {
        Application.Quit();
    }

    public void setVolume(float volume) {

        // Cambio il volume
        AudioListener.volume = volume;

        // Visualizzo il volume nuovo
        volumeTextValue.text = volume.ToString("0.0");
    }


    // Permette di applicare il volume scelto dallo slider
    public void VolumeApply() {

        // Salvo in PlayerPrefs il contenuto del volume
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);

        // Avvia il metodo ConfirmationBox
        StartCoroutine(ConfirmationBox());

    }

    public void SetMouseSensitivity(float sensitivity){

        // Converto in intero
        mainMouseSensitivity = Mathf.RoundToInt(sensitivity);

        mouseSensitivityTextValue.text = sensitivity.ToString("0");

    }


    public void GameplayApply() {
        
        //
        // Salvataggio inversione del mouse
        //

        // Se il toggle è premuto
        if(invertMouseToggle.isOn){

            // Lo setto come valore booleano
            PlayerPrefs.SetInt("masterInvertMouse", 1);

        }else{

            PlayerPrefs.SetInt("masterInvertMouse", 0);
        }

        //
        // Salvataggio sensitività del mouse
        //

        PlayerPrefs.SetFloat("masterMouseSensitivity", mainMouseSensitivity);
        
        // Mostro la checkbox verde in basso a sinistra
        StartCoroutine(ConfirmationBox());


    }

    public void SetBrightness(float brightness) {

        // brightnessLevel contiene ciò che poi vogliamo salvare
        brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool fullScreen) {

        // isFullScreen contiene il valore da salvare
        isFullScreen = fullScreen;

    }

    public void SetQuality(int quality) {

        // qualityLevel contiene il valore da salvare
        qualityLevel = quality;

    }

    public void GraphicsApply() {

        //
        // Salvo la luminosità nelle PlayerPrefs
        //

        PlayerPrefs.SetFloat("masterBrightness", brightnessLevel);
        
        
        // TODO: nel caso mettere qui come viene cambiata la luminosità //
        


        //
        // Salvo la qualità nelle PlayerPrefs
        //

        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        //
        // Salvo il FullScreen nelle PlayerPRefs
        //

        PlayerPrefs.SetInt("masterFullscreen", (isFullScreen ? 1 : 0));
        Screen.fullScreen = isFullScreen;

        StartCoroutine(ConfirmationBox());

    }


    public void ResetButton(string menuType) {

        //
        // Se siamo nelle impostazioni audio
        //

        if(menuType == "Audio"){
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text= defaultVolume.ToString("0.0");
            VolumeApply();
        }
        

        //
        // Se siamo nelle impostazioni del gameplay
        //

        if(menuType == "Gameplay") {

            mouseSensitivityTextValue.text= defaultMouseSensitivity.ToString("0");
            mouseSensitivitySlider.value = defaultMouseSensitivity;
            mainMouseSensitivity = defaultMouseSensitivity;

            invertMouseToggle.isOn = false;

            GameplayApply();
        }

        if(menuType == "Graphics") {
            
            
            // TODO: Reset brightness value
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            // Reset a medium
            qualityDropDown.value = 1;
            QualitySettings.SetQualityLevel(1); 

            // Reset a off
            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;
            
            // Rimetto la risoluzione dello schermo attuale
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length; // Ultima risoluzione (quella max supportata dal nostro schermo)

            GraphicsApply();
        }

    }

    public IEnumerator ConfirmationBox() {

        confirmationPrompt.SetActive(true); // Mostro sullo schermo il box di conferma
        yield return new WaitForSeconds(2); // Rimane attivo per 2 secondi
        confirmationPrompt.SetActive(false); // Lo disattivo

    }

}
