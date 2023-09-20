using UnityEngine;

public class StartingWarningController : MonoBehaviour {

    public string mainMenuScene = "MainMenu";
    
    public void ContinueButton() {

        SceneHelper.LoadScene(mainMenuScene);
    }

    public void ExitButton() {
        Application.Quit();
    }
}
