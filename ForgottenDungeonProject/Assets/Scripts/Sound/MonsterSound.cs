using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class MonsterSound : MonoBehaviour
{
    [Header("===== 플레이어 설정 =====")]

    [Tooltip("플레이어 Transform (보통 Player 오브젝트)")]
    public Transform player;

    [Header("===== 몬스터 사운드 설정 =====")]

    [Tooltip("몬스터 사운드 클립")]
    public AudioClip monsterSound;

    [Tooltip("사운드가 들리기 시작하는 거리")]
    [Range(1f, 50f)]
    public float soundDistance = 15f;

    [Tooltip("사운드 볼륨")]
    [Range(0f, 1f)]
    public float volume = 0.8f;

    [Tooltip("사운드 반복 여부")]
    public bool loop = true;

    private AudioSource audioSource;
    private bool isPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = monsterSound;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;

        // 3D 사운드
        audioSource.spatialBlend = 1f;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= soundDistance)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                audioSource.Play();
            }
        }
        else
        {
            if (isPlaying)
            {
                isPlaying = false;
                audioSource.Stop();
            }
        }
    }
}