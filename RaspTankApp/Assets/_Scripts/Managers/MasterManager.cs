using UnityEngine;

public class MasterManager : MonoBehaviour {


    // Istanza principale
    public static MasterManager instance { get; private set; }

    //
    // Manager
    //
    public AudioManager audioManager { get; private set; }
    public ClientTCPManager clientTCPManager { get; private set; }
    public VideoReceiverManager videoReceiverManager { get; private set; }
    public GameObject battleHUD { get; private set; }
    public GameObject robotHUD {get; private set;}
    public GameObject connectionState {get; private set;}
    public Player player { get; private set; }
    public MonsterDatabase monsterDatabase { get; private set; }
    public BattleManager battleManager { get; private set; }
    public BattleEffectsManager battleEffectsManager { get; private set; }


    //
    // Metodi
    //

    private void Awake() {

        // Se c'è già un'istanza, la distruggo
        if(instance != null) {

            Destroy(this.gameObject);
            

            return;
        }

        instance = this;


        // Ottengo i vari GameObject figli
        robotHUD = this.transform.GetChild(0).GetChild(0).gameObject;
        battleHUD = this.transform.GetChild(0).GetChild(1).gameObject;
        connectionState = this.transform.GetChild(0).GetChild(2).gameObject;
        videoReceiverManager = GetComponentInChildren<VideoReceiverManager>();
        audioManager = GetComponentInChildren<AudioManager>();
        clientTCPManager = GetComponentInChildren<ClientTCPManager>();
        player = GetComponentInChildren<Player>();
        monsterDatabase = GetComponentInChildren<MonsterDatabase>();
        battleManager = GetComponentInChildren<BattleManager>();
        battleEffectsManager = GetComponentInChildren<BattleEffectsManager>();
        

    }
    


}
