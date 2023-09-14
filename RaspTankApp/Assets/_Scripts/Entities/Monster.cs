using UnityEngine;

public class Monster : Fighter {

    private void Awake() {
        level = CalculateLevel(MasterManager.instance.player.getLevel(), 2);
    }

    private void Start() {
        InitializeStats();
    }

    public int CalculateLevel(int playerLevel, int levelVariation) {

        // Calcola un livello casuale entro l'intervallo di levelVariation
        int randomLevelOffset = Random.Range(-levelVariation, levelVariation + 1);

        // Calcola il livello del nemico in base al livello del giocatore e all'offset casuale
        int enemyLevel = playerLevel + randomLevelOffset;

        // Assicurati che il livello del nemico sia almeno 1 (non può essere negativo)
        enemyLevel = Mathf.Max(1, enemyLevel);

        return enemyLevel;
    }

}