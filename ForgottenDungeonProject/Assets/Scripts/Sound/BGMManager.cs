using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    [Header("===== BGM 설정 =====")]

    [Tooltip("이 씬에서 재생할 음악")]
    public AudioClip bgmClip;

    [Tooltip("볼륨")]
    [Range(0f, 1f)]
    public float volume = 0.6f;

    [Tooltip("반복 재생 여부")]
    public bool loop = true;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = bgmClip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.spatialBlend = 0f; // 2D 사운드
        audioSource.Play();
    }
}