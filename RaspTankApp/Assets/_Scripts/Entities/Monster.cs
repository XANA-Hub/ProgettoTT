using UnityEngine;

public class Monster : Fighter {

    private void Awake() {
        level = CalculateLevel(MasterManager.instance.player.getLevel(), MasterManager.instance.battleManager.monsterLevelVariation);
    }

    private void Start() {
        InitializeStats();
    }

    public int CalculateLevel(int playerLevel, int levelVariation) {

        int monsterLevel = Random.Range(playerLevel-levelVariation, playerLevel + 1 + levelVariation);

        if(monsterLevel <= 0) {
            monsterLevel = 1;
        }

        return monsterLevel;

    }

}