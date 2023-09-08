using UnityEngine;

[System.Serializable]
public class FighterData {
    public string name;     // Nome dell'entità
    public Sprite sprite;   // Sprite dell'entità

    // Altri dati necessari per calcolare le statistiche effettive
    public int baseAttack;         // Punti di attacco di base
    public int baseDefense;        // Punti di difesa di base
    public int baseHealth;         // Punti vita di base
    public int baseHeal;           // Punti di cura base
}