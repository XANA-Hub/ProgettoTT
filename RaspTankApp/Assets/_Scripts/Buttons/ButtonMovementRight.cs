using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMovementRight : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED)
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStop);
    }


    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED)
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStart);
    }


}
