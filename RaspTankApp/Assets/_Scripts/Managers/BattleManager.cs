using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour {   

    // Variabili private
    private Player player;
    private Monster monster;
    private bool battleMenuIsUp = false;

    [Header("State of battle")]
    public BattleState battleState;

    [Header("Battle menu animator")]
    public Animator battleMenuAnimator;

    [Header("Enemy sprite")]
    public Image enemySprite;

    [Header("HP Bars")]
    public RectTransform playerHP;
    public RectTransform enemyHP;

    [Header("Player texts")]
    public TMP_Text playerNameText;
    public TMP_Text playerLevelText;

    [Header("Enemy texts")]
    public TMP_Text enemyNameText;
    public TMP_Text enemyLevelText;

    [Header("Dialogue")]
    public TMP_Text dialogueText;

    [Header("Battle parameters")]
    public float criticalHitProbability = 0.125f; // 12.5% 
    [Range(1.0f, 2.0f)] public float criticalHitMultiplier = 1.5f; // 50% in più di danno
    [Range(0f, 1f)] public float fleeProbability = 0.3f; // 30%
    [Range(0f, 1f)] public float healProbability = 0.4f; // 40%
    
    private void Start() {

        this.player = MasterManager.instance.player;
        this.monster = MasterManager.instance.monsterDatabase.GetRandomMonster();

        playerNameText.SetText(player.data.name);
        playerLevelText.SetText("Lvl: " + player.getLevel());
        enemyNameText.SetText(monster.data.name);
        enemyLevelText.SetText("Lvl: " + monster.getLevel());

        enemySprite.sprite = monster.data.sprite; // Cambio lo sprite in base al mostro scelto
        

        Debug.Log("HP BASE giocatore: " + player.data.baseHP);
        Debug.Log("HP ATTUALI giocatore: " + player.getActualHP());
        
        Debug.Log("HP BASE mostro: " + monster.data.baseHP);
        Debug.Log("HP ATTUALI mostro: " + monster.getActualHP());

        Debug.Log("LIVELLO giocatore: " + player.getLevel());
        Debug.Log("LIVELLO mostro: " + monster.getLevel());


        // Inizia la battaglia
        battleState = BattleState.START;

        // Per inizializzare la battaglia
        StartCoroutine(SetUpBattle());
    }

    //
    // Metodi
    //

    IEnumerator SetUpBattle() {

        Debug.Log("La battaglia è iniziata!");
        dialogueText.SetText("A wild " + monster.data.name + " appeared!");

        // Aspetto 2 secondi prima di mostrare il menù della battaglia
        yield return new WaitForSeconds(2f);
        battleState = BattleState.PLAYER_TURN;
        PlayerTurn();

    }

    public void PlayerTurn() {
        dialogueText.SetText("Choose an action:");
    }

    public void OnAttackButton() {

        // Se non è il turno del giocatore
        if(battleState != BattleState.PLAYER_TURN) {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton() {

        // Se non è il turno del giocatore
        if(battleState != BattleState.PLAYER_TURN) {
            return;
        }

        StartCoroutine(PlayerHeal());
    }

    public void OnRunButton() {

        // Se non è il turno del giocatore
        if(battleState != BattleState.PLAYER_TURN) {
            return;
        }

        StartCoroutine(PlayerRun());
    }

    // Funzione che permette di individuare se il colpo sarà critico o meno
    private bool isCriticalHit() {

        float randomValue = Random.Range(0f, 1f);
        return randomValue < criticalHitProbability;
    }

    private bool isFleeSuccesful() {
        float randomValue = Random.Range(0f, 1f);
        return randomValue < fleeProbability;
    }

    private bool isHealSuccesful() {
        float randomValue = Random.Range(0f, 1f);
        return randomValue < healProbability;
    }

    IEnumerator PlayerAttack() {

        // Ottengo il danno fatto dal giocatore 
        int dmgAmount = CalculateDamage(isCriticalHit());
        bool isEnemyDead = monster.takeDamage(dmgAmount);
        
        Debug.Log("Il giocatore fa " + dmgAmount + " danni al nemico!");

        // Il nemico è morto?
        if(isEnemyDead) {
            battleState = BattleState.WON;
            SetEnemyHP(0);
            EndBattle();
        }
        else {
            battleState = BattleState.ENEMY_TURN;
            SetEnemyHP(monster.getActualHP());
            dialogueText.SetText("You deal " + dmgAmount + " damage to " + monster.data.name);
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerRun() {

        if(isFleeSuccesful()) {
            dialogueText.SetText ("You got away safely!");
            yield return new WaitForSeconds(2f);
            EndBattle();
        } else{
            battleState = BattleState.ENEMY_TURN;
            dialogueText.SetText ("You can't escape!");
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
        
    }

    IEnumerator EnemyTurn() {

        dialogueText.SetText(monster.data.name + " attacks!");
        yield return new WaitForSeconds(2f);

        // Ottengo il danno fatto dal giocatore 
        int dmgAmount = CalculateDamage(isCriticalHit());
        bool isPlayerDead = player.takeDamage(dmgAmount);

        Debug.Log("Il nemico fa " + dmgAmount + " danni al giocatore!");
        
        // Il giocatore è morto?
        if(isPlayerDead) {
            battleState = BattleState.LOST;
            SetPlayerHP(0);
            EndBattle();
        }
        else {
            battleState = BattleState.PLAYER_TURN;
            SetPlayerHP(player.getActualHP());
            dialogueText.SetText("You were dealt " + dmgAmount + " points of damage!");
            yield return new WaitForSeconds(2f);
            PlayerTurn();
        }
    }


    IEnumerator PlayerHeal() {

        battleState = BattleState.ENEMY_TURN;

        if(isHealSuccesful()) {
            SetPlayerHP(player.getActualHP() + player.data.baseHeal);
            dialogueText.SetText ("You healed " + player.data.baseHeal + " HP!");
        }
        else {
            dialogueText.SetText ("You couldn't heal your wounds!");
        }
        
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());
    }

    private void EndBattle() {

        if(battleState == BattleState.WON) {
            dialogueText.SetText("You won the battle!");
        } else if(battleState == BattleState.LOST) {
            dialogueText.SetText("You lost the battle!");
        }
    }

    private int CalculateDamage(bool isCritic) {
        
        // Danno
        int dmgAmount = Random.Range(1, 20);

        // Danno aumentato in caso di critico
        if(isCritic) {
            Debug.Log("Il danno è critico!");
            dmgAmount = Mathf.RoundToInt(dmgAmount * criticalHitMultiplier); // Esempio: aumenta del 50%
        }

        return dmgAmount;
        
    }

    public void SetPlayerHP(int currentHP) {

        float ratio = (float)currentHP / (float)MasterManager.instance.player.data.baseHP;

        // Modifico la barra degli HP
        playerHP.localScale = new Vector3(ratio, 1, 1);
        
    }


    public void SetEnemyHP(int currentHP) {

        float ratio = (float)currentHP / (float)monster.data.baseHP;

        // Modifico la barra degli HP
        enemyHP.localScale = new Vector3(ratio, 1, 1);
        
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }


}
