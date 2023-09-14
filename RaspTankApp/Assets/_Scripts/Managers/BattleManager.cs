using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour {   

    // Variabili private
    private Player player;
    private Monster monster;
    private bool battleMenuIsUp = false;

    [Header("Starting battle state")]
    public BattleState battleState;

    [Header("Battle menu animator")]
    public Animator battleMenuAnimator;

    [Header("Dialogue")]
    public TMP_Text dialogueText;

    [Header("Player")]
    public Image playerSprite;
    public RectTransform playerHP;
    public TMP_Text playerNameText;
    public TMP_Text playerLevelText;

    [Header("Enemy")]
    public Image enemySprite;
    public RectTransform enemyHP;
    public TMP_Text enemyNameText;
    public TMP_Text enemyLevelText;

    [Header("Battle parameters")]
    [Range(0f, 1f)] public float criticalHitProbability = 0.125f; // 12.5% 
    [Range(1.0f, 2.0f)] public float criticalHitMultiplier = 1.5f; // 50% in più di danno
    [Range(0f, 1f)] public float fleeProbability = 0.5f; // 50%
    [Range(0f, 1f)] public float healProbability = 0.45f; // 45%
    [Range(0, 10)] public int monsterLevelVariation = 3; // I mostri possono apparire con una variazione del livello del giocatore
    


    
    private void Start() {

        this.player = MasterManager.instance.player;
        this.monster = MasterManager.instance.monsterDatabase.GetRandomMonster();

        playerNameText.SetText(player.data.name);
        playerLevelText.SetText("Lvl: " + player.getLevel());
        enemyNameText.SetText(monster.data.name);
        enemyLevelText.SetText("Lvl: " + monster.getLevel());

        enemySprite.sprite = monster.data.sprite; // Cambio lo sprite in base al mostro scelto

        // Inizia la battaglia
        battleState = BattleState.START;

        // Per inizializzare la battaglia
        StartCoroutine(SetUpBattle());
    }

    //
    // Metodi
    //

    IEnumerator SetUpBattle() {

        Debug.Log("BATTAGLIA INIZIATA!");
        dialogueText.SetText("A wild " + monster.data.name + " appeared!");

        SetPlayerHP(player.getCurrentHP());
        SetEnemyHP(monster.getCurrentHP());

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

    // Per debug
    public void OnBattleInfoButton() {

        Debug.Log("++ CURRENT PLAYER STATS ++");
        Debug.Log("Nature: " + player.getCurrentNature());
        Debug.Log("Level: " + player.getLevel());
        Debug.Log("HP: " + player.getCurrentHP());
        Debug.Log("Attack: " + player.getCurrentAttack());
        Debug.Log("Defense: " + player.getCurrentDefense());
        Debug.Log("Speed: " + player.getCurrentSpeed());

        Debug.Log("++ CURRENT MONSTER STATS ++");
        Debug.Log("Nature: " + monster.getCurrentNature());
        Debug.Log("Level: " + monster.getLevel());
        Debug.Log("HP: " + monster.getCurrentHP());
        Debug.Log("Attack: " + monster.getCurrentAttack());
        Debug.Log("Defense: " + monster.getCurrentDefense());
        Debug.Log("Speed: " + monster.getCurrentSpeed());


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
        bool isCrit = isCriticalHit();
        int dmgAmount = CalculateDamage(isCrit);
        bool isEnemyDead = monster.takeDamage(dmgAmount);
        
        if(isCrit) {
            dialogueText.SetText("You deal " + dmgAmount + " damage to " + monster.data.name +". It was a critical hit!");
        }else {
            dialogueText.SetText("You deal " + dmgAmount + " damage to " + monster.data.name);
        }

        // Il nemico è morto?
        if(isEnemyDead) {
            battleState = BattleState.WON;
            SetEnemyHP(0);
            dialogueText.SetText("You defeated " + monster.data.name + "!");

            yield return new WaitForSeconds(2f);
            EndBattle();
        }
        else {
            battleState = BattleState.ENEMY_TURN;
            SetEnemyHP(monster.getCurrentHP());

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

        yield return new WaitForSeconds(2f);

        // Ottengo il danno fatto dal giocatore 
        bool isCrit = isCriticalHit();
        int dmgAmount = CalculateDamage(isCrit);
        bool isPlayerDead = player.takeDamage(dmgAmount);

         if(isCrit) {
            dialogueText.SetText(this.monster.data.name + " attacks! You were dealt " + dmgAmount + " points of damage! It was a critical hit!");
        }else {
            dialogueText.SetText(this.monster.data.name + " attacks! You were dealt " + dmgAmount + " points of damage!");
        }
        
        // Il giocatore è morto?
        if(isPlayerDead) {
            battleState = BattleState.LOST;
            SetPlayerHP(0);
            dialogueText.SetText("You were defeated by " + monster.data.name + "!");

            EndBattle();
        }
        else {
            battleState = BattleState.PLAYER_TURN;
            SetPlayerHP(player.getCurrentHP());

            yield return new WaitForSeconds(2f);
            PlayerTurn();
        }
    }


    IEnumerator PlayerHeal() {

        battleState = BattleState.ENEMY_TURN;

        if(isHealSuccesful()) {

            // Se supero gli HP massimi del giocatore, li metto al max
            if(player.getCurrentHP() + player.data.baseHeal > player.getMaxCurrentHP()) {
                SetPlayerHP(player.getMaxCurrentHP());
                dialogueText.SetText ("You succesfully healed " + player.data.baseHeal + " HP! You are at full HP!");
            }else {
                SetPlayerHP(player.getCurrentHP() + player.data.baseHeal);
                dialogueText.SetText ("You succesfully healed " + player.data.baseHeal + " HP!");
            }
            
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
        
        // Danno casuale
        int dmgAmount = Random.Range(1, 10);

        // Danno aumentato in caso di critico
        if(isCritic) {
            dmgAmount = Mathf.RoundToInt(dmgAmount * criticalHitMultiplier); // Esempio: aumenta del 50%
        }

        return dmgAmount;
        
    }

    public void SetPlayerHP(int currentHP) {

        float ratio = (float)currentHP / (float)player.getMaxCurrentHP();

        // Modifico la barra degli HP
        playerHP.localScale = new Vector3(ratio, 1, 1);
        
    }


    public void SetEnemyHP(int currentHP) {

        float ratio = (float)currentHP / (float)monster.getMaxCurrentHP();

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
