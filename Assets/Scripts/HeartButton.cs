using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeartButton : MonoBehaviour
{
    public UnityEvent OnButtonSelected;
    [SerializeField] MeshRenderer buttonRenderer;

    private void Start()
    {
        buttonRenderer.material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Arrow arrow))
        {
            ButtonSelected();
            arrow.DestroyArrow();
        }
    }

    private void ButtonSelected()
    {
        OnButtonSelected.Invoke();
        StartCoroutine(SelectedColoring());
    }

    IEnumerator SelectedColoring()
    {
        buttonRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        buttonRenderer.material.color = Color.white;
    }
}
