
using UnityEngine;

public class Player : Fighter {

    // Punti esperienza del giocatore
    private int currentExp = 0;
    private int expRequiredForNextLevel = 1;

    [Header("Exp")]
    [Range(1, 500)] public int baseExpRequiredForNextLevel = 200;


    private void Awake() {
        level = 5;
        expRequiredForNextLevel = CalculateExpRequiredForNextLevel();
        CalculateStats();
    }

    public void GainExp(int amount) {

        currentExp += amount;

        // Controlla se il giocatore ha abbastanza esperienza per salire di livello
        while (currentExp >= expRequiredForNextLevel) {

            // Sali di livello
            level++;
            currentExp -= expRequiredForNextLevel;

            // Aumenta l'esperienza richiesta per il prossimo livello (puoi calcolarla in base al livello corrente)
            expRequiredForNextLevel = CalculateExpRequiredForNextLevel();

            // Ricalcola le statistiche del giocatore
            CalculateStats();
        }

        Debug.Log("NEW CURRENT PLAYER EXP: " + currentExp);
    }

    private int CalculateExpRequiredForNextLevel() {

        // Formula per una crescita esponenziale
        int exponent = 2; // Esponente per una crescita quadratica

        return Mathf.RoundToInt(baseExpRequiredForNextLevel * Mathf.Pow(level, exponent));
    }
    

    // 
    // Getters e Setters
    //

    public int getCurrentExp() {
        return currentExp;
    }


    public int getExpRequiredForNextLevel() {
        return expRequiredForNextLevel;
    }

    public void SetCurrentExp(int value) {
        currentExp = value;
    }

    public void SetExpRequiredForNextLevel(int value) {
        expRequiredForNextLevel = value;
    }

    public void SetLevel(int value) {
        level = value;
    }

   


}