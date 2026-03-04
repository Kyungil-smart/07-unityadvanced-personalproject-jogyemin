using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StoryPlayer : MonoBehaviour
{
    [Header("===== UI 참조 =====")]
    public TextMeshProUGUI storyText;
    public Button prevButton;
    public Button nextButton;

    [Header("===== 이동할 씬 =====")]
    public string gameSceneName = "Game";

    private int currentIndex = 0;

    private string[] stories =
    {
// 1
@"비가 그친 직후였다. 질척이는 진흙길을 걷던 중, 

짙은 안개 너머로 낡은 성문이 모습을 드러냈다. 

지도조차 존재하지 않는 버려진 폐성. 

그저 남은 비를 피할 요량으로 안으로 발을 들였을 뿐이었다. 

하지만 문지방을 넘는 찰나, 등 뒤에서 육중한 마찰음이 울리며 출구가 닫혀버렸다. 

황급히 몸을 돌렸을 때,방금 전까지 열려 있던 문은 흔적조차 남지 않은 차가운 돌벽으로 변해 있었다.",

// 2
@"내부는 끝을 알 수 없이 반복되는 복도였다. 

금이 간 돌벽과 붉게 녹슨 사슬, 그리고 일정한 간격으로 타오르는 횃불들. 

처음엔 그저 기괴한 폐허일 뿐이라 생각했다. 

그러나 다시금 같은 길을 지나는 순간, 기묘한 위화감이 목덜미를 훑었다. 

분명 방금까지 타오르던 횃불 하나가 통째로 사라져 있었고, 

벽에 걸린 낡은 초상화 속 눈동자는 미묘하게 다른 곳을 보고 있었다. 

같은 공간임이 분명한데도 기억의 아귀가 맞지 않는다. 

이 던전은 아주 섬세하고 악랄한 차이로 나를 시험하고 있었다.",

// 3
@"복도의 끝에는 두 개의 문이 입을 벌리고 있었다. 

하나는 다시 궤적을 되돌리는 듯한 문, 다른 하나는 더 깊은 심연으로 이끄는 문. 

이상이 있다면 돌아가고, 이상이 없다면 나아가야 한다는 직감이 머릿속을 맴돌았다. 

하지만 확신은 모래처럼 빠져나갔다. 문을 주시할수록 심박은 거칠어졌고, 

무거운 정적 사이로 미세한 숨소리가 섞여 들었다. 분명, 내 것이 아니었다.",

// 4
@"바로 그때였다. 복도 끝 짙은 어둠 속에서 기이한 형체가 꿈틀거리며 기어 나왔다. 

얼굴이 있어야 할 자리에 괴이한 책을 뒤집어쓴 괴물이 

기괴하게 뒤틀린 팔다리로 바닥을 긁고 있었다. 

반대편 벽에서는 바짝 말라비틀어진 오크가 마치 그림자처럼 소리 없이 기어 다녔다. 

놈들은 아무런 소리도 내지 않았지만, 명백한 살의로 나를 인식하고 있었다. 

무심코 그들과 시선이 얽힌 순간,복도의 횃불이 일제히 꺼지며 던전은 완벽한 암흑 속으로 가라앉았다.",

// 5
@"나는 숨을 죽인 채 차가운 돌벽에 몸을 밀착시켰다. 

하지만 놈들에게 빛은 필요하지 않은 듯했다. 

기어오는 소리가 가까워질수록 바닥을 타고 오르는 미세한 진동이 온몸을 떨게 했다. 

이곳은 단순한 기억력의 시험장이 아니었다. 섣부른 선택, 

혹은 너무 오랜 응시는 곧 그들을 부르는 참혹한 신호였다.",

// 6
@"가까스로 다시 두 개의 문 앞에 섰다. 

하나는 안전을 덮어쓴 함정일 것이고, 다른 하나는 탈출로 위장한 절망일 터였다. 

여덟 번을 무사히 넘기면 끝난다는 낯선 속삭임이 이명처럼 스쳐 지나갔다. 

단 한 번이라도 실패한다면 처음의 지옥으로 돌아가거나, 영원히 저들의 먹잇감이 될 것이다.",

// 7
@"나는 과연 이 악몽 같은 던전에서 벗어날 수 있을까. 

아니면 끝없이 반복되는 이 미로의 또 다른 그림자로 전락할 것인가. 

떨리는 손을 뻗어 차가운 문고리를 움켜쥐었다. 

바로 그 순간, 칠흑 같은 어둠 속에서 거대한 무언가가 나를 향해 덮쳐왔다. . . . ."
    };

    void Start()
    {
        ShowStory();
    }

    public void OnClickNext()
    {
        if (currentIndex < stories.Length - 1)
        {
            currentIndex++;
            ShowStory();
        }
        else
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public void OnClickPrev()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowStory();
        }
    }

    void ShowStory()
    {
        storyText.text = stories[currentIndex];

        prevButton.interactable = currentIndex > 0;

        if (currentIndex == stories.Length - 1)
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "게임 시작";
        else
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "다음";
    }
}