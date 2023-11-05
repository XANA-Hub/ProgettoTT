using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonConnect : Button {
    
    public override void OnPointerUp(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTION_ABORTED || 
            MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.NOT_CONNECTED) {
            Debug.Log("ButtonConnect: premuto");
            base.OnPointerUp(eventData);
            MasterManager.instance.clientTCPManager.RetryConnectionAfterFailure();
        } else {
            Debug.Log("ButtonConnect: premuto ma già connesso o in connessione!");
        }
    }


}
