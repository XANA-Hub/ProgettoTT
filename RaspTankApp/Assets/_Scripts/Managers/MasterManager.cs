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
    public GameObject hud { get; private set; }
    public Player player { get; private set; }
    public MonsterDatabase monsterDatabase { get; private set; }
    public BattleManager battleManager { get; private set; }


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
        hud = this.transform.GetChild(0).gameObject;
        videoReceiverManager = GetComponentInChildren<VideoReceiverManager>();
        audioManager = GetComponentInChildren<AudioManager>();
        clientTCPManager = GetComponentInChildren<ClientTCPManager>();
        player = GetComponentInChildren<Player>();
        monsterDatabase = GetComponentInChildren<MonsterDatabase>();
        battleManager = GetComponentInChildren<BattleManager>();
        //robotResponseManager = GetComponentInChildren<RobotResponseManager>();

    }
    


}
