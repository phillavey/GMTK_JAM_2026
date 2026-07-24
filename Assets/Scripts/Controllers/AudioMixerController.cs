using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("masterVolume", linearize(volume));
    }
    public void SetSoundEffectsVolume(float volume)
    {
        mixer.SetFloat("soundEffectsVolume", linearize(volume));

    }
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("musicVolume", linearize(volume));

    }

    private float linearize(float val)
    {
        return Mathf.Log10(val) * 20;
    }
}
