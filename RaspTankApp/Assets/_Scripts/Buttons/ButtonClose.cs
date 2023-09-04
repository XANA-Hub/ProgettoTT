using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClose : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        
        MasterManager.instance.clientTCPManager.SendData(RobotCommands.disconnect);
        Debug.Log(this.gameObject.name + " OnPointerUp invocato!");
    }



}
