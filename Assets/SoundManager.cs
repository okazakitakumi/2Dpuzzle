using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSourceBGM = default;

    [SerializeField]
    AudioClip[] audioClips = default;

    [SerializeField]
    AudioSource audioSourceSE = default;

    [SerializeField]
    AudioClip[] SEClips = default;

    public enum BGM
    {
        Title,
        Game
    }

    public enum SE
    {
        Destroy,
        Touch,
    }
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Title);
    }
    public void PlayBGM(BGM bgm)
    {
        audioSourceBGM.clip = audioClips[(int)bgm];
        audioSourceBGM.Play();
    }
    public void PlaySE(SE se)
    {
        audioSourceSE.PlayOneShot(SEClips[(int)se]);
        
    }
}
