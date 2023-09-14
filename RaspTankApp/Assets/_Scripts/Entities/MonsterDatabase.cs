using UnityEngine;



public class MonsterDatabase : MonoBehaviour {

    public Monster[] monsters;

    // Metodo per ottenere un mostro casuale con il nome e lo sprite associato
    public Monster GetRandomMonster() {
        int randomIndex = Random.Range(0, monsters.Length);
        return monsters[randomIndex];
    }

    /*
    public Player GetPlayer() {

    } 
    */
    
    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }


}
