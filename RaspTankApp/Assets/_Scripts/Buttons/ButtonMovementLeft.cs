using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMovementLeft : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonMovementLeft: rilasciato");
            base.OnPointerUp(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStop);
        } else {
            Debug.Log("ButtonMovementLeft: rilasciato ma non connesso!");
        }
    }


    public override void OnPointerDown(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonMovementLeft: premuto");
            base.OnPointerDown(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStart);
        } else {
            Debug.Log("ButtonMovementLeft: premuto ma non connesso!");
        }
    }


}
