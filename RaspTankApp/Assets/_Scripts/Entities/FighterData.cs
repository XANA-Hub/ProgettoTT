using UnityEngine;

[System.Serializable]
public class FighterData {

    // Attributi base
    public string name;     // Nome dell'entità
    public Sprite sprite;   // Sprite dell'entità

    // Altri dati necessari per calcolare le statistiche effettive
    public int baseAttack;         // Punti di attacco di base
    public int baseDefense;        // Punti di difesa di base
    public int baseHP;             // Punti vita di base
    public int baseHeal;           // Punti di cura base
    public int baseSpeed;          // Punti di velocità base
}