using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudio : MonoBehaviour
{
    public static CharacterAudio instance;
    private AudioSource audioSource;

    [Header("Clips")]
    public AudioClip dieSound;
    public AudioClip ShotSound;
    

    [Header("Pitch Random")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        instance = this;
    }

    public void Play(AudioClip clip)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }

    public void PlayDieSound()
    {
        Play(dieSound);
    }

    public void PlayShotSound()
    {
        Play(ShotSound);
    }
}