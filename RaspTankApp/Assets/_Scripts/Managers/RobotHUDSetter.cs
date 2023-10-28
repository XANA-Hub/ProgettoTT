using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotHUDSetter : MonoBehaviour {

    // Player
    private Player player;
    private int playerCurrentLevel;
    private int playerCurrentHP;
    private int playerMaxCurrentHP;
    private int playerCurrentExp;
    private int playerExpRequiredForNextLevel;


    // Barre vita
    [Header("HP and XP bar")]
    public RectTransform playerHP;
    public RectTransform playerExp;
    public TMP_Text playerNameText;
    public TMP_Text playerLevelText;

    // Bottoni
    [Header("Buttons")]
    public Button moveUpButton;
    public Button moveDownButton;
    public Button moveLeftButton;
    public Button moveRightButton;

    public Button craneUpButton;
    public Button craneDownButton;
    public Button craneGrabButton;
    public Button craneReleaseButton;

    public Button recognitionButton;

    public Button connectButton;
    public Button disconnectButton;

    private Color originalButtonColor;




    // Carico i dati all'inizio
    private void Start() {

        player = MasterManager.instance.player;
        originalButtonColor = moveUpButton.image.color; // Salva il colore originale dei bottoni

        LoadData();
        SetPlayerHUD();

    }

    private void Update() {
        
        //
        // Attivazione / Disattivazione dei bottoni
        //

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            ActivateButtons();    
            connectButton.interactable = false;
            connectButton.image.color = new Color(0.5f, 0.5f, 0.5f);

            disconnectButton.interactable = true;
            disconnectButton.image.color = originalButtonColor;
        }
        else {
            DeactivateButtons();
            connectButton.interactable = true;
            connectButton.image.color = originalButtonColor;

            disconnectButton.interactable = false;
            disconnectButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro
        }
    }

    public void ActivateButtons() {

        // Riattiva l'interattività dei bottoni
        moveUpButton.interactable = true;
        moveUpButton.image.color = originalButtonColor;

        moveDownButton.interactable = true;
        moveDownButton.image.color = originalButtonColor;

        moveLeftButton.interactable = true;
        moveLeftButton.image.color = originalButtonColor;

        moveRightButton.interactable = true;
        moveRightButton.image.color = originalButtonColor;
        
        craneUpButton.interactable = true;
        craneUpButton.image.color = originalButtonColor;

        craneDownButton.interactable = true;
        moveUpButton.image.color = originalButtonColor;

        craneGrabButton.interactable = true;
        craneGrabButton.image.color = originalButtonColor;

        craneReleaseButton.interactable = true;
        craneReleaseButton.image.color = originalButtonColor;
        
        recognitionButton.interactable = true;
        recognitionButton.image.color = originalButtonColor;
    }


    public void DeactivateButtons() {

        // Disattiva l'interrattività dei bottoni
        moveUpButton.interactable = false;
        moveUpButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro

        moveDownButton.interactable = false;
        moveDownButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro

        moveLeftButton.interactable = false;
        moveLeftButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro

        moveRightButton.interactable = false;
        moveRightButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro

        craneUpButton.interactable = false;
        craneUpButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro

        craneDownButton.interactable = false;
        craneDownButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro

        craneGrabButton.interactable = false;
        craneGrabButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro

        craneReleaseButton.interactable = false;
        craneReleaseButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro

        recognitionButton.interactable = false;
        recognitionButton.image.color = new Color(0.5f, 0.5f, 0.5f); // Grigio scuro
    }



    private void SetPlayerHUD() {
        playerNameText.SetText(player.data.name);
        playerLevelText.SetText("Lvl: " + playerCurrentLevel);
        SetPlayerHPBar();
        SetPlayerExpBar();
    }

    private void LoadData() {
        
        // CurrentLevel
        if(PlayerPrefs.HasKey("playerCurrentLevel")) {
            playerCurrentLevel = PlayerPrefs.GetInt("playerCurrentLevel");
        } else {
            playerCurrentLevel = player.getLevel();
        }

        // CurrentHP
        if(PlayerPrefs.HasKey("playerCurrentHP")) {
            playerCurrentHP = PlayerPrefs.GetInt("playerCurrentHP");
        } else {
            playerCurrentHP = player.getCurrentHP();
        }

        // MaxCurrentHP
        if(PlayerPrefs.HasKey("playerMaxCurrentHP")) {
            playerMaxCurrentHP = PlayerPrefs.GetInt("playerMaxCurrentHP");
        } else {
            playerMaxCurrentHP = player.getMaxCurrentHP();
        }

        // CurrentExp
        if(PlayerPrefs.HasKey("playerCurrentExp")) {
            playerCurrentExp = PlayerPrefs.GetInt("playerCurrentExp");
        } else {
            playerCurrentExp = player.getCurrentExp();
        }

        // ExpRequiredForNextLevel
        if(PlayerPrefs.HasKey("playerExpRequiredForNextLevel")) {
            playerExpRequiredForNextLevel = PlayerPrefs.GetInt("playerExpRequiredForNextLevel");
        } else {
            playerExpRequiredForNextLevel = player.getExpRequiredForNextLevel();
        }

    }


    private void SetPlayerHPBar() {

        float ratio = (float)playerCurrentHP / (float)playerMaxCurrentHP;

        // Modifico la barra degli HP
        playerHP.localScale = new Vector3(ratio, 1, 1);
        
    }
    

    private void SetPlayerExpBar() {

        float ratio = (float)playerCurrentExp / (float)playerExpRequiredForNextLevel;

        // Modifico la barra degli HP
        playerExp.localScale = new Vector3(ratio, 1, 1);
        
    }



    
}
