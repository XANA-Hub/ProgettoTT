using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAIRecognition : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        
        MasterManager.instance.clientTCPManager.SendData(RobotCommands.aiIdentifyCurrent);
        Debug.Log(this.gameObject.name + " OnPointerUp invocato!");
    }


}
