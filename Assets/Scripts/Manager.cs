using Convai.Scripts.Runtime.PlayerStats;
using DG.Tweening;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
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

    //Grace
    [SerializeField] GameObject grace;
    [SerializeField] SkinnedMeshRenderer[] hands;
    [SerializeField] SkinnedMeshRenderer[] graceRenderers;
    [SerializeField] ParticleSystem[] particleSystems;
    [field: SerializeField] public ConvaiPlayerDataSO ConvaiPlayerDataSO { get; private set; }

    //Logo
    [SerializeField] GameObject logoScreen;
    [SerializeField] SpriteRenderer[] logoSprites;
    [SerializeField] TMPro.TextMeshProUGUI logoText;

    //Quiz
    [SerializeField] List<QuizQuestion> quizQuestions;
    private int currentQuiz = 0;
    [SerializeField] Quiz quiz;

    //Keyboard
    [SerializeField] NonNativeKeyboard keyboard;
    [SerializeField] Transform centerEyeAnchor;
    [SerializeField] TMPro.TMP_InputField keyboardInputField;

    private void Awake()
    {
        Instance = this;
        foreach (var item in graceRenderers)
        {
            item.enabled = false;
        }
    }

    private void Start()
    {
        //keyboard.OnTextSubmittedAction += NameEntered;
        //keyboard.OnKeyboardValueKeyPressed += KeyPressed;
    }

    private void KeyPressed(KeyboardValueKey key)
    {
        KeyBoardBackspace();
    }

    public void KeyBoardBackspace()
    {
        if (keyboardInputField.text.Length > 0)
        {
            keyboardInputField.text = keyboardInputField.text.Substring(0, keyboardInputField.text.Length - 1);
        }
    }

    private void OnDestroy()
    {
        //keyboard.OnTextSubmittedAction -= NameEntered;
        //keyboard.OnKeyboardValueKeyPressed -= KeyPressed;
    }

    [Button]
    public void ShowGrace()
    {
        StartCoroutine(Co_ShowGrace());
    }

    private IEnumerator Co_ShowGrace()
    {
        foreach (var item in particleSystems)
        {
            item.Play();
        }
        yield return new WaitForSeconds(1);
        foreach (var item in graceRenderers)
        {
            item.enabled = true;
        }
        grace.GetComponent<CapsuleCollider>().enabled = true;
        dateManager.SendMsgHidden($"Present yourself mentioning my name, {username}, in your salutation, and give me an overview of how are you going to help me with dating advice. Also this is some information about me: {quiz.userInformation}. Please keep your answers around 3 sentences long." );
    }
    [Button]
    public void ShowGraceTest()
    {
        StartCoroutine(Co_ShowGraceTest());
    }

    private IEnumerator Co_ShowGraceTest()
    {
        foreach (var item in particleSystems)
        {
            item.Play();
        }
        yield return new WaitForSeconds(1);
        foreach (var item in graceRenderers)
        {
            item.enabled = true;
        }
        grace.GetComponent<CapsuleCollider>().enabled = true;
        //dateManager.SendMsgHidden($"Present yourself mentioning my name, {username}, in your salutation, and give me an overview of how are you going to help me with dating advice. Also this is some information about me: {quiz.userInformation}");
    }

    public void ShowIntroHands()
    {
        hands[0].material.DOFade(1, 1);
        hands[1].material.DOFade(1, 1);
    }
    public void HideIntroHands()
    {
        float value = 1;
        DOTween.To(() => value, x => value = x, 0, 1)
        .OnUpdate(() =>
        {
            hands[0].material.SetFloat("_Opacity", value);
            hands[1].material.SetFloat("_Opacity", value);
            hands[0].material.SetFloat("_OutlineOpacity", value);
            hands[1].material.SetFloat("_OutlineOpacity", value);
        }).OnComplete(() =>
        {
            hands[0].gameObject.SetActive(false);
            hands[1].gameObject.SetActive(false);
        });
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
        HideIntroHands();

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

    [Button]
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
        ShowKeyboard();
    }

    [Button]
    private void ShowKeyboard()
    {
        //ShowGrace();
        keyboard.transform.parent.gameObject.SetActive(true);
    }
    [Button]
    public void NameEntered()
    {
        username = keyboardInputField.text;
        keyboard.gameObject.SetActive(false);
        ConvaiPlayerDataSO.PlayerName = username;
        ShowGrace();
    }
}

