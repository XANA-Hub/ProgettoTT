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
        SetHPBar(player, player.getMaxCurrentHP(), playerHP);

        // Mostro
        enemyNameText.SetText(monster.data.name);
        enemyLevelText.SetText("Lvl: " + monster.getLevel());
        enemySprite.sprite = monster.data.sprite; // Cambio lo sprite in base al mostro scelto
        SetHPBar(monster, monster.getMaxCurrentHP(), enemyHP);

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

        StartCoroutine(Attack(player, monster));
    }

    public void OnDefendButton() {

        // Se non è il turno del giocatore
        if(battleState != BattleState.PLAYER_TURN) {
            return;
        }

        StartCoroutine(Defend(monster, player));
    }

    public void OnHealButton() {

        // Se non è il turno del giocatore
        if(battleState != BattleState.PLAYER_TURN) {
            return;
        }

        StartCoroutine(Heal(player));
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


    // Permette al giocatore di scappare dalla battaglia
    IEnumerator PlayerRun() {

        if(isFleeSuccesful()) {
            battleState = BattleState.ESCAPED;
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
        ChooseEnemyAction(monster.getCurrentTemper() == Temper.AGGRESSIVE);
        yield return new WaitForSeconds(2f);
    }


    private void ChooseEnemyAction(bool isAggressive) {

        float monsterHPPercentage = (float)monster.getCurrentHP() / monster.getMaxCurrentHP();
        float playerHPPercentage = (float)player.getCurrentHP() / player.getMaxCurrentHP();
        float randomValue = Random.Range(0f, 1f);

        // Se il giocatore ha meno del 50% degli HP
        if (playerHPPercentage < 0.5f) { 
            if (randomValue < 0.7f) {
                StartCoroutine(Attack(monster, player));
            } else if (randomValue < 0.15f) {
                StartCoroutine(Defend(player, monster));
            } else if(randomValue < 0.15f) {
                StartCoroutine(Heal(monster));
            }
        }

        // Se il giocatore ha PIU' del 50% degli HP 
        else { 

            // Se il mostro ha MENO del 50% degli HP
            if (monsterHPPercentage <= 0.5f) {
                if (randomValue < 0.15f) {
                    StartCoroutine(Heal(monster));
                } else if (randomValue < 0.35f) {
                    StartCoroutine(Defend(player, monster));
                } else if (randomValue < 0.15f) {
                    StartCoroutine(Attack(monster, player));
                }
            } 
            
            // Se il mostro ha PIU' del 50% degli HP (situazione normale)
            else {

                // Suddivido i due casi di comportamento normale in base alla temper del mostro
                if(isAggressive) {
                    if (randomValue < 0.7f) {
                        StartCoroutine(Attack(monster, player));
                    } else if (randomValue < 0.15f) {
                        StartCoroutine(Defend(player, monster));
                    } else if (randomValue < 0.15f) {
                        StartCoroutine(Heal(monster));
                    } 
                }else {
                    if (randomValue < 0.5f) {
                        StartCoroutine(Defend(player, monster));
                    } else if (randomValue < 0.3f) {
                        StartCoroutine(Attack(monster,player));
                    } else if (randomValue < 0.2f) {
                        StartCoroutine(Heal(monster));
                    } 
                }

            }
        }
        
    }


    IEnumerator Attack(Fighter attacker, Fighter defender) {

        bool isPlayerAttacker = attacker is Player;
        Debug.Log(attacker.name + " VUOLE ATTACCARE " + defender.name);

        // Ottengo il danno fatto dall'attaccante
        bool isCrit = isCriticalHit();
        int dmgAmount = CalculateDamage(isCrit, attacker.data.baseAttackDamage, attacker.getCurrentAttack(), defender.getCurrentDefense());

        dialogueText.SetText(attacker.data.name + " deals " + dmgAmount + " damage to " + defender.data.name);
        
        // Attendi
        yield return new WaitForSeconds(2f);
        if (isCrit) {
            dialogueText.SetText("It was a critical hit!");
        } else {
            dialogueText.SetText("It wasn't a critical hit!");
        }

        // Il difensore è morto?
        if (defender.takeDamage(dmgAmount)) {
            if (isPlayerAttacker) {
                battleState = BattleState.WON;
                SetHPBar(defender, 0, enemyHP);
                dialogueText.SetText("You defeated " + defender.data.name + "!");
            } else {
                battleState = BattleState.LOST;
                SetHPBar(attacker, 0, playerHP);
                dialogueText.SetText(attacker.data.name + " was defeated by " + defender.data.name + "!");
            }

            yield return new WaitForSeconds(2f);
            EndBattle();
        } else {
            if (isPlayerAttacker) {
                battleState = BattleState.ENEMY_TURN;
                SetHPBar(defender, defender.getCurrentHP(), enemyHP);
            } else {
                battleState = BattleState.PLAYER_TURN;
                SetHPBar(defender, defender.getCurrentHP(), playerHP);
            }

            yield return new WaitForSeconds(2f);

            if (isPlayerAttacker) {
                StartCoroutine(EnemyTurn());
            } else {
                PlayerTurn();
            }
        }
    }


    IEnumerator Defend(Fighter attacker, Fighter defender) {

        bool isPlayerDefender = defender is Player;

        Debug.Log(defender.name + " VUOLE DIFENDERSI DA " + attacker.name);

        // Il difensore prende una posizione difensiva e perde il turno
        if (isPlayerDefender) {
            dialogueText.SetText(defender.data.name + " takes a defensive stance!");
            battleState = BattleState.ENEMY_TURN;
        } else {
            dialogueText.SetText(defender.data.name + " defends itself!");
            battleState = BattleState.PLAYER_TURN;
        }

        // Attendi per simulare la perdita del turno
        yield return new WaitForSeconds(2f);

        // Adesso il prossimo attacco dell'avversario farà meno danno
        int mitigatedDamage = CalculateMitigatedDamage(attacker.data.baseAttackDamage, attacker.getCurrentAttack(), defender.getCurrentDefense());

        // Modifica il testo di dialogo in base al tipo di difensore (giocatore o nemico)
        if (isPlayerDefender) {
            dialogueText.SetText(defender.data.name + " mitigates the next attack!");
        } else {
            dialogueText.SetText(defender.data.name + " is in a defensive stance!");
        }

        // Attendere per un po' per mostrare il messaggio e quindi passare al prossimo turno
        yield return new WaitForSeconds(2f);

        // Se il turno appartiene all'avversario, inizia il suo turno
        if (isPlayerDefender) {
            StartCoroutine(EnemyTurn());
        } else {
            PlayerTurn();
        }
    }


    IEnumerator Heal(Fighter fighter) {

        battleState = fighter is Player ? BattleState.ENEMY_TURN : BattleState.PLAYER_TURN;
        Debug.Log(fighter.name + " VUOLE CURARSI!");

        if (isHealSuccesful()) {
            fighter.Heal(fighter.data.baseHeal);

            // Se supero gli HP massimi del fighter, li metto al massimo
            if (fighter.getCurrentHP() + fighter.data.baseHeal > fighter.getMaxCurrentHP()) {
                if (fighter is Player) {
                    SetHPBar(player, player.getMaxCurrentHP(), playerHP);
                    dialogueText.SetText("You successfully healed " + fighter.data.baseHeal + " HP! You are at full HP!");
                } else {
                    SetHPBar(monster, monster.getMaxCurrentHP(), enemyHP);
                    dialogueText.SetText(fighter.data.name + " successfully healed " + fighter.data.baseHeal + " HP! It's now at full HP!");
                }
            } else {
                if (fighter is Player) {
                    SetHPBar(player, player.getCurrentHP() + player.data.baseHeal, playerHP);
                    dialogueText.SetText("You successfully healed " + fighter.data.baseHeal + " HP!");
                } else {
                    SetHPBar(monster, monster.getCurrentHP() + monster.data.baseHeal, enemyHP);
                    dialogueText.SetText(fighter.data.name + " successfully healed " + fighter.data.baseHeal + " HP!");
                }
            }
        } else {
            dialogueText.SetText(fighter.data.name + " couldn't heal their wounds!");
        }

        yield return new WaitForSeconds(2f);

        if (fighter is Player) {
            StartCoroutine(EnemyTurn());
        } else {
            PlayerTurn();
        }
    }

    private void EndBattle() {

        if(battleState == BattleState.WON) {
            dialogueText.SetText("You won the battle!");
        } else if(battleState == BattleState.LOST) {
            dialogueText.SetText("You lost the battle!");
        } else if(battleState == BattleState.ESCAPED) {
            dialogueText.SetText ("You got away safely!");
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


    private int CalculateMitigatedDamage(int baseDamage, int attackerAttack, int defenderDefense) {

        // Calcola il danno mitigato in base agli attributi di attacco e difesa
        float defenseFactor = (float)defenderDefense / attackerAttack;
        int mitigatedDamage = Mathf.RoundToInt(baseDamage * defenseFactor);

        // Assicurati che il danno mitigato sia almeno 1
        mitigatedDamage = Mathf.Max(1, mitigatedDamage);

        return mitigatedDamage;
    }

    public void SetHPBar(Fighter fighter, int currentHP, RectTransform healthBar) {
        float ratio = (float)currentHP / (float)fighter.getMaxCurrentHP();
        healthBar.localScale = new Vector3(ratio, 1, 1);
    }


    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }


}
