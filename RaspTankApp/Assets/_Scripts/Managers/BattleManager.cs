using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour {   

    // Variabili private
    private Player player;
    private Monster monster;
    private int playerMitigatedDamage = 0;
    private int monsterMitigatedDamage = 0;

    [Header("Starting battle state")]
    public BattleState battleState;

    [Header("Battle menu animator")]
    public Animator battleMenuAnimator;

    [Header("Buttons")]
    public Button attackButton;
    public Button defendButton;
    public Button healButton;
    public Button runButton;

    [Header("Dialogue")]
    public TMP_Text dialogueText;

    [Header("Player")]
    public Image playerSprite;
    public RectTransform playerHP;
    public RectTransform playerExp;
    public TMP_Text playerNameText;
    public TMP_Text playerLevelText;

    [Header("Enemy")]
    public Image enemySprite;
    public RectTransform enemyHP;
    public TMP_Text enemyNameText;
    public TMP_Text enemyLevelText;

    [Header("Battle parameters")]
    [Range(1.0f, 2.0f)] public float criticalHitMultiplier = 1.5f; // 50% in più di danno
    [Range(0f, 1f)] public float minFleeProbability = 0.2f; // Valore minimo della probabilità di fuga
    [Range(0f, 1f)] public float maxFleeProbability = 0.8f; // Valore massimo della probabilità di fuga
    [Range(0, 10)] public int monsterLevelVariation = 3; // I mostri possono apparire con una variazione del livello del giocatore
    [Range(0, 10)] public int attackDamageVariation = 5; // I mostri possono apparire con una variazione del livello del giocatore
    [Range(0, 10)] public int healingPointsVariation = 5; // Di quanto può variare la cura

    private void Start() {

        player = MasterManager.instance.player;
        monster = Instantiate(MasterManager.instance.monsterDatabase.GetRandomMonster());

        // Inizia la battaglia (con la musica)
        battleState = BattleState.START;
        MasterManager.instance.audioManager.PlayMusic("Battle");

        // Per inizializzare la battaglia
        StartCoroutine(SetUpBattle());
    }

    //
    // Metodi
    //

    private void deactivateButtons() {
        attackButton.interactable = false;
        defendButton.interactable = false;
        healButton.interactable = false;
        runButton.interactable = false;
    }

    private void activateButtons() {
        attackButton.interactable = true;
        defendButton.interactable = true;
        healButton.interactable = true;
        runButton.interactable = true;
    }

    private void LoadOrInitializeData() {
        
        // CurrentLevel
        if(PlayerPrefs.HasKey("playerCurrentLevel")) {
            player.SetLevel(PlayerPrefs.GetInt("playerCurrentLevel"));
        } else {
            PlayerPrefs.SetInt("playerCurrentLevel", player.getLevel());
        }

        // CurrentHP
        if(PlayerPrefs.HasKey("playerCurrentHP")) {
            player.SetCurrentHP(PlayerPrefs.GetInt("playerCurrentHP"));
        } else {
            PlayerPrefs.SetInt("playerCurrentHP", player.getCurrentHP());
        }

        // MaxCurrentHP
        if(PlayerPrefs.HasKey("playerMaxCurrentHP")) {
            player.SetMaxCurrentHP(PlayerPrefs.GetInt("playerMaxCurrentHP"));
        } else {
            PlayerPrefs.SetInt("playerMaxCurrentHP", player.getMaxCurrentHP());
        }

        // CurrentExp
        if(PlayerPrefs.HasKey("playerCurrentExp")) {
            player.SetCurrentExp(PlayerPrefs.GetInt("playerCurrentExp"));
        } else {
            PlayerPrefs.SetInt("playerCurrentExp", player.getCurrentExp());
        }

        // ExpRequiredForNextLevel
        if(PlayerPrefs.HasKey("playerExpRequiredForNextLevel")) {
            player.SetExpRequiredForNextLevel(PlayerPrefs.GetInt("playerExpRequiredForNextLevel"));
        } else {
            PlayerPrefs.SetInt("playerExpRequiredForNextLevel", player.getExpRequiredForNextLevel());
        }

        PlayerPrefs.Save();

    }

    private void SaveData() {

        // LEVEL
        PlayerPrefs.SetInt("playerCurrentLevel", player.getLevel());

        // HP
        PlayerPrefs.SetInt("playerCurrentHP", player.getCurrentHP());
        PlayerPrefs.SetInt("playerMaxCurrentHP", player.getMaxCurrentHP());

        // EXP
        PlayerPrefs.SetInt("playerCurrentExp", player.getCurrentExp());
        PlayerPrefs.SetInt("playerExpRequiredForNextLevel", player.getExpRequiredForNextLevel());

        PlayerPrefs.Save();
    }
    
    private void SetUpBattleHUD() {

        // Giocatore
        playerNameText.SetText(player.data.name);
        playerLevelText.SetText("Lvl: " + player.getLevel());
        playerSprite.sprite = player.data.sprite; // Cambio lo sprite del giocatore
        SetPlayerHPBar(player.getCurrentHP());
        SetPlayerExpBar();

        // Mostro
        enemyNameText.SetText(monster.data.name);
        enemyLevelText.SetText("Lvl: " + monster.getLevel());
        enemySprite.sprite = monster.data.sprite; // Cambio lo sprite in base al mostro scelto
        SetMonsterHPBar(monster.getMaxCurrentHP());

    }

    IEnumerator SetUpBattle() {

        // Saved data
        LoadOrInitializeData();

        deactivateButtons();
        SetUpBattleHUD();

        Debug.Log("BATTAGLIA INIZIATA!");
        dialogueText.SetText("A wild " + monster.data.name + " appeared!");

        // Aspetto 2 secondi prima di mostrare il menù della battaglia
        yield return new WaitForSeconds(2f);
        battleState = BattleState.PLAYER_TURN;
        PlayerTurn();

    }


    //
    // Turni
    //

    private void PlayerTurn() {
        activateButtons();
        dialogueText.SetText("Choose an action:");
    }

    IEnumerator EnemyTurn() {
        
        ChooseEnemyAction(monster.getCurrentTemper() == Temper.AGGRESSIVE);
        yield return new WaitForSeconds(2f);
    }

    public void OnAttackButton() {
        deactivateButtons();
        StartCoroutine(PlayerAttack());
    }

    public void OnDefendButton() {
        deactivateButtons();
        StartCoroutine(PlayerDefend());
    }

    public void OnHealButton() {
        deactivateButtons();
        StartCoroutine(PlayerHeal());
    }

    public void OnRunButton() {
        deactivateButtons();
        StartCoroutine(PlayerRun());
    }

    // Per debug
    public void OnBattleInfoButton() {

        Debug.Log("++ CURRENT PLAYER STATS ++");
        Debug.Log("Nature: " + player.getCurrentNatureBonusAsString());
        Debug.Log("Level: " + player.getLevel());
        Debug.Log("HP: " + player.getCurrentHP());
        Debug.Log("Attack: " + player.getCurrentAttack());
        Debug.Log("Defense: " + player.getCurrentDefense());
        Debug.Log("Speed: " + player.getCurrentSpeed());

        Debug.Log("++ CURRENT MONSTER STATS ++");
        Debug.Log("Nature: " + monster.getCurrentNatureBonusAsString());
        Debug.Log("Temper: " + monster.getCurrentTemperAsString());
        Debug.Log("Level: " + monster.getLevel());
        Debug.Log("HP: " + monster.getCurrentHP());
        Debug.Log("Attack: " + monster.getCurrentAttack());
        Debug.Log("Defense: " + monster.getCurrentDefense());
        Debug.Log("Speed: " + monster.getCurrentSpeed());

    }

    
    //
    // Funzioni per verificare se una certa azione è andata a buon fine
    //


    private bool isMissedAttack(Fighter fighter) {
        
        float randomValue = Random.Range(0f, 1f);
        return randomValue < fighter.data.missProbability;
    }

    private bool isCriticalHit(Fighter fighter) {
        
        float randomValue = Random.Range(0f, 1f);
        return randomValue < fighter.data.criticalHitProbability;
    }

    private bool isFleeSuccessful(Fighter fighter) {

        float currentFleeProbability = fighter.data.baseFleeProbability;
        int speedDifference = 0;
        
        // Calcola la differenza di velocità tra i due combattenti
        if(fighter is Player) {
            speedDifference = player.getCurrentSpeed() - monster.getCurrentSpeed();
        } else {
            speedDifference = monster.getCurrentSpeed() - player.getCurrentSpeed();
        }

        // Aumenta la probabilità di fuga
        if (speedDifference > 0) {
            currentFleeProbability  += currentFleeProbability * (speedDifference * 0.05f); // Aumenta del 5% per ogni punto di differenza
        }
        // Diminuisci la probabilità
        else if (speedDifference < 0) {
            currentFleeProbability -= currentFleeProbability * (Mathf.Abs(speedDifference) * 0.05f); // Diminuisci del 5% per ogni punto di differenza
        }

        // Assicurati che fleeChance sia compreso tra i valori massimo e minimo
        currentFleeProbability = Mathf.Clamp(currentFleeProbability, minFleeProbability, maxFleeProbability);

        Debug.Log("CURRENT FLEE PROBABILITY: " + currentFleeProbability * 100 + "%");

        float randomValue = Random.Range(0f, 1f);
        return randomValue < currentFleeProbability;
    }



    private bool isHealSuccessful(Fighter fighter) {

        float currentHealProbability = fighter.data.healProbability;

        // La aumento nel caso il fighter sia di natura "HEAL"
        // Altrimenti rimane di base (0.45%)
        if(fighter.getCurrentNatureBonus() == NatureBonus.HEAL) {
            currentHealProbability += 0.10f;
        }

        Debug.Log("CURRENT HEAL PROBABILITY: " + currentHealProbability);

        float randomValue = Random.Range(0f, 1f);
        return randomValue < currentHealProbability;
    }


    private void ChooseEnemyAction(bool isAggressive) {

        float monsterHPPercentage = (float)monster.getCurrentHP() / monster.getMaxCurrentHP();
        float playerHPPercentage = (float)player.getCurrentHP() / player.getMaxCurrentHP();
        float randomValue = Random.Range(0f, 1f);

        // Se il giocatore ha meno del 50% degli HP
        if (playerHPPercentage < 0.5f) { 
            if (randomValue < 0.80f) {
                StartCoroutine(MonsterAttack());
            } else if (randomValue < 0.95f) {
                StartCoroutine(MonsterHeal());
            } else {
                StartCoroutine(MonsterDefend());
            }
        }

        // Se il giocatore ha PIU' del 50% degli HP 
        else { 

            // Se il mostro ha MENO del 50% degli HP
            if (monsterHPPercentage <= 0.5f) {
                
                // Suddivido i due casi di comportamento normale in base alla temper del mostro
                // Aggressivo
                if(isAggressive) {
                    if (randomValue < 0.30f) {
                        StartCoroutine(MonsterHeal());
                    } else if (randomValue < 0.60f) {
                        StartCoroutine(MonsterRun());
                    } else if (randomValue < 0.80f) {
                        StartCoroutine(MonsterAttack());
                    } else {
                        StartCoroutine(MonsterDefend());
                    }
                }

                // Difensivo
                else {

                    if (randomValue < 0.30f) {
                        StartCoroutine(MonsterHeal());
                    } else if (randomValue < 0.60f) {
                        StartCoroutine(MonsterDefend());
                    } else if (randomValue < 0.80f) {
                        StartCoroutine(MonsterAttack());
                    } else {
                        StartCoroutine(MonsterRun());
                    }

                }

            } 
            
            // Se il mostro ha PIU' del 50% degli HP (situazione normale)
            else {

                // Suddivido i due casi di comportamento normale in base alla temper del mostro
                // Aggressivo
                if(isAggressive) {
                    if (randomValue < 0.70f) {
                        StartCoroutine(MonsterAttack());
                    } else if (randomValue < 0.90f) {
                        StartCoroutine(MonsterHeal());
                    } else {
                        StartCoroutine(MonsterDefend());
                    }
                }
                // Difensivo
                else {
                    if (randomValue < 0.50f) {
                        StartCoroutine(MonsterDefend());
                    } else if (randomValue < 0.80f) {
                        StartCoroutine(MonsterAttack());
                    } else {
                        StartCoroutine(MonsterHeal());
                    }

                }

            }
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

    private int CalculateHealingPoints(int baseHealing, int healingVariation) {

        // Calcola il valore base di cura
        int healing = baseHealing;

        // Aggiungi una variazione random alla cura (ad esempio, da -2 a +2)
        int healingVariationAmount = Random.Range(-healingVariation, healingVariation + 1);
        healing += healingVariationAmount;

        // Assicurati che l'HP recuperato sia sempre positivo
        healing = Mathf.Max(0, healing);

        return healing;
    }


    private int CalculateMitigatedDamage(int baseDamage, int attackerAttack, int defenderDefense) {

        // Calcola il danno mitigato in base agli attributi di attacco e difesa
        float defenseFactor = (float)defenderDefense / attackerAttack;
        int mitigatedDamage = Mathf.RoundToInt(baseDamage * defenseFactor);

        // Assicurati che il danno mitigato sia almeno 1
        mitigatedDamage = Mathf.Max(1, mitigatedDamage);

        return mitigatedDamage;
    }

    //
    // Azioni
    //
    	
    
    IEnumerator PlayerAttack() {

        // Ottengo il danno fatto dal giocatore 
        bool isMissed = isMissedAttack(player);
        bool isCrit = isCriticalHit(player);
        
        if(!isMissed) {
            Debug.Log("PLAYER ATTACK SUCCESSFUL!");
            int dmgAmount = CalculateDamage(isCrit, player.data.baseAttackDamage, player.getCurrentAttack(), monster.getCurrentDefense());
            Debug.Log("Player DMG: " + dmgAmount);
            Debug.Log("Monster MITIGATED DMG: " + monsterMitigatedDamage);

            dmgAmount -= monsterMitigatedDamage;
            dmgAmount = Mathf.Max(1, dmgAmount); // Così è sempre minimo 1
            monsterMitigatedDamage = 0;

            bool isEnemyDead = monster.takeDamage(dmgAmount);
            MasterManager.instance.battleEffectsManager.ShakeGameObject(MasterManager.instance.battleHUD, 0.8f, 0.4f);
            
            if(isCrit) {
                // TODO: mettere suono critico
                MasterManager.instance.audioManager.PlaySound("PlayerPunch");
                dialogueText.SetText("You deal " + dmgAmount + " damage to " + monster.data.name +". Critical hit!");
            }else {
                MasterManager.instance.audioManager.PlaySound("PlayerPunch");
                dialogueText.SetText("You deal " + dmgAmount + " damage to " + monster.data.name + "!");
            }

            // Il nemico è morto?
            if(isEnemyDead) {
                battleState = BattleState.WON;
                SetMonsterHPBar(0);
                dialogueText.SetText("You defeated " + monster.data.name + "!");

                MasterManager.instance.audioManager.PlayMusic("VictorySound");
                yield return new WaitForSeconds(2f);
                
                GivePlayerExp(true);
                yield return new WaitForSeconds(2f);

                EndBattle();
            }
            else {
                battleState = BattleState.ENEMY_TURN;
                SetMonsterHPBar(monster.getCurrentHP());

                yield return new WaitForSeconds(2f);
                StartCoroutine(EnemyTurn());
            }
        }
        else {
            MasterManager.instance.audioManager.PlaySound("AttackMiss");
            dialogueText.SetText("You missed the attack!");
            battleState = BattleState.ENEMY_TURN;
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }

    }
	
	
	IEnumerator MonsterAttack() {
        
        // Ottengo il danno fatto dal nemico 
        bool isMissed = isMissedAttack(monster);
        bool isCrit = isCriticalHit(monster);
        
        if(!isMissed) {
            int dmgAmount = CalculateDamage(isCrit, monster.data.baseAttackDamage, monster.getCurrentAttack(), player.getCurrentDefense());
            
            Debug.Log("Monster DMG: " + dmgAmount);
            Debug.Log("Player MITIGATED DMG: " + monsterMitigatedDamage);

            dmgAmount -= playerMitigatedDamage;
            dmgAmount = Mathf.Max(1, dmgAmount); // Così è sempre minimo 1
            playerMitigatedDamage = 0;

            bool isPlayerDead = player.takeDamage(dmgAmount);
            MasterManager.instance.battleEffectsManager.ShakeCamera(Camera.main, 0.5f, 0.1f);

            if(isCrit) {
                MasterManager.instance.audioManager.PlaySound("EnemyPunch");
                dialogueText.SetText(this.monster.data.name + " attacks! You were dealt " + dmgAmount + " damage! Critical hit!");
            }else {
                MasterManager.instance.audioManager.PlaySound("EnemyPunch");
                dialogueText.SetText(this.monster.data.name + " attacks! You were dealt " + dmgAmount + " damage!");
            }
            
            // Il giocatore è morto?
            if(isPlayerDead) {
                battleState = BattleState.LOST;
                SetPlayerHPBar(0);
                dialogueText.SetText("You were defeated by " + monster.data.name + "!");
                
                //MasterManager.instance.audioManager.PlayMusic("DefeatSound");
                yield return new WaitForSeconds(2f);
                EndBattle();
            }
            else {
                battleState = BattleState.PLAYER_TURN;
                SetPlayerHPBar(player.getCurrentHP());

                yield return new WaitForSeconds(2f);
                PlayerTurn();
            }
        }
        else {
            MasterManager.instance.audioManager.PlaySound("AttackMiss");
            dialogueText.SetText(this.monster.data.name + " missed the attack!");
            battleState = BattleState.PLAYER_TURN;
            yield return new WaitForSeconds(2f);
            PlayerTurn();
        }

        
    }


    IEnumerator PlayerDefend() {

        battleState = BattleState.ENEMY_TURN;
        dialogueText.SetText("You take a defensive stance!");

        // Attendi per simulare la perdita del turno
        yield return new WaitForSeconds(2f);

        // Adesso il prossimo attacco dell'avversario farà meno danno
        playerMitigatedDamage = CalculateMitigatedDamage(monster.data.baseAttackDamage, monster.getCurrentAttack(), player.getCurrentDefense());
        Debug.Log("MITIGATED DMG BY PLAYER: " + playerMitigatedDamage);

        // Modifica il testo di dialogo in base al tipo di difensore (giocatore o nemico)
        dialogueText.SetText("You'll mitigate the next attack!");

        // Attendere per un po' per mostrare il messaggio e quindi passare al prossimo turno
        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
        
    }



    IEnumerator MonsterDefend() {

        battleState = BattleState.PLAYER_TURN;
        dialogueText.SetText(monster.data.name + " takes a defensive stance!");

        // Attendi per simulare la perdita del turno
        yield return new WaitForSeconds(2f);

        // Adesso il prossimo attacco dell'avversario farà meno danno
        monsterMitigatedDamage = CalculateMitigatedDamage(player.data.baseAttackDamage, player.getCurrentAttack(), monster.getCurrentDefense());
        Debug.Log("MITIGATED DMG BY MOSTER: " + monsterMitigatedDamage);

        // Modifica il testo di dialogo in base al tipo di difensore (giocatore o nemico)
        dialogueText.SetText(monster.data.name + " will mitigate the next attack!");

        // Attendere per un po' per mostrare il messaggio e quindi passare al prossimo turno
        yield return new WaitForSeconds(2f);

        PlayerTurn();
        
    }

	
	 IEnumerator PlayerHeal() {

        battleState = BattleState.ENEMY_TURN;
        int healingAmount = CalculateHealingPoints(player.getCurrentHeal(), healingPointsVariation);

        if(isHealSuccessful(player)) {
            
            // Setto i dati interni del giocatore
            player.Heal(healingAmount);

            MasterManager.instance.audioManager.PlaySound("Healing");

            // Se supero gli HP massimi del giocatore, li metto al max
            if(player.getCurrentHP() + healingAmount > player.getMaxCurrentHP()) {
                SetPlayerHPBar(player.getMaxCurrentHP());
                dialogueText.SetText ("You succesfully healed " + healingAmount + " HP! You are full HP!");
            }else {
                SetPlayerHPBar(player.getCurrentHP() + healingAmount);
                dialogueText.SetText ("You succesfully healed " + healingAmount + " HP!");
            }
            
        }
        else {
            MasterManager.instance.audioManager.PlaySound("HealingMiss");
            dialogueText.SetText ("You couldn't heal your wounds!");
        }
        
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());
    }

    	
	 IEnumerator MonsterHeal() {

        battleState = BattleState.PLAYER_TURN;
        int healingAmount = CalculateHealingPoints(monster.getCurrentHeal(), healingPointsVariation);
        

        if(isHealSuccessful(monster)) {
            
            // Setto i dati interni del mostro
            monster.Heal(healingAmount);

            MasterManager.instance.audioManager.PlaySound("Healing");

            // Se supero gli HP massimi del mostro, li metto al max
            if(monster.getCurrentHP() + healingAmount > monster.getMaxCurrentHP()) {
                SetMonsterHPBar(monster.getMaxCurrentHP());
                dialogueText.SetText(monster.data.name + " succesfully healed " + healingAmount + " HP! It's now at full HP!");
            }else {
                SetMonsterHPBar(monster.getCurrentHP() + healingAmount);
                dialogueText.SetText(monster.data.name + " succesfully healed " + healingAmount + " HP!");
            }
            
        }
        else {
            MasterManager.instance.audioManager.PlaySound("HealingMiss");
            dialogueText.SetText (monster.data.name + " couldn't heal their wounds!");
        }
        
        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }


    IEnumerator PlayerRun() {

        if(isFleeSuccessful(player)) {
            MasterManager.instance.audioManager.PlaySound("SuccessfulEscape");
            battleState = BattleState.PLAYER_ESCAPED;
            yield return new WaitForSeconds(2f);

            EndBattle();
        } else {
            MasterManager.instance.audioManager.PlaySound("FailedEscape");
            battleState = BattleState.ENEMY_TURN;
            dialogueText.SetText ("You can't escape!");
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
        
    }


    IEnumerator MonsterRun() {

        if(isFleeSuccessful(monster)) {
            MasterManager.instance.audioManager.PlaySound("SuccessfulEscape");
            battleState = BattleState.MONSTER_ESCAPED;
            yield return new WaitForSeconds(2f);
            
            // Exp
            GivePlayerExp(false);
            yield return new WaitForSeconds(2f);

            EndBattle();
        } else {
            MasterManager.instance.audioManager.PlaySound("FailedEscape");
            battleState = BattleState.PLAYER_TURN;
            dialogueText.SetText (monster.data.name + " tried to escape but couldn't!");
            yield return new WaitForSeconds(2f);
            PlayerTurn();
        }
        
    }


    IEnumerator GivePlayerExp(bool isMonsterDefeated) {

        yield return new WaitForSeconds(2f);

        // Ne do un quarto se il mostro è scappato o se il giocatore è scappato
        if(isMonsterDefeated) {
            player.GainExp(monster.GetPlayerExp());
        } else {
            player.GainExp(monster.GetPlayerExp() / 4);
        }
        SetPlayerExpBar();

        dialogueText.SetText("You gained " + monster.GetPlayerExp() + " XP points!");

        // Salvo i dati una volta che ho dato l'exp al giocatore
        SaveData();

        yield return new WaitForSeconds(2f);
    }


    private void EndBattle() {
        
        // Per ora salvo solo se il giocatore non ha perso la partita

        if(battleState == BattleState.WON) {
            MasterManager.instance.audioManager.PlayMusic("VictoryMusic");
            StartCoroutine(GivePlayerExp(true));
            dialogueText.SetText("You won the battle!");

        } else if(battleState == BattleState.LOST) {
            MasterManager.instance.audioManager.PlayMusic("DefeatMusic");
            dialogueText.SetText("You lost the battle!");

        } else if(battleState == BattleState.PLAYER_ESCAPED) {
            MasterManager.instance.audioManager.PlayMusic("Escaped");
            StartCoroutine(GivePlayerExp(false));
            dialogueText.SetText ("You got away safely!");

        } else if(battleState == BattleState.MONSTER_ESCAPED) {
            MasterManager.instance.audioManager.PlayMusic("Escaped");
            StartCoroutine(GivePlayerExp(false));
            dialogueText.SetText (monster.data.name + " got away safely!");

        } else {
            Debug.LogError("BattleManager error: Unknown battle state!");
            dialogueText.SetText ("ERROR: Unknown battle state!");
        }

    }

    public void SetPlayerHPBar(int currentHP) {

        float ratio = (float)currentHP / (float)player.getMaxCurrentHP();

        // Modifico la barra degli HP
        playerHP.localScale = new Vector3(ratio, 1, 1);
        
    }
    

    public void SetPlayerExpBar() {

        float ratio = (float)player.getCurrentExp() / (float)player.getExpRequiredForNextLevel();

        // Modifico la barra degli HP
        playerExp.localScale = new Vector3(ratio, 1, 1);
        
    }

    public void SetMonsterHPBar(int currentHP) {

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
