using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*

To use SceneHelper to load one scene:

SceneHelper.LoadScene ("JustOneScene");

To additively load multiple scenes:

SceneHelper.LoadScene( "Scene1");
SceneHelper.LoadScene( "Scene2", additive: true);
SceneHelper.LoadScene( "Lighting", additive: true, setActive: true);

*/


public static class SceneHelper  {

	// Permette di caricare la scena in maniera corretta e con diversi parametri
	public static void LoadScene(string s, bool additive = false, bool setActive = false) {

		if (s == null) {
			s = GetCurrentSceneName();
		}

		SceneManager.LoadScene (s, additive ?  LoadSceneMode.Additive : 0);

		if (setActive) {

            // to mark it active we have to wait a frame for it to load.
			CallAfterDelay.Create( 0, () => {
				SceneManager.SetActiveScene(
					SceneManager.GetSceneByName( s));
			});
		}
	}

	// Permette di de-caricare la scena
	public static void UnloadScene(string s) {
		SceneManager.UnloadSceneAsync(s);
	}


	// Permette di ottenere il nome della Scene attualmente in esecuzione
	public static string GetCurrentSceneName() {
		return SceneManager.GetActiveScene().name;
	}
}