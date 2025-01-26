using Convai.Scripts.Runtime.PlayerStats;
using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Manager : MonoBehaviour
{
    [SerializeField] DateManager dateManager;
    public static Manager Instance { get; private set; }

    [SerializeField] string username = "Lucas";
    [SerializeField] SkinnedMeshRenderer[] hands;
    [SerializeField] SkinnedMeshRenderer[] graceRenderers;
    [field: SerializeField] public ConvaiPlayerDataSO ConvaiPlayerDataSO { get; private set; }

    //Logo
    [SerializeField] GameObject logoScreen;
    [SerializeField] SpriteRenderer[] logoSprites;
    [SerializeField] TMPro.TextMeshProUGUI logoText;

    //Quiz
    [SerializeField] List<QuizQuestion> quizQuestions;
    private int currentQuiz = 0;

    private void Awake()
    {
        Instance = this;
        foreach (var item in graceRenderers)
        {
            item.enabled = false;
        }
    }

    [Button]
    public void ShowGrace()
    {
        foreach (var item in graceRenderers)
        {
            item.enabled = true;
        }
        dateManager.SendMsgHidden($"Present yourself mentioning my name, {username}, in your salutation, and give me an overview of how are you going to help me with dating advice");
    }

    public void ShowIntroHands()
    {
        hands[0].material.DOFade(1, 1);
        hands[1].material.DOFade(1, 1);
    }
    public void HideIntroHands()
    {
        hands[0].material.DOFade(0, 1);
        hands[1].material.DOFade(0, 1);
    }

    public void ShowLogo()
    {
        foreach (var item in logoSprites)
        {
            item.DOFade(1, 3);
        }
        logoText.DOFade(1, 3);
    }

    public void LogoHit()
    {
        //logoScreen.transform.DOMoveY(2, 2).SetEase(Ease.InElastic);
        logoSprites[0].DOFade(0, 2);
        logoSprites[1].DOFade(0, 4).OnComplete(() =>
        {
            logoScreen.SetActive(false);
        });

        float scale = logoSprites[1].transform.localScale.x;

        logoSprites[1].transform.DOScale(scale * 1.2f, 0.5f).OnComplete(() =>
        {
            logoSprites[1].transform.DOScale(0.001f, 1).OnComplete(() =>
            {
                StartCoroutine(StartQuiz());
            });
        });

        logoText.DOFade(0, 2);
    }

    [Button]
    private void TestQuiz()
    {
        StartCoroutine(StartQuiz());
    }

    public IEnumerator StartQuiz()
    {
        yield return new WaitForSeconds(2);
        quizQuestions[currentQuiz].ShowQuestion();
        yield return new WaitForSeconds(3);
        NextQuiz();
    }

    public void NextQuiz()
    {
        StartCoroutine(Co_NextQuiz());
    }

    private IEnumerator Co_NextQuiz()
    {
        quizQuestions[currentQuiz].HideQuestion();
        yield return new WaitForSeconds(2);
        currentQuiz++;
        if(currentQuiz >= quizQuestions.Count)
        {
            FinishQuiz();
        }
        else
        {
            quizQuestions[currentQuiz].ShowQuestion();
        }
    }

    private void FinishQuiz()
    {
        Debug.Log("Finished quiz");
    }
}

