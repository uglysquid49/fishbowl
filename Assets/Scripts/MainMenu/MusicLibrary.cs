using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;

    public AudioClip GetClipFromName(string trackname)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == trackname)
            {
                return track.clip;
            }
        }
        return null;
    }
}
