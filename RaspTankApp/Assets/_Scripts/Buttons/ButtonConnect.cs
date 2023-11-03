using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonConnect : Button {
    
    public override void OnPointerUp(PointerEventData eventData) {

        base.OnPointerUp(eventData);
        MasterManager.instance.clientTCPManager.SendData(RobotCommands.start);
        MasterManager.instance.clientTCPManager.RetryConnectionAfterFailure();
    }


}
