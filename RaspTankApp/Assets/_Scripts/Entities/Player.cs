using Unity.VisualScripting;
using UnityEngine;


public class Player : Fighter {


    // Metodo per inizializzare il mostro con un oggetto "MonsterData" e un livello calcolato
    public void InitializePlayer(FighterData playerData) {
        data = playerData;
        
        CalculateStats(); // Calcola le statistiche effettive in base al livello
    }

    // Altri metodi o logica specifica del mostro possono essere aggiunti qui
}
