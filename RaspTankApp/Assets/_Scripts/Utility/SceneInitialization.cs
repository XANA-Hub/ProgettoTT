using UnityEngine;

public class SceneInitialization : MonoBehaviour {

    //
    // Managers
    //

    [SerializeField] private bool audioManagerEnabled = false;
    [SerializeField] private bool clientTCPManagerEnabled = false;
    [SerializeField] private bool videoReceiverManagerEnabled = false;
    [SerializeField] private bool battleManagerEnabled = false;
    [SerializeField] private bool monsterDatabaseEnabled = false;
    [SerializeField] private bool battleHUDEnabled = false;
    [SerializeField] private bool battleEffectsManagerEnabled = false;
    [SerializeField] private bool robotHUDEnabled = false;
    [SerializeField] private bool connectionStateManagerEnabled = false;



    private void Awake() {

        // 
        // Managers
        //

        if(audioManagerEnabled) {
            MasterManager.instance.audioManager.Enable();
        } else {
            MasterManager.instance.audioManager.Disable();
        }
        if(clientTCPManagerEnabled) {
            MasterManager.instance.clientTCPManager.Enable();
        } else {
            MasterManager.instance.clientTCPManager.Disable();
        }
        if(videoReceiverManagerEnabled) {
            MasterManager.instance.videoReceiverManager.Enable();
        } else {
            MasterManager.instance.videoReceiverManager.Disable();
        }
        if(battleManagerEnabled) {
            MasterManager.instance.battleManager.Enable();
        } else {
            MasterManager.instance.battleManager.Disable();
        }
        if(monsterDatabaseEnabled) {
            MasterManager.instance.monsterDatabase.Enable();
        } else {
            MasterManager.instance.monsterDatabase.Disable();
        }
        if(robotHUDEnabled) {
            MasterManager.instance.robotHUD.SetActive(true);
        } else {
            MasterManager.instance.robotHUD.SetActive(false);
        }
        if(battleHUDEnabled) {
            MasterManager.instance.battleHUD.SetActive(true);
        } else {
            MasterManager.instance.battleHUD.SetActive(false);
        }
        if(battleEffectsManagerEnabled) {
            MasterManager.instance.battleEffectsManager.Enable();
        } else {
            MasterManager.instance.battleEffectsManager.Disable();
        }
        if(connectionStateManagerEnabled) {
            MasterManager.instance.connectionStateManager.Enable();
        } else {
            MasterManager.instance.connectionStateManager.Disable();
        }

       
    }


}
