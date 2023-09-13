using UnityEngine;

public class Player : MonoBehaviour {

    // Satistiche base
    [Header("Statistiche base del giocatore")]
    public FighterData data;
    
    // Statistiche effettive
    private int level = 1;
    private int actualAttack;
    private int actualDefense;
    private int actualHP;


    private void Awake() {
        CalculateStats(); // Calcola le statistiche effettive in base al livello
    }

    // Metodo per calcolare le statistiche effettive in base al livello
    private void CalculateStats() {
        int levelMultiplier = level * 2;
        actualAttack = data.baseAttack + levelMultiplier;
        actualDefense = data.baseDefense + levelMultiplier;
        actualHP  = data.baseHealth + (level * 10);
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