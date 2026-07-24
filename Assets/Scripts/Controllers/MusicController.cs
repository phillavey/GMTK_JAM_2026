using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            EVENT_BUS.Subscribe(EventType.REQUEST_MUSIC, PlayMusic);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlayMusic(PublishEventArgs args)
    {
        // May not need this
        if (args.Data.TryGetValue("track", out object trackName))
        {
            Debug.Log("Found a track in args");
            PlayMusic(trackName.ToString());
        }
    }

    public void PlayMusic(string track, float fadeTime = 0.5f)
    {
        // Just access this via the singleton instance
        StartCoroutine(PlayMusicCrossfade(musicLibrary.getTrack(track), fadeTime));
    }

    public IEnumerator PlayMusicCrossfade(AudioClip track, float fadeTime = 0.5f)
    {
        // Fade out current track
        float percent = 0f;
        while (percent < 1)
        {
            percent += Time.unscaledDeltaTime * 1 / fadeTime;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        // Fade in new track
        musicSource.clip = track;
        musicSource.Play();

        percent = 0f;
        while (percent < 1)
        {
            percent += Time.unscaledDeltaTime * 1 / fadeTime;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }
}
