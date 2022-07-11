using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public enum SoundTrigger
{
    Match,
    Cut,
    Win,
    Lose,
}

[System.Serializable]
public class Sounds : SerializableDictionaryBase<SoundTrigger, AudioSource> { }

public class SoundManager : MonoBehaviour
{
    public Sounds sounds;

    public static SoundManager Instance;

    private int matchCombo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        matchCombo = 0;
    }

    public void PlaySound(SoundTrigger soundTrigger, bool onlyIfThisSoundNotPlaying = false, bool onlyIfNoSoundsPlaying = false)
    {
        if (onlyIfThisSoundNotPlaying)
        {
            if (sounds[soundTrigger].isPlaying)
                return;
        }
        else if (onlyIfNoSoundsPlaying)
        {
            foreach (var sound in sounds)
            {
                if (sound.Value.isPlaying)
                    return;
            }
        }

        sounds[soundTrigger].Play();
    }

    public void PlayPlatformSound(bool isMatch)
    {
        if (isMatch)
        {
            sounds[SoundTrigger.Match].pitch = 1 + ((float)matchCombo * 0.1f);

            sounds[SoundTrigger.Match].Play();

            matchCombo++;
        }
        else
        {
            matchCombo = 0;
            sounds[SoundTrigger.Cut].Play();

        }
    }

    public void TryPlaySound(string sound)
    {
        SoundTrigger soundTrigger;
        if (System.Enum.TryParse<SoundTrigger>(sound, out soundTrigger))
        {
            sounds[soundTrigger].Play();
        }
    }
}
