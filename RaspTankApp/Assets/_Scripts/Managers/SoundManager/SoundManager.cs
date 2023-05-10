using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tutorial: https://www.youtube.com/watch?v=6OT43pvUyfY
// e https://www.youtube.com/watch?v=QL29aTa7J5Q
//

public class SoundManager: MonoBehaviour {

	public Sound[] sounds;
	private static Dictionary<string, float> soundTimerDictionary;
	private Sound music = null;
	private string previousMusicName;
	public string startingMusic;


	private void Awake() {
		
		soundTimerDictionary = new Dictionary<string, float>();

		// Ottengo tutti i suoni
		foreach(Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.isLoop;

			if(s.hasCooldown) {
				Debug.Log(s.name);
				soundTimerDictionary[s.name] = 0f;
			}
		}
	}

	private void Start() {

		// Musica menù principale, viene riprodotta subito
		PlayMusic(startingMusic);
	}

	
    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

	public void PlaySound(string name) {
		
		// Vogliamo trovare tra tutti i suoni, quello il cui nome
		// è uguale a "name"
		Sound sound = Array.Find(sounds, s => s.name == name);

		if (sound == null) {
			Debug.LogError("PlaySound: Suono " + name + " non trovato!");
			return;
		}

		if (!CanPlaySound(sound)) {
			return;
		}

		sound.source.Play();
	}

	public void PlayMusic(string name) {
		
		// Fermo la musica precedente
		if(music != null) {
			previousMusicName = music.name;
			
			music.source.Stop();
			// StartCoroutine(FadeOut(music, 1f));
		}

		// Vogliamo trovare tra tutti i suoni, quello il cui nome
		// è uguale a "name"
		music = Array.Find(sounds, s => s.name == name);

		if (music == null) {
			Debug.LogError("PlayMusic: Musica " + name + " non trovata!");
			return;
		}

		if (!CanPlaySound(music)) {
			return;
		}

		StartCoroutine(FadeIn(music, 1f));

	} 

	public void PlayPreviousMusic() {

		// Fermo la musica precedente
		if(music != null) {
			music.source.Stop();
			// StartCoroutine(FadeOut(music, 1f));
		}

		// Vogliamo trovare tra tutti i suoni, quello il cui nome
		// è uguale a "name"
		music = Array.Find(sounds, s => s.name == previousMusicName);

		if (music == null) {
			Debug.LogError("PlayPreviousMusic: Musica " + previousMusicName + " non trovata!");
			return;
		}

		if (!CanPlaySound(music)) {
			return;
		}

		StartCoroutine(FadeIn(music, 1f));

	}

	public void StopMusic(string name) {

		Sound sound = Array.Find(sounds, s => s.name == name);

		if (sound == null) {
			Debug.LogError("Stop: Suono " + name + " non trovato!");
			return;
		}

		StartCoroutine(FadeOut(sound, 0.5f));
	}

	public void StopSound(string name) {

		Sound sound = Array.Find(sounds, s => s.name == name);

		if (sound == null) {
			Debug.LogError("Stop: Suono " + name + " non trovato!");
			return;
		}

		sound.source.Stop();
	}

	private static bool CanPlaySound(Sound sound) {
		
		if (soundTimerDictionary.ContainsKey(sound.name)) {
			float lastTimePlayed = soundTimerDictionary[sound.name];

			if ((lastTimePlayed + sound.clip.length) < Time.time) {
				soundTimerDictionary[sound.name] = Time.time;
				return true;
			}

			return false;
		}

		return true;
	}


	// ! ////////////////////////
	// !
	// ! FADE IN - FADE OUT
	// !
	// ! ////////////////////////


    private static IEnumerator FadeOut(Sound sound, float FadeTime) {

        float startVolume = sound.source.volume;
 
        while (sound.source.volume > 0) {
            sound.source.volume -= startVolume * Time.deltaTime / FadeTime;
			
            yield return null;
        }
 
        sound.source.Stop();
        sound.source.volume = startVolume;
    }
 
    private static IEnumerator FadeIn(Sound sound, float FadeTime) {

        float startVolume = 0.2f;
 
        sound.source.volume = 0;
        sound.source.Play();
 
        while (sound.source.volume < sound.volume) {
            sound.source.volume += startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        sound.source.volume = sound.volume;
    }
}


//
// Usage (New):
//
// SoundManager.GetInstance().PlaySound("PlayerRunning");
// SoundManager.GetInstance().Stop("PlayerRunning");
//
//
// Fade In - Fade Out:
//
// StartCoroutine(AudioFadeScript.FadeIn(audioGameMusic, 5f)); - Start Music
// StartCoroutine(AudioFadeScript.FadeOut(audioMenuMusic, 0.5f)); - Stop Music
//

