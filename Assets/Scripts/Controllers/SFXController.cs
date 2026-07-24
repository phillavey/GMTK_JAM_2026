using UnityEngine;

public class SFX_Controller : MonoBehaviour
{
    public static SFX_Controller instance;

    [SerializeField] private AudioSource soundEffectsSource;
    [SerializeField] private SFXLibrary sfxLibrary;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        EVENT_BUS.Subscribe(EventType.ATTACK, HandleAttackSFX);
    }

    private void OnDisable()
    {
        EVENT_BUS.Unsubscribe(EventType.ATTACK, HandleAttackSFX);
    }

    void HandleAttackSFX(PublishEventArgs args)
    {
        Debug.Log("HandleAttackSFX Fired!!!");
        playSFX("attack", player.transform, 1f);
    }

    public void playSFX(string clipName, Transform trans, float volume = 0f)
    {
        PlaySoundClip(sfxLibrary.getTrack(clipName), trans, volume);
    }

    public void PlaySoundClip(AudioClip clip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundEffectsSource, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipDuration = clip.length;

        Destroy(audioSource.gameObject, clipDuration);
    }
}
