using UnityEngine;

// Tutorial: https://www.youtube.com/watch?v=6OT43pvUyfY
// e https://www.youtube.com/watch?v=QL29aTa7J5Q

[System.Serializable]
public class Sound {
	
	public string name;

	public AudioClip clip;

	// Slider
	[Range(0f, 1f)]
	public float volume = 1f;

	// Slider
	[Range(.1f, 3f)]
	public float pitch = 1f;

	public bool isLoop;
	public bool hasCooldown;

	[HideInInspector]
	public AudioSource source;
}