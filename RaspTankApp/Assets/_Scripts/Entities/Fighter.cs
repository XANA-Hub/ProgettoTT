using UnityEngine;

public class Fighter {

    // Satistiche base
    [Header("Statistiche base del combattente")]
    public FighterData data;
    
    // Statistiche effettive
    protected int level {get; set;}
    protected int actualAttack {get; set; }
    protected int actualDefense {get; set; }
    protected int actualHP {get; set; }


    // Metodo per calcolare le statistiche effettive in base al livello
    protected void CalculateStats() {
        int levelMultiplier = level * 2;
        actualAttack = data.baseAttack + levelMultiplier;
        actualDefense = data.baseDefense + levelMultiplier;
        actualHP  = data.baseHealth + (level * 10);
    }

    // Altri metodi o logica specifica del mostro possono essere aggiunti qui
    public void Attacca(Fighter fighter) {
        Debug.Log(this.data.name + " HA ATTACCATO " + fighter.data.name);
    }
}