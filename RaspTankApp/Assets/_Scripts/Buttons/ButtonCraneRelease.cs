using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCraneRelease : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {

        base.OnPointerUp(eventData);

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED)
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armRelease);
    }


}
