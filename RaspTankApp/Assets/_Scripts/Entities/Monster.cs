using UnityEngine;

public class Monster : Fighter {

    private void Awake() {
        this.level = CalculateLevel(MasterManager.instance.player.getLevel(), MasterManager.instance.battleManager.monsterLevelVariation);
        InitializeStats();
    }

    private int CalculateLevel(int playerLevel, int levelVariation) {

        int minMonsterLevel = Mathf.Max(1, playerLevel - levelVariation);
        int maxMonsterLevel = playerLevel + levelVariation;

        int monsterLevel = Random.Range(minMonsterLevel, maxMonsterLevel + 1);
        return monsterLevel;
    }

}