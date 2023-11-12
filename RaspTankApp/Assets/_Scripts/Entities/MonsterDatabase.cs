using UnityEngine;



public class MonsterDatabase : MonoBehaviour {

    public Monster[] monsters;

    // Metodo per ottenere un mostro casuale con il nome e lo sprite associato
    public Monster GetRandomMonster() {
        int randomIndex = Random.Range(0, monsters.Length);
        return monsters[randomIndex];
    }

    public Monster GetSpecificMonster(string monsterName) {

        // Trovo il mostro in base al nome
        for(int i=0; i<monsterName.Length; i++) {
            if (monsters[i].name == monsterName) {
                return monsters[i];
            }
        }

        // Se il mostro specificato non è stato trovato, ne restituisco uno a caso
        return GetRandomMonster();
    }

    
    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }


}
