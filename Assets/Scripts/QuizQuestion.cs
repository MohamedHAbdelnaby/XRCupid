using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuizQuestion : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private List<CanvasGroup> canvases;

    private void OnValidate()
    {
        if(spriteRenderers.Count == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        }
        if (canvases.Count == 0)
        {
            canvases = GetComponentsInChildren<CanvasGroup>().ToList();
        }
    }

    private void Awake()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.gameObject.SetActive(false);
            spriteRenderer.DOFade(0, 0.01f);
        }
        foreach (var canvases in canvases)
        {
            canvases.gameObject.SetActive(false);
            canvases.alpha = 0;
        }
    }

    [Button]
    public void ShowQuestion()
    {
        foreach(var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.DOFade(1, 1);
        }
        foreach(var canvases in canvases)
        {
            canvases.gameObject.SetActive(true);
            canvases.DOFade(1, 1);
        }
    }
    [Button]
    public void HideQuestion()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.DOFade(0, 1).OnComplete(() =>
            {
                spriteRenderer.gameObject.SetActive(false);
            });
        }
        foreach (var canvases in canvases)
        {
            canvases.DOFade(0, 1).OnComplete(() =>
            {
                canvases.gameObject.SetActive(false);
            });
        }
    }
}
