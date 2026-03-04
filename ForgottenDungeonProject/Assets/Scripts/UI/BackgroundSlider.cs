using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundSlider : MonoBehaviour
{
    [Header("===== 배경 이미지 3개 =====")]
    public Image background1;
    public Image background2;
    public Image background3;

    [Header("===== 설정 =====")]
    [Tooltip("이미지 유지 시간")]
    public float displayTime = 4f;

    [Tooltip("페이드 시간")]
    public float fadeDuration = 3f;

    private Image[] backgrounds;
    private int currentIndex = 0;

    void Start()
    {
        backgrounds = new Image[3];
        backgrounds[0] = background1;
        backgrounds[1] = background2;
        backgrounds[2] = background3;

        // 모든 이미지 알파 초기화
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].color = new Color(1, 1, 1, 0);
        }

        // 첫 이미지 보이기
        backgrounds[0].color = new Color(1, 1, 1, 1);

        StartCoroutine(SlideRoutine());
    }

    IEnumerator SlideRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(displayTime);

            int nextIndex = (currentIndex + 1) % backgrounds.Length;

            yield return StartCoroutine(Fade(backgrounds[currentIndex], backgrounds[nextIndex]));

            currentIndex = nextIndex;
        }
    }

    IEnumerator Fade(Image from, Image to)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            from.color = new Color(1, 1, 1, 1 - t);
            to.color = new Color(1, 1, 1, t);

            yield return null;
        }

        from.color = new Color(1, 1, 1, 0);
        to.color = new Color(1, 1, 1, 1);
    }
}