using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDisconnect : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        
        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonDisconnect: premuto");
            base.OnPointerUp(eventData);
            MasterManager.instance.clientTCPManager.DisconnectFromServer();
        } else {
            Debug.Log("ButtonDisconnect: premuto ma non connesso o in connessione!");
        }
            
    }



}
