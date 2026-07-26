using UnityEngine;
using UnityEngineInternal;

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
        EVENT_BUS.Subscribe(EventType.NUKEING_IS_NOW_LEGAL, PlayNukeSFX);
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, PlaySuccSFX);
    }

    private void OnDisable()
    {
        EVENT_BUS.Unsubscribe(EventType.ATTACK, HandleAttackSFX);
        EVENT_BUS.Unsubscribe(EventType.NUKEING_IS_NOW_LEGAL, PlayNukeSFX);
    }

    void PlaySuccSFX(PublishEventArgs args)
    {
        playSFX("task_success", player.transform, 0.75f);
    }

    void PlayNukeSFX(PublishEventArgs args)
    {
        playSFX("nuke_alert", player.transform, 0.75f);
    }

    void HandleAttackSFX(PublishEventArgs args)
    {
        playSFX("spell_cast_progress", player.transform, 1f);
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
