using System.Collections;
using UnityEngine;


// Classe che contiene i movimenti che il robot farà durante la battaglia
public class RobotBattleAnimations {

    public static IEnumerator PlayerAttackRobotAnimation() {
        
        if (MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            
            // Attacco: movimento braccio su e giù
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armUp);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armGrab);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armRelease);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armDown);
            yield return new WaitForSeconds(0.5f);
        }
        else {
            Debug.Log("RobotBattleAnimations: robot non connesso, animazione non disponibile!");
        }

    }

    public static IEnumerator PlayerDamagedRobotAnimation() {

        if (MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            // Genera un numero casuale 0 o 1
            int randomValue = Random.Range(0, 2);

            if (randomValue == 0) {

                // Movimento SX
                MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStart);
                yield return new WaitForSeconds(5f); // TODO
                MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStop);

            }
            else {

                // Movimento DX
                MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStart);
                yield return new WaitForSeconds(5f); // TODO
                MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStop);

            }
        }
        else {
            Debug.Log("RobotBattleAnimations: robot non connesso, animazione non disponibile!");
        }

    }

    public static IEnumerator PlayerDefendRobotAnimation() {

        if (MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            
            // Movimento SX e DX
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStart);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStop);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStart);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStop);
            yield return new WaitForSeconds(0.5f);

            // Movimento braccio su e giù
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armUp);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armDown);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armUp);
            yield return new WaitForSeconds(0.5f);
        }
        else {
            Debug.Log("RobotBattleAnimations: robot non connesso, animazione non disponibile!");
        }

    }


    
    public static IEnumerator PlayerHealRobotAnimation() {

        if (MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            // Movimento braccio su e giù
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armUp);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armDown);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armUp);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armDown);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armUp);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armDown);
            yield return new WaitForSeconds(0.5f);
        }
        else {
            Debug.Log("RobotBattleAnimations: robot non connesso, animazione non disponibile!");
        }

    }

    public static IEnumerator PlayerFleeRobotAnimation() {

        if (MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            // Movimento SX e DX
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStart);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStop);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStart);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStop);
            yield return new WaitForSeconds(0.5f);

            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStart);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStop);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStart);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStop);
            yield return new WaitForSeconds(0.5f);

            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStart);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateLeftStop);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStart);
            yield return new WaitForSeconds(0.5f);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStop);
            yield return new WaitForSeconds(0.5f);
        }
        else {
            Debug.Log("RobotBattleAnimations: robot non connesso, animazione non disponibile!");
        }
    }
    
}