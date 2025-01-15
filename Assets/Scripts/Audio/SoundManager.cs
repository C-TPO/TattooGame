using Unity.Mathematics;

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {get; private set;}

    [SerializeField] private AudioSource sfxSourcePrefab;

    private void Awake()
    {
         if(instance != null)
        {
            Debug.Log("FOUND MULTIPLE SOUND MANAGERS IN SCENE! DESTROYING NEWEST INSTANCE.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip, Transform spawnTransform, float volume = 1f)
    {
        AudioSource source = Instantiate(sfxSourcePrefab, spawnTransform.position, quaternion.identity);
        source.clip = clip;
        source.volume = volume;
        source.Play();

        float clipLength = source.clip.length;
        Destroy(source.gameObject, clipLength);
    }
}
