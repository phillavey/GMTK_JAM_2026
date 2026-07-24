using UnityEngine;

[System.Serializable]
public struct SFXTrack
{
    public string trackName;
    public AudioClip trackClip;
}

public class SFXLibrary : MonoBehaviour
{
    public SFXTrack[] tracks;

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
