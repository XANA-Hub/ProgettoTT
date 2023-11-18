using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BattleManager : MonoBehaviour {   

    // Variabili private giocatore / mostro
    private Player player;
    private Monster monster;

    // Danni mitigati dalla difesa del giocatore / mostro
    private int playerMitigatedDamage = 0;
    private int monsterMitigatedDamage = 0;
    private BattleState battleState;

    [Header("Buttons")]
    [SerializeField] private Button attackButton;
    [SerializeField] private  Button defendButton;
    [SerializeField] private  Button healButton;
    [SerializeField] private  Button runButton;
    [SerializeField] private  Button endBattleButton;

    [Header("Dialogue box")]
    [SerializeField] private  TMP_Text dialogueText;

    [Header("UI objects to activate/deactivate")]
    [SerializeField] private GameObject EndBattleDialogue;


    [Header("Player")]
    [SerializeField] private  Image playerSprite;
    [SerializeField] private  RectTransform playerHP;
    [SerializeField] private  RectTransform playerExp;
    [SerializeField] private  TMP_Text playerNameText;
    [SerializeField] private  TMP_Text playerLevelText;

    [Header("Enemy")]
    [SerializeField] private  Image enemySprite;
    [SerializeField] private  RectTransform enemyHP;
    [SerializeField] private  TMP_Text enemyNameText;
    [SerializeField] private  TMP_Text enemyLevelText;

    [Header("Battle parameters")]
    [Range(1.0f, 2.0f)] public float criticalHitMultiplier = 1.5f; // 50% in più di danno
    [Range(0f, 1f)] public float minFleeProbability = 0.2f; // Valore minimo della probabilità di fuga
    [Range(0f, 1f)] public float maxFleeProbability = 0.8f; // Valore massimo della probabilità di fuga
    [Range(0, 10)] public int monsterLevelVariation = 3; // I mostri possono apparire con una variazione del livello del giocatore
    [Range(0, 10)] public int attackDamageVariation = 5; // I mostri possono apparire con una variazione del livello del giocatore
    [Range(0, 10)] public int healingPointsVariation = 5; // Di quanto può variare la cura

    // Forse da mettere uno Start insieme a questo metodo???
    private void OnEnable() {

        // Inizialmente disattivato
        EndBattleDialogue.SetActive(false);
        
        // Inizializzo il giocatore e il mostro
        player = MasterManager.instance.player;
        ChooseMonster();

        // Inizia la battaglia
        battleState = BattleState.START;

        // Scelgo la musica
        ChooseBattleMusic();

        // Per inizializzare la battaglia
        StartCoroutine(SetUpBattle());
    }

    //
    // Scelta inizio battaglia
    //

    private void ChooseMonster() {

        string monsterToBattle = MasterManager.instance.clientTCPManager.GetMonsterToBattle();
        Debug.Log("BattleManager: stringa del mostro è " + monsterToBattle);
        
        if(!string.IsNullOrEmpty(monsterToBattle)) {
            monster = Instantiate(MasterManager.instance.monsterDatabase.GetSpecificMonster(monsterToBattle));
        } else {
            Debug.Log("BattleManager: stringa del mostro vuota! Verrà usato un mostro random!");
            monster = Instantiate(MasterManager.instance.monsterDatabase.GetRandomMonster());
        }

        MasterManager.instance.clientTCPManager.CleanMonsterToBattle();
    }

    private void ChooseBattleMusic() {
        
        if (monster.name.Equals("Primordial Sorcerer") ||
            monster.name.Equals("Primordial Wizard") ||
            monster.name.Equals("Forgotten Evil") ||
            monster.name.Equals("Vampire Lord") ||
            monster.name.Equals("Archaic Minotaur")) {
            MasterManager.instance.audioManager.PlayMusic("BossBattle");
        } else {
            MasterManager.instance.audioManager.PlayMusic("Battle");
        }

    }

    //
    // Bottoni
    //

    private void DeactivateButtons() {
        attackButton.interactable = false;
        defendButton.interactable = false;
        healButton.interactable = false;
        runButton.interactable = false;
    }

    private void ActivateButtons() {
        attackButton.interactable = true;
        defendButton.interactable = true;
        healButton.interactable = true;
        runButton.interactable = true;
    }

    public void OnAttackButton() {
        DeactivateButtons();
        StartCoroutine(PlayerAttack());
    }

    public void OnDefendButton() {
        DeactivateButtons();
        StartCoroutine(PlayerDefend());
    }

    public void OnHealButton() {
        DeactivateButtons();
        StartCoroutine(PlayerHeal());
    }

    public void OnRunButton() {
        DeactivateButtons();
        StartCoroutine(PlayerRun());
    }
    public void OnContinueButton() {

        DeactivateButtons();
        
        // Se il giocatore è morto
        if(battleState == BattleState.LOST) {

            // Carico la nuova scena e basta
            // Tengo così i vecchi dati salvati dal giocatore
            SceneHelper.LoadScene("Robot");
        } 
        else if(battleState == BattleState.WON) { // Se il mostro è morto
            
            // Curo di metà dei suoi HP massimi nel caso in cui vinca la battaglia
            player.Heal(player.GetMaxCurrentHP() / 2);
            SaveData();
            SceneHelper.LoadScene("Robot");

        } 
        else if(battleState == BattleState.MONSTER_ESCAPED) { // Il mostro è scappato

            // Curo solo di 1/3
            player.Heal(player.GetMaxCurrentHP() / 3);
            SaveData();
            SceneHelper.LoadScene("Robot");
        }
        else if(battleState == BattleState.PLAYER_ESCAPED) {

            // Curo solo di 1/4
            player.Heal(player.GetMaxCurrentHP() / 4);
            SaveData();
            SceneHelper.LoadScene("Robot");
        }

        
    }


    //
    // Inizializzazione battaglia
    //
    
    private void SetUpBattleHUD() {

        // Giocatore
        playerNameText.SetText(player.data.name);
        playerLevelText.SetText("Lvl: " + player.GetLevel());
        playerSprite.sprite = player.data.sprite; // Cambio lo sprite del giocatore
        SetPlayerHPBar(player.GetCurrentHP());
        SetPlayerExpBar();

        // Mostro
        enemyNameText.SetText(monster.data.name);
        enemyLevelText.SetText("Lvl: " + monster.GetLevel());
        enemySprite.sprite = monster.data.sprite; // Cambio lo sprite in base al mostro scelto
        SetMonsterHPBar(monster.GetMaxCurrentHP());

    }

    IEnumerator SetUpBattle() {

        // Saved data
        LoadOrInitializeData();

        DeactivateButtons();
        SetUpBattleHUD();

        Debug.Log("BATTAGLIA INIZIATA!");
        dialogueText.SetText("A wild " + monster.data.name + " appeared!");

        // Aspetto 2 secondi prima di mostrare il menù della battaglia
        yield return new WaitForSeconds(2f);
        battleState = BattleState.PLAYER_TURN;
        PlayerTurn();

    }


    //
    // Salvataggio/caricamento dati
    //

    private void LoadOrInitializeData() {
        
        // CurrentLevel
        if(PlayerPrefs.HasKey("playerCurrentLevel")) {
            player.SetLevel(PlayerPrefs.GetInt("playerCurrentLevel"));
        } else {
            PlayerPrefs.SetInt("playerCurrentLevel", player.GetLevel());
        }

        // CurrentHP
        if(PlayerPrefs.HasKey("playerCurrentHP")) {
            player.SetCurrentHP(PlayerPrefs.GetInt("playerCurrentHP"));
        } else {
            PlayerPrefs.SetInt("playerCurrentHP", player.GetCurrentHP());
        }

        // MaxCurrentHP
        if(PlayerPrefs.HasKey("playerMaxCurrentHP")) {
            player.SetMaxCurrentHP(PlayerPrefs.GetInt("playerMaxCurrentHP"));
        } else {
            PlayerPrefs.SetInt("playerMaxCurrentHP", player.GetMaxCurrentHP());
        }

        // CurrentExp
        if(PlayerPrefs.HasKey("playerCurrentExp")) {
            player.SetCurrentExp(PlayerPrefs.GetInt("playerCurrentExp"));
        } else {
            PlayerPrefs.SetInt("playerCurrentExp", player.GetCurrentExp());
        }

        // ExpRequiredForNextLevel
        if(PlayerPrefs.HasKey("playerExpRequiredForNextLevel")) {
            player.SetExpRequiredForNextLevel(PlayerPrefs.GetInt("playerExpRequiredForNextLevel"));
        } else {
            PlayerPrefs.SetInt("playerExpRequiredForNextLevel", player.GetExpRequiredForNextLevel());
        }

        PlayerPrefs.Save();

    }

    private void SaveData() {

        // LEVEL
        PlayerPrefs.SetInt("playerCurrentLevel", player.GetLevel());

        // HP
        PlayerPrefs.SetInt("playerCurrentHP", player.GetCurrentHP());
        PlayerPrefs.SetInt("playerMaxCurrentHP", player.GetMaxCurrentHP());

        // EXP
        PlayerPrefs.SetInt("playerCurrentExp", player.GetCurrentExp());
        PlayerPrefs.SetInt("playerExpRequiredForNextLevel", player.GetExpRequiredForNextLevel());

        PlayerPrefs.Save();
    }


    //
    // Turni
    //

    private void PlayerTurn() {
        ActivateButtons();
        dialogueText.SetText("Choose an action:");
    }

    IEnumerator EnemyTurn() {
        ChooseEnemyAction(monster.getCurrentTemper() == Temper.AGGRESSIVE);
        yield return new WaitForSeconds(2f);
    }

 

    // Per debug
    public void OnBattleInfoButton() {

        Debug.Log("++ CURRENT PLAYER STATS ++");
        Debug.Log("Nature: " + player.GetCurrentNatureBonusAsString());
        Debug.Log("Level: " + player.GetLevel());
        Debug.Log("HP: " + player.GetCurrentHP());
        Debug.Log("Attack: " + player.GetCurrentAttack());
        Debug.Log("Defense: " + player.GetCurrentDefense());
        Debug.Log("Speed: " + player.GetCurrentSpeed());

        Debug.Log("++ CURRENT MONSTER STATS ++");
        Debug.Log("Nature: " + monster.GetCurrentNatureBonusAsString());
        Debug.Log("Temper: " + monster.getCurrentTemperAsString());
        Debug.Log("Level: " + monster.GetLevel());
        Debug.Log("HP: " + monster.GetCurrentHP());
        Debug.Log("Attack: " + monster.GetCurrentAttack());
        Debug.Log("Defense: " + monster.GetCurrentDefense());
        Debug.Log("Speed: " + monster.GetCurrentSpeed());

    }

    
    //
    // Funzioni per verificare se una certa azione è andata a buon fine
    //

    private bool IsMissedAttack(Fighter fighter) {
        
        float randomValue = Random.Range(0f, 1f);
        return randomValue < fighter.data.missProbability;
    }

    private bool IsCriticalHit(Fighter fighter) {
        
        float randomValue = Random.Range(0f, 1f);
        return randomValue < fighter.data.criticalHitProbability;
    }

    private bool IsFleeSuccessful(Fighter fighter) {

        float currentFleeProbability = fighter.data.baseFleeProbability;
        int speedDifference;

        // Calcola la differenza di velocità tra i due combattenti
        if (fighter is Player) {
            speedDifference = player.GetCurrentSpeed() - monster.GetCurrentSpeed();
        } else {
            speedDifference = monster.GetCurrentSpeed() - player.GetCurrentSpeed();
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

    private bool IsHealSuccessful(Fighter fighter) {

        float currentHealProbability = fighter.data.healProbability;

        // La aumento nel caso il fighter sia di natura "HEAL"
        // Altrimenti rimane di base (0.45%)
        if(fighter.GetCurrentNatureBonus() == NatureBonus.HEAL) {
            currentHealProbability += 0.10f;
        }

        Debug.Log("CURRENT HEAL PROBABILITY: " + currentHealProbability);

        float randomValue = Random.Range(0f, 1f);
        return randomValue < currentHealProbability;
    }


    //
    // AI dei nemici
    //

    private void ChooseEnemyAction(bool isAggressive) {

        float monsterHPPercentage = (float)monster.GetCurrentHP() / monster.GetMaxCurrentHP();
        float playerHPPercentage = (float)player.GetCurrentHP() / player.GetMaxCurrentHP();
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


    //
    // Calcolo dei punti di danno ecc..
    //

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
        bool isMissed = IsMissedAttack(player);
        bool isCrit = IsCriticalHit(player);
        
        if(!isMissed) {

            // Animazione robot
            StartCoroutine(RobotBattleAnimations.PlayerAttackRobotAnimation());

            Debug.Log("PLAYER ATTACK SUCCESSFUL!");
            int dmgAmount = CalculateDamage(isCrit, player.data.baseAttackDamage, player.GetCurrentAttack(), monster.GetCurrentDefense());
            Debug.Log("Player DMG: " + dmgAmount);
            Debug.Log("Monster MITIGATED DMG: " + monsterMitigatedDamage);

            dmgAmount -= monsterMitigatedDamage;
            dmgAmount = Mathf.Max(1, dmgAmount); // Così è sempre minimo 1
            monsterMitigatedDamage = 0;

            //MasterManager.instance.battleEffectsManager.ShakeGameObject(MasterManager.instance.battleHUD, 0.8f, 0.4f);
            bool isEnemyDead = monster.takeDamage(dmgAmount);
            
            if(isCrit) {
                // TODO: mettere suono critico
                MasterManager.instance.audioManager.PlaySound("PlayerPunch");
                dialogueText.SetText("You deal " + dmgAmount + " damage to " + monster.data.name +". Critical hit!");
            } else {
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
                SetMonsterHPBar(monster.GetCurrentHP());

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
        bool isMissed = IsMissedAttack(monster);
        bool isCrit = IsCriticalHit(monster);
        
        if(!isMissed) {

            // Animazione robot
            StartCoroutine(RobotBattleAnimations.PlayerDamagedRobotAnimation());

            int dmgAmount = CalculateDamage(isCrit, monster.data.baseAttackDamage, monster.GetCurrentAttack(), player.GetCurrentDefense());
            
            Debug.Log("Monster DMG: " + dmgAmount);
            Debug.Log("Player MITIGATED DMG: " + monsterMitigatedDamage);

            dmgAmount -= playerMitigatedDamage;
            dmgAmount = Mathf.Max(1, dmgAmount); // Così è sempre minimo 1
            playerMitigatedDamage = 0;

            MasterManager.instance.battleEffectsManager.ShowOverlay(BattleEffect.ENEMY_ATTACK);
            bool isPlayerDead = player.takeDamage(dmgAmount);
            

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
                SetPlayerHPBar(player.GetCurrentHP());

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

        // Animazione robot
        StartCoroutine(RobotBattleAnimations.PlayerDefendRobotAnimation());

        battleState = BattleState.ENEMY_TURN;
        MasterManager.instance.battleEffectsManager.ShowOverlay(BattleEffect.PLAYER_DEFEND);
        dialogueText.SetText("You take a defensive stance!");

        // Attendi per simulare la perdita del turno
        yield return new WaitForSeconds(2f);

        // Mostro l'overlay
        MasterManager.instance.battleEffectsManager.ShowOverlay(BattleEffect.PLAYER_DEFEND);

        // Adesso il prossimo attacco dell'avversario farà meno danno
        playerMitigatedDamage = CalculateMitigatedDamage(monster.data.baseAttackDamage, monster.GetCurrentAttack(), player.GetCurrentDefense());
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
        monsterMitigatedDamage = CalculateMitigatedDamage(player.data.baseAttackDamage, player.GetCurrentAttack(), monster.GetCurrentDefense());
        Debug.Log("MITIGATED DMG BY MOSTER: " + monsterMitigatedDamage);

        // Modifica il testo di dialogo in base al tipo di difensore (giocatore o nemico)
        dialogueText.SetText(monster.data.name + " will mitigate the next attack!");

        // Attendere per un po' per mostrare il messaggio e quindi passare al prossimo turno
        yield return new WaitForSeconds(2f);

        PlayerTurn();
        
    }

	
	 IEnumerator PlayerHeal() {

        // Animazione robot
        StartCoroutine(RobotBattleAnimations.PlayerAttackRobotAnimation());

        battleState = BattleState.ENEMY_TURN;
        int healingAmount = CalculateHealingPoints(player.GetCurrentHeal(), healingPointsVariation);

        if(IsHealSuccessful(player)) {
            
            // Mostro l'overlay
            MasterManager.instance.battleEffectsManager.ShowOverlay(BattleEffect.PLAYER_HEAL);

            // Setto i dati interni del giocatore
            player.Heal(healingAmount);

            MasterManager.instance.audioManager.PlaySound("Healing");

            // Se supero gli HP massimi del giocatore, li metto al max
            if(player.GetCurrentHP() + healingAmount > player.GetMaxCurrentHP()) {
                SetPlayerHPBar(player.GetMaxCurrentHP());
                dialogueText.SetText ("You succesfully healed " + healingAmount + " HP! You are full HP!");
            }else {
                SetPlayerHPBar(player.GetCurrentHP() + healingAmount);
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
        int healingAmount = CalculateHealingPoints(monster.GetCurrentHeal(), healingPointsVariation);

        if(IsHealSuccessful(monster)) {
            
            // Setto i dati interni del mostro
            monster.Heal(healingAmount);

            MasterManager.instance.audioManager.PlaySound("Healing");

            // Se supero gli HP massimi del mostro, li metto al max
            if(monster.GetCurrentHP() + healingAmount > monster.GetMaxCurrentHP()) {
                SetMonsterHPBar(monster.GetMaxCurrentHP());
                dialogueText.SetText(monster.data.name + " succesfully healed " + healingAmount + " HP! It's now at full HP!");
            }else {
                SetMonsterHPBar(monster.GetCurrentHP() + healingAmount);
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

        if(IsFleeSuccessful(player)) {

            // Animazione robot
            StartCoroutine(RobotBattleAnimations.PlayerDefendRobotAnimation());
            
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

        if(IsFleeSuccessful(monster)) {
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
        
        // Controlla se il giocatore è di livello 100
        if (player.GetLevel() >= 100) {
            dialogueText.SetText("You are already at max level!");
            // Il giocatore è già di livello 100, non fare nulla
            yield break;
        }
        
        // Se il giocatore non è al livello massimo
        int playerLevelBefore = player.GetLevel();

        // Ne do un quarto se il mostro è scappato o se il giocatore è scappato
        if (isMonsterDefeated)
            player.GainExp(monster.GetPlayerExp());
        else
            player.GainExp(monster.GetPlayerExp() / 4);

        int playerLevelAfter = player.GetLevel();
        SetPlayerExpBar();

        dialogueText.SetText("You gained " + monster.GetPlayerExp() + " XP points!");
        yield return new WaitForSeconds(2f);

        // Il giocatore è aumentato di livello
        if (playerLevelAfter > playerLevelBefore) {
            playerLevelText.SetText("Lvl: " + player.GetLevel());
            dialogueText.SetText("You are now level " + player.GetLevel() + "!");
        }

        yield return new WaitForSeconds(2f);
        
    }

    IEnumerator ShowEndBattleDialogue() {
        
        yield return new WaitForSeconds(6f);
        EndBattleDialogue.SetActive(true);

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
            MasterManager.instance.battleEffectsManager.ShowOverlay(BattleEffect.PLAYER_DEFEAT);
            
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
        
        //Destroy(monster);
        StartCoroutine(ShowEndBattleDialogue());
    }

    //
    // Barre degli HP, XP, ecc...
    //

    private void SetPlayerHPBar(int currentHP) {

        float ratio = (float)currentHP / (float)player.GetMaxCurrentHP();

        // Modifico la barra degli HP
        playerHP.localScale = new Vector3(ratio, 1, 1);
        
    }
    

    private void SetPlayerExpBar() {

        float ratio = (float)player.GetCurrentExp() / (float)player.GetExpRequiredForNextLevel();

        // Modifico la barra degli HP
        playerExp.localScale = new Vector3(ratio, 1, 1);
        
    }

    private void SetMonsterHPBar(int currentHP) {

        float ratio = (float)currentHP / (float)monster.GetMaxCurrentHP();

        // Modifico la barra degli HP
        enemyHP.localScale = new Vector3(ratio, 1, 1);
        
    }

   
    //
    // Enable/Disable
    //

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }


}
