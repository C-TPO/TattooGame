using UnityEngine;

public class OneShotSFX : MonoBehaviour
{
    [SerializeField] private AudioClip sfxClip = null;

    public void PlaySFX()
    {
        if(!sfxClip)
            return;
        
        SoundManager.instance.PlaySound(sfxClip, transform);
    }
}
