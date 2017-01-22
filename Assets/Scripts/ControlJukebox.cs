using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJukebox : MonoBehaviour {

	private AudioSource[] backgroundTracks;
	private int lastTrackPlayed;

	public float exploredSpacePercentage = 0.0f;
	private float bgmBreakpointStep;

	private AudioSource[] dyingSounds;
	private AudioSource[] winningSounds;

	// Use this for initialization
	void Start () {
		// Background music
		backgroundTracks = new AudioSource[] {
			findAudioSource("LoopBeat"),
			findAudioSource("LoopOrbMoves1"),
			findAudioSource("LoopOrbMoves2"),
			findAudioSource("LoopOrbAddCry"),
			findAudioSource("LoopMidwayBeatAdd"),
			findAudioSource("LoopMidwayPad"),
			findAudioSource("LoopNearEndFlute"),
			findAudioSource("LoopNearEndStrings"),
		};
		bgmBreakpointStep = 1.0f / backgroundTracks.Length;

		foreach(AudioSource bgm in backgroundTracks) {
			bgm.volume = 0f;
			bgm.mute = false;
		}

		lastTrackPlayed = 0;
		StartCoroutine(unmuteBackgroundTrack(lastTrackPlayed, 0.5f));

		// Event sounds
		dyingSounds = new AudioSource[] {
			findAudioSource("OrbDies1"),
			findAudioSource("OrbDies2"),
		};

		winningSounds = new AudioSource[] {
			findAudioSource("OrbWins1"),
		};
	}
	
	// Update is called once per frame
	void Update () {
		exploredSpacePercentage = Time.timeSinceLevelLoad / 20.0f;

		if(exploredSpacePercentage > bgmBreakpointStep * (float)lastTrackPlayed) {
			addNextBackgroundTrack();
			// Special case MidwayPad #5 may not play over EndFlute #6:
			if(lastTrackPlayed == 6) {
				muteBackgroundTrack(5, 1.0f);
			}
		}
	}

	private AudioSource findAudioSource(string childName) {
		GameObject go = transform.FindChild(childName).gameObject;
		return go.GetComponent<AudioSource>();
	}

	private void addNextBackgroundTrack() {
		lastTrackPlayed += 1;
		StartCoroutine(unmuteBackgroundTrack(lastTrackPlayed, 1.0f));
	}

	private IEnumerator muteBackgroundTrack(int index, float time) {
		AudioSource snd = backgroundTracks[index];
		float volumeStep = snd.volume;
		while(snd.volume > 0.0f) {
			volumeStep -= Time.deltaTime;
			snd.volume = Mathf.Max(0.0f, volumeStep / time);
			yield return null;
		}
	}

	private IEnumerator unmuteBackgroundTrack(int index, float time) {
		AudioSource snd = backgroundTracks[index];
		float volumeStep = 0;
		while(snd.volume < 1.0f) {
			volumeStep += Time.deltaTime;
			snd.volume = Mathf.Min(1.0f, volumeStep / time);
			yield return null;
		}
	}

	public void playDeathSound() {
		int rndIndex = Random.Range(0, dyingSounds.Length);
		dyingSounds[rndIndex].Play();
	}

	public void playWinSound() {
		int rndIndex = Random.Range(0, winningSounds.Length);
		winningSounds[rndIndex].Play();
	}
}
