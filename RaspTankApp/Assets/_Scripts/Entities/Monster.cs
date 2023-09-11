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
        level = CalculateLevel(MasterManager.instance.player.getLevel());
        Debug.Log("LIVELLO MOSTRO GENERATO: " + level);
        CalculateStats(); // Calcola le statistiche effettive in base al livello
    }

    // Metodo per calcolare le statistiche effettive in base al livello
    private void CalculateStats() {
        int levelMultiplier = level * 2;
        actualAttack = data.baseAttack + levelMultiplier;
        actualDefense = data.baseDefense + levelMultiplier;
        actualHP  = data.baseHealth + (level * 10);
    }
    
    // Metodo per calcolare il livello del mostro in base al livello del giocatore
    private int CalculateLevel(int playerLevel) {

        // Genera un livello casuale per il nemico basato sul livello del giocatore
        int livelloCasualeNemico = Random.Range(playerLevel - 2, playerLevel + 2);
        livelloCasualeNemico = Mathf.Max(1, livelloCasualeNemico);

        return livelloCasualeNemico;
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