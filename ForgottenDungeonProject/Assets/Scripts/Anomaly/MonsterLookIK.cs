using UnityEngine;

[DisallowMultipleComponent]
public class MonsterLookIK : MonoBehaviour
{
    [Header("===== 시선(IK) 설정 =====")]

    [Tooltip("고개가 바라볼 목표 지점(하늘을 보게 하려면 머리 위/앞에 Empty 오브젝트를 둔 뒤 연결)")]
    [SerializeField] private Transform lookTarget;

    [Tooltip("시선 적용 강도(0=적용 안함, 1=최대)")]
    [Range(0f, 1f)]
    [SerializeField] private float weight = 1f;

    [Tooltip("몸(body) 회전에 영향을 주는 정도(1이면 상체도 같이 따라갈 수 있음)")]
    [Range(0f, 1f)]
    [SerializeField] private float bodyWeight = 0.2f;

    [Tooltip("머리(head) 회전에 영향을 주는 정도(고개만 움직이게 하려면 이 값을 높게)")]
    [Range(0f, 1f)]
    [SerializeField] private float headWeight = 1f;

    [Tooltip("눈(eyes) 회전에 영향을 주는 정도(모델에 눈 본이 없으면 체감 적음)")]
    [Range(0f, 1f)]
    [SerializeField] private float eyesWeight = 0.8f;

    [Tooltip("상체/목 회전의 자연스러움 보정(0~1). 보통 0.2~0.6 권장")]
    [Range(0f, 1f)]
    [SerializeField] private float clampWeight = 0.5f;

    [Tooltip("시선이 갑자기 튀지 않도록 부드럽게 따라가는 속도(값이 클수록 빠름)")]
    [Range(1f, 30f)]
    [SerializeField] private float followSpeed = 10f;

    [Tooltip("시선을 켜고 끄는 스위치(테스트용)")]
    [SerializeField] private bool enableLook = true;

    private Animator animator;
    private Vector3 smoothedLookPos;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("[MonsterLookIK] Animator를 찾을 수 없습니다. 몬스터에 Animator가 있는지 확인하세요.");
        }
    }

    private void Start()
    {
        smoothedLookPos = (lookTarget != null) ? lookTarget.position : transform.position + transform.forward * 5f + Vector3.up * 2f;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // 타겟이 없으면 LookAt을 끔(NullReference 방지)
        if (!enableLook || lookTarget == null || weight <= 0f)
        {
            animator.SetLookAtWeight(0f);
            return;
        }

        // 목표 위치를 부드럽게 보정(튀는 현상 완화)
        smoothedLookPos = Vector3.Lerp(smoothedLookPos, lookTarget.position, Time.deltaTime * followSpeed);

        // LookAt 적용
        animator.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
        animator.SetLookAtPosition(smoothedLookPos);
    }

#if UNITY_EDITOR
    // Scene 뷰에서 타겟 확인용(선택)
    private void OnDrawGizmosSelected()
    {
        if (lookTarget == null) return;
        Gizmos.DrawLine(transform.position + Vector3.up * 1.5f, lookTarget.position);
        Gizmos.DrawWireSphere(lookTarget.position, 0.1f);
    }
#endif
}