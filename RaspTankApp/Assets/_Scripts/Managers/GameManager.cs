using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //
    // Animazioni
    //
    public Animator battleMenuAnimator;
    
    //public bool gameIsPaused; // Stato pubblico perché può servire in altri script!
    private bool battleMenuIsUp = false;


    //
    // Metodi
    //



    public void ShowBattleScreen() {

        if (battleMenuIsUp == false) {
            Debug.Log("DOVREBBE ESSERE MOSTRATO IL BATTLE MENU!");
            battleMenuAnimator.SetTrigger("Open");
            battleMenuIsUp = true;
            // Time.timeScale = 0f;
        }
        else {
            battleMenuAnimator.SetTrigger("Close");
            battleMenuIsUp = false;
            // Time.timeScale = 1f;
        }
    
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

}
