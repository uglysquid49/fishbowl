using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; //acess from anywhere in code

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    [SerializeField] private AudioSource sfx3DSource;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null && sfx3DSource != null)
        {
            sfx3DSource.transform.position = pos;
            sfx3DSource.PlayOneShot(clip);
        }
    }
    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    } //proximity

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }

}

