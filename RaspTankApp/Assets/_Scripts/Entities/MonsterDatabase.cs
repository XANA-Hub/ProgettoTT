using UnityEngine;

public class MonsterDatabase : MonoBehaviour {

    [SerializeField] private Monster[] monsters;

    // Metodo per ottenere un mostro casuale con il nome e lo sprite associato
    public Monster GetRandomMonster() {
        int randomIndex = Random.Range(0, monsters.Length);
        return monsters[randomIndex];
    }

    public Monster GetSpecificMonster(string monsterName) {
        
        // Trovo il mostro in base al nome
        for(int i=0; i<monsters.Length; i++) {
            if (monsters[i].data.name == monsterName) {
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
