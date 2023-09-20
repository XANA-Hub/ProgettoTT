using UnityEngine;

[System.Serializable]
public class FighterData {

    // Attributi base

    [Header("Base parameters")]
    public string name;     // Nome dell'entità
    public Sprite sprite;   // Sprite dell'entità

    // Altri dati necessari per calcolare le statistiche effettive

    [Header("Nature bonus")]
    public NatureBonus preferredNatureBonus;        // Indica quale sarà il bonus natura che verrà scelto con più probabilità
    
    [Header("Base stats")]
    [Range(50, 200)] public int baseHP;             // Punti vita di base
    [Range(50, 200)] public int baseAttack;         // Punti di attacco di base
    [Range(50, 200)] public int baseDefense;        // Punti di difesa di base
    [Range(50, 200)] public int baseSpeed;          // Punti di velocità base
    [Range(10, 50)] public int baseHeal;            // Punti di cura base
    [Range(1, 40)] public int baseAttackDamage;     // Danno base dell'attacco
    
    [Header("Other battle parameters")]
    [Range(0f, 1f)] public float healProbability = 0.45f; // 45%
    [Range(0f, 1f)] public float baseFleeProbability = 0.5f; // 50%
    [Range(0f, 1f)] public float criticalHitProbability = 0.125f; // Probabilità che faccia un colpo critico
    [Range(0f, 1f)] public float missProbability = 0.05f; // Probabilità che missi l'attacco
    [Range(0f, 1f)] public float natureBonusPercentage = 0.1f; // Di quanto viene aumentata la statistica che fa riferimento alla natura scelta (10%)
    [Range(0f, 1f)] public float preferredNatureProbability = 0.7f; // Quanto è probabile che venga scelta la natura preferita (70%)

}