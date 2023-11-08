using UnityEngine;

public class Monster : Fighter {

    // Indica come si comporterà il mostro
    // AGRRESSIVE: preferisce attaccare
    // DEFENSIVE: preferisce difendere

    [Header("Exp points given to the player")]
    [Range(1, 100)] public int baseExpGivenToPlayer = 50;

    private Temper currentTemper;

    private void Awake() {

        if(PlayerPrefs.HasKey("playerCurrentLevel")) {
            level = CalculateLevel(PlayerPrefs.GetInt("playerCurrentLevel"), MasterManager.instance.battleManager.monsterLevelVariation);
        } else {
            level = CalculateLevel(MasterManager.instance.player.GetLevel(), MasterManager.instance.battleManager.monsterLevelVariation);
            Debug.LogWarning("Monster: verrà usato un livello di default perché non è stato salvato il livello del giocatore!");
        }

        CalculateStats();

        currentTemper = ChooseTemper();
    }

    private Temper ChooseTemper() {

        if(currentNatureBonus == NatureBonus.ATTACK || currentNatureBonus == NatureBonus.SPEED || currentNatureBonus == NatureBonus.HEAL) {
            currentTemper = Temper.AGGRESSIVE;
        } else if(currentNatureBonus == NatureBonus.DEFENSE || currentNatureBonus == NatureBonus.HP) {
            currentTemper = Temper.DEFENSIVE;
        }

        return currentTemper;
    }

    private int CalculateLevel(int playerLevel, int levelVariation) {

        int minMonsterLevel = Mathf.Max(1, playerLevel - levelVariation);
        int maxMonsterLevel = Mathf.Min(100, playerLevel + levelVariation); // Imposta il limite massimo a 100

        int monsterLevel = Random.Range(minMonsterLevel, maxMonsterLevel + 1);
        return monsterLevel;
    }


    // Darà una certa quantità di exp al giocatore quando sconfigge questo msotro
    public int GetPlayerExp() {

        int exponent = 2; // Esponente per una crescita quadratica

        return Mathf.RoundToInt(baseExpGivenToPlayer * Mathf.Pow(level, exponent));
    }

    public string getCurrentTemperAsString() {
        return System.Enum.GetName(typeof(Temper), currentTemper);
    }

    public Temper getCurrentTemper() {
        return this.currentTemper;
    }


}