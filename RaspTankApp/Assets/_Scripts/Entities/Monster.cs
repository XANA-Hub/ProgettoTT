using UnityEngine;

public class Monster : Fighter {

    // Indica come si comporterà il mostro
    // AGRRESSIVE: preferisce attaccare
    // DEFENSIVE: preferisce difendere
    private Temper currentTemper;

    private void Awake() {
        level = CalculateLevel(MasterManager.instance.player.getLevel(), MasterManager.instance.battleManager.monsterLevelVariation);

        InitializeStats();

        currentTemper = ChooseTemper();
    }

    private Temper ChooseTemper() {

        if(currentNatureBonus == NatureBonus.ATTACK || currentNatureBonus == NatureBonus.SPEED || currentNatureBonus == NatureBonus.HEAL) {
            currentTemper = Temper.AGGRESSIVE;
        }else if(currentNatureBonus == NatureBonus.DEFENSE || currentNatureBonus == NatureBonus.HP) {
            currentTemper = Temper.DEFENSIVE;
        }

        return currentTemper;
    }

    private int CalculateLevel(int playerLevel, int levelVariation) {

        int minMonsterLevel = Mathf.Max(1, playerLevel - levelVariation);
        int maxMonsterLevel = playerLevel + levelVariation;

        int monsterLevel = Random.Range(minMonsterLevel, maxMonsterLevel + 1);
        return monsterLevel;
    }

    public string getCurrentTemperAsString() {
        return System.Enum.GetName(typeof(Temper), currentTemper);
    }

    public Temper getCurrentTemper() {
        return this.currentTemper;
    }


}