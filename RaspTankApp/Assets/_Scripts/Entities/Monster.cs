using UnityEngine;

public class Monster : MonoBehaviour {

    // Satistiche base
    [Header("Statistiche base del mostro")]
    public FighterData data;
    
    // Statistiche effettive
    private int level = 1;
    private int actualAttack;
    private int actualDefense;
    private int actualHP;


    // Metodo per inizializzare il mostro con un oggetto "MonsterData" e un livello calcolato
    public void Awake() {

        // Calcola il livello del mostro in base al livello del giocatore
        level = CalculateLevel(MasterManager.instance.player.getLevel(), 2);
        Debug.Log("LIVELLO MOSTRO GENERATO: " + level);
        CalculateStats(); // Calcola le statistiche effettive in base al livello
        Debug.Log("ACTUAL HPPP: " + actualHP);
    }

    // Metodo per calcolare le statistiche effettive in base al livello
    private void CalculateStats() {
        int levelMultiplier = level * 2;
        actualAttack = data.baseAttack + levelMultiplier;
        actualDefense = data.baseDefense + levelMultiplier;
        actualHP  = data.baseHealth + (level * 10);
    }
    
    // Metodo per calcolare il livello del mostro in base al livello del giocatore
    public int CalculateLevel(int playerLevel, int levelVariation) {

        // Calcola un livello casuale entro l'intervallo di levelVariation
        int randomLevelOffset = Random.Range(-levelVariation, levelVariation + 1);

        // Calcola il livello del nemico in base al livello del giocatore e all'offset casuale
        int enemyLevel = playerLevel + randomLevelOffset;

        // Assicurati che il livello del nemico sia almeno 1 (non può essere negativo)
        enemyLevel = Mathf.Max(1, enemyLevel);

        return enemyLevel;
    }

    public bool takeDamage(int dmgAmount) {

        actualHP -= dmgAmount;

        // Morto
        if(actualHP <= 0) 
            return true;
        else
            return false;
    }


    public int getLevel() {
        return this.level;
    }
    public int getActualAttack() {
        return this.actualAttack;
    }
    public int getActualDefense() {
        return this.actualDefense;
    }
    public int getActualHP() {
        return this.actualHP;
    }

}