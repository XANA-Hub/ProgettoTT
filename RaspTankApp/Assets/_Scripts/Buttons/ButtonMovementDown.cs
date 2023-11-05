using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMovementDown : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonMovementDown: rilasciato");
            base.OnPointerUp(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.backwardStop);
        } else {
            Debug.Log("ButtonMovementDown: rilasciato ma non connesso!");
        }
    }


    public override void OnPointerDown(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonMovementDown: premuto");
            base.OnPointerDown(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.backwardStart);
        } else {
            Debug.Log("ButtonMovementDown: premuto ma non connesso!");
        }

    }


}
