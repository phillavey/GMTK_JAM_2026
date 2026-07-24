using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip trackClip;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;

    public AudioClip getTrack(string name)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == name)
            {
                return track.trackClip;
            }
        }
        return null;

    }
}