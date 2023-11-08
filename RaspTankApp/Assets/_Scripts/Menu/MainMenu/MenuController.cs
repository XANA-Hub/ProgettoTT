using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Tutorial: https://www.youtube.com/watch?v=Cq_Nnw_LwnI&t=812s


public class MenuController : MonoBehaviour {


    [Header("Volume Settings")]
    public TMP_Text volumeTextValue = null;
    public Slider volumeSlider = null;
    public float defaultVolume = 1.0f;

    [Header("Connection settings")]
    public TMP_InputField ipAddressInput = null;
    public TMP_InputField portInput = null;
    public string defaultIPAddress = "192.168.1.15";
    public string defaultPort = "25565";

    [Header("Confirmation")]
    public GameObject confirmationPrompt = null;


    
    private void Start() {

        if(!PlayerPrefs.HasKey("masterIP")) {
            PlayerPrefs.SetString("masterIP", defaultIPAddress);
            ipAddressInput.text = defaultIPAddress;
        }

        if(!PlayerPrefs.HasKey("masterPort")) {
            PlayerPrefs.SetString("masterPort", defaultPort);
            portInput.text = defaultPort;
        }

    }


    public void ExitButton() {
        Application.Quit();
    }


    public void StartGame() {
        SceneHelper.LoadScene("Robot");
    }

    //
    // Audio
    //

    
    public void SetVolume(float volume) {

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


    //
    // Connection
    //

    public void ConnectionApply() {

        // Salvo in PlayerPrefs il contenuto dell campo della connessione

        if(IsValidIP()) {
            Debug.Log("IP inserito valido e salvato correttamente!");
            PlayerPrefs.SetString("masterIP", ipAddressInput.text);
            
        } else {
            Debug.LogError("Errore: IP inserito non valido, verrà usato quello di default!");
            PlayerPrefs.SetString("masterIP", defaultIPAddress);
            ipAddressInput.text = defaultIPAddress;

        }

        if(IsValidPort()) {
            Debug.Log("Porta inserita valida e salvata correttamente!");
            PlayerPrefs.SetString("masterPort", portInput.text);
        } else {
            Debug.LogError("Errore: Porta inserita non valida, ne verrà usata una di default!");
            PlayerPrefs.SetString("masterPort", defaultPort);
            portInput.text = defaultPort;
        }
            
            

        // Avvia il metodo ConfirmationBox
        StartCoroutine(ConfirmationBox());
    }

    private bool IsValidIP() {

        if(string.IsNullOrWhiteSpace(ipAddressInput.text)) {
            return false;
        }

        System.Net.IPAddress ipAddress = null;
        return System.Net.IPAddress.TryParse(ipAddressInput.text, out ipAddress);
    }

    private bool IsValidPort() {

        if(string.IsNullOrWhiteSpace(portInput.text)) {
            return false;
        }

        if (int.TryParse(portInput.text, out int portNumber)) {

            if (portNumber >= 1 && portNumber <= 65535) {
                return true;
            }
        }

        return false;

    }

    public void ResetButton(string menuType) {

        //
        // Se siamo nelle impostazioni audio
        //

        if(menuType == "Audio"){
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if(menuType == "Connection") {
            ipAddressInput.text = defaultIPAddress;
            portInput.text = defaultPort;
            ConnectionApply();
        }

    }


    private IEnumerator ConfirmationBox() {

        confirmationPrompt.SetActive(true); // Mostro sullo schermo il box di conferma
        yield return new WaitForSeconds(2); // Rimane attivo per 2 secondi
        confirmationPrompt.SetActive(false); // Lo disattivo

    }

}
