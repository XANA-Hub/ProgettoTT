using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadPlayerStatsRobot : MonoBehaviour {

    private Player player;
    private int playerCurrentLevel;
    private int playerCurrentHP;
    private int playerMaxCurrentHP;
    private int playerCurrentExp;
    private int playerExpRequiredForNextLevel;

    public RectTransform playerHP;
    public RectTransform playerExp;
    public TMP_Text playerNameText;
    public TMP_Text playerLevelText;



    // Carico i dati all'inizio
    private void Start() {

        player = MasterManager.instance.player;

        LoadData();
        SetPlayerHUD();
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
