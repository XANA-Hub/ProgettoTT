using UnityEngine;
using System.Collections;

/*

Use CallAfterDelay to cause something to happen later in Unity3D.

For instance:

CallAfterDelay.Create( 2.0f, () => {
Debug.Log( "This is two seconds later.");
});

To make it survive from scene to scene, simply set the returned reference as DontDestroyOnLoad(), like so:

DontDestroyOnLoad( CallAfterDelay.Create( 2.0f, () => {
Debug.Log( "This is two seconds later and survives scene changes.");
}));

----------------------

Specific use of playing a button sound and waiting for the sound to finish, then moving on:

AudioSource.PlayOneShot( ButtonAudioClip);
CallAfterDelay.Create( ButtonAudioClip.length, () => {
UnityEngine.SceneManagement.SceneManager.LoadScene( "MyNextScene");
});

Warning: you may want to disable the button too, otherwise the user can spam the sound.

----------------------

If you want a CallAfterDelay instance to die at the same time as the rest of your scene, so you don't get null references if you change scenes while one of these is pending, I find the easiest way is to parent the CallAfterDelay instance to the script where you call it from, something like this:

CallAfterDelay.Create( 2.0f, () => {
myButton.interactive = true;
}).transform.SetParent( myButton.transform);

That will set myButton to interactive in 2 seconds, but if you change scenes (or destroy myButton) before then, it avoids firing a pesky missing / destroyed reference.

*/


public class CallAfterDelay : MonoBehaviour {

	private float delay;
	private System.Action action;

	// Will never call this frame, always the next frame at the earliest
	public static CallAfterDelay Create( float delay, System.Action action) {
		CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
		cad.delay = delay;
		cad.action = action;
		return cad;
	}

	float age;

	void Update() {
		if (age > delay) {
			action();
			Destroy ( gameObject);
		}
	}
	void LateUpdate() {
		age += Time.deltaTime;
	}
}