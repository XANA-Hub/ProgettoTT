using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour {   

    public Animator battleMenuAnimator;
    private Player player;
    private Monster monster;
    private bool battleMenuIsUp = false;

    [Header("Player texts")]
    public TMP_Text playerNameText;
    public TMP_Text playerLevelText;

    [Header("Enemy texts")]
    public TMP_Text enemyNameText;
    public TMP_Text enemyLevelText;
    
    


    private void Start() {
        this.player = MasterManager.instance.player;
        this.monster = MasterManager.instance.monsterDatabase.GetRandomMonster();
        playerNameText.SetText(player.data.name);
        playerLevelText.SetText("Lvl: " + player.getLevel());
        enemyNameText.SetText(monster.data.name);
        enemyLevelText.SetText("Lvl: " + monster.getLevel());

        Debug.Log("LIVELLO GIOCATORE BATTLEMANAGER: " + player.getLevel());
        Debug.Log("LIVELLO MOSTRO BATTLEMANAGER: " + monster.getLevel());
       

        StartBattle();
    }

    //
    // Metodi
    //

    private void ShowBattleScreen() {
        
        if (!battleMenuIsUp) {
            Debug.Log("DOVREBBE ESSERE MOSTRATO IL BATTLE MENU!");
            battleMenuAnimator.SetTrigger("Open");
            battleMenuIsUp = true;
        }
        else {
            battleMenuAnimator.SetTrigger("Close");
            battleMenuIsUp = false;
        }
    
    }

    public void StartBattle() {
        //ShowBattleScreen();
        Debug.Log("La battaglia è iniziata!");
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }


}
