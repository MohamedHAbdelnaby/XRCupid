using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] DateManager dateManager;
    public static Manager Instance { get; private set; }

    [SerializeField] string username = "Lucas";
    [SerializeField] SkinnedMeshRenderer[] hands;
    [SerializeField] SkinnedMeshRenderer[] graceRenderers;

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
}

