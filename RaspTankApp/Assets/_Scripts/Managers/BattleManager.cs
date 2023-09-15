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
    [Range(0, 10)] public int attackDamageVariation = 5; // I mostri possono apparire con una variazione del livello del giocatore
    


    
    private void Start() {

        player = MasterManager.instance.player;
        monster = Instantiate(MasterManager.instance.monsterDatabase.GetRandomMonster());

        // Inizia la battaglia
        battleState = BattleState.START;

        // Per inizializzare la battaglia
        StartCoroutine(SetUpBattle());
    }

    //
    // Metodi
    //

    private void SetUpBattleHUD() {

        // Giocatore
        playerNameText.SetText(player.data.name);
        playerLevelText.SetText("Lvl: " + player.getLevel());
        playerSprite.sprite = player.data.sprite; // Cambio lo sprite del giocatore
        SetPlayerHPBar(player.getCurrentHP());

        // Mostro
        enemyNameText.SetText(monster.data.name);
        enemyLevelText.SetText("Lvl: " + monster.getLevel());
        enemySprite.sprite = monster.data.sprite; // Cambio lo sprite in base al mostro scelto
        SetEnemyHPBar(monster.getCurrentHP());

    }

    IEnumerator SetUpBattle() {

        SetUpBattleHUD();

        Debug.Log("BATTAGLIA INIZIATA!");
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
        int dmgAmount = CalculateDamage(isCrit, player.data.baseAttackDamage, player.getCurrentAttack(), monster.getCurrentDefense());
        Debug.Log("Player DMG: " + dmgAmount);
        bool isEnemyDead = monster.takeDamage(dmgAmount);
        
        if(isCrit) {
            dialogueText.SetText("You deal " + dmgAmount + " damage to " + monster.data.name +". It was a critical hit!");
        }else {
            dialogueText.SetText("You deal " + dmgAmount + " damage to " + monster.data.name);
        }

        // Il nemico è morto?
        if(isEnemyDead) {
            battleState = BattleState.WON;
            SetEnemyHPBar(0);
            dialogueText.SetText("You defeated " + monster.data.name + "!");

            yield return new WaitForSeconds(2f);
            EndBattle();
        }
        else {
            battleState = BattleState.ENEMY_TURN;
            SetEnemyHPBar(monster.getCurrentHP());

            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }

    }

    // Permette al giocatore di scappare dalla battaglia
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

        //
        // TODO: Enemy AI, per ora attacca e basta
        //

        StartCoroutine(EnemyAttack());


        yield return new WaitForSeconds(2f);
    }

    IEnumerator EnemyAttack() {
        
        // Ottengo il danno fatto dal nemico 
        bool isCrit = isCriticalHit();
        int dmgAmount = CalculateDamage(isCrit, monster.data.baseAttackDamage, monster.getCurrentAttack(), player.getCurrentDefense());
        Debug.Log("Monster DMG: " + dmgAmount);
        bool isPlayerDead = player.takeDamage(dmgAmount);

         if(isCrit) {
            dialogueText.SetText(this.monster.data.name + " attacks! You were dealt " + dmgAmount + " points of damage! It was a critical hit!");
        }else {
            dialogueText.SetText(this.monster.data.name + " attacks! You were dealt " + dmgAmount + " points of damage!");
        }
        
        // Il giocatore è morto?
        if(isPlayerDead) {
            battleState = BattleState.LOST;
            SetPlayerHPBar(0);
            dialogueText.SetText("You were defeated by " + monster.data.name + "!");

            EndBattle();
        }
        else {
            battleState = BattleState.PLAYER_TURN;
            SetPlayerHPBar(player.getCurrentHP());

            yield return new WaitForSeconds(2f);
            PlayerTurn();
        }
    }


    IEnumerator PlayerHeal() {

        battleState = BattleState.ENEMY_TURN;
        

        if(isHealSuccesful()) {
            
            // Setto i dati interni del giocatore
            player.Heal(player.data.baseHeal);

            // Se supero gli HP massimi del giocatore, li metto al max
            if(player.getCurrentHP() + player.data.baseHeal > player.getMaxCurrentHP()) {
                SetPlayerHPBar(player.getMaxCurrentHP());
                dialogueText.SetText ("You succesfully healed " + player.data.baseHeal + " HP! You are full HP!");
            }else {
                SetPlayerHPBar(player.getCurrentHP() + player.data.baseHeal);
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

    private int CalculateDamage(bool isCritic, int baseDamage, int attackerAttack, int defenderDefense) {
        
        // Calcolo del danno in base agli attributi
        int damage = baseDamage;

        // Calcolo del danno in base all'attacco e alla difesa
        float attackFactor = (float)attackerAttack / defenderDefense;
        damage = Mathf.RoundToInt(damage * attackFactor);

        // Calcolo del danno in base alla velocità
        // float speedFactor = (float)attackerSpeed / defenderSpeed;
        // damage = Mathf.RoundToInt(damage * speedFactor);

        // Aggiungi una variazione random al danno (ad esempio, da -2 a +2)
        int damageVariation = Random.Range(-attackDamageVariation, attackDamageVariation+1);
        damage += damageVariation;

        // Assicurati che il danno minimo sia almeno 1
        damage = Mathf.Max(1, damage);

        // Se è un colpo critico, aumenta il danno
        if(isCritic) {
            damage = Mathf.RoundToInt(damage * criticalHitMultiplier); // Esempio: aumenta del 50%
        }

        return damage;
    }

    public void SetPlayerHPBar(int currentHP) {

        float ratio = (float)currentHP / (float)player.getMaxCurrentHP();

        // Modifico la barra degli HP
        playerHP.localScale = new Vector3(ratio, 1, 1);
        
    }


    public void SetEnemyHPBar(int currentHP) {

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
