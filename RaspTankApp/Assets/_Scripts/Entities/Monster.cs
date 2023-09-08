using UnityEngine;

public class Monster : Fighter {


    // Metodo per inizializzare il mostro con un oggetto "MonsterData" e un livello calcolato
    public void InitializeMonster(FighterData monsterData, int playerLevel) {
        data = monsterData;

        // Calcola il livello del mostro in base al livello del giocatore
        level = CalculateMonsterLevel(playerLevel);
        CalculateStats(); // Calcola le statistiche effettive in base al livello
    }

    // Metodo per calcolare il livello del mostro in base al livello del giocatore
    private int CalculateMonsterLevel(int playerLevel) {
        int maxLevelDifference = 2; // Differenza massima tra il livello del mostro e il livello del giocatore
        int monsterLevel = playerLevel + Random.Range(-maxLevelDifference, maxLevelDifference + 1);

        // Assicurati che il livello del mostro sia sempre positivo o almeno 1
        monsterLevel = Mathf.Max(1, monsterLevel);

        return monsterLevel;
    }

    // Altri metodi o logica specifica del mostro possono essere aggiunti qui
}
