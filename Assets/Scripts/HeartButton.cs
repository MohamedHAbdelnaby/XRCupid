using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class ButtonEvent : UnityEvent<string> { }
public class HeartButton : MonoBehaviour
{
    public ButtonEvent OnButtonSelected;
    [SerializeField] MeshRenderer buttonRenderer;
    bool buttonAlreadyPressed;

    private void Start()
    {
        if(buttonRenderer != null)
        {
            buttonRenderer.material.color = Color.white;
        }
    }

    [Button]
    public void TestButton()
    {
        string text = string.Empty;
        if (GetComponentInChildren<TMPro.TextMeshProUGUI>())
        {
            text = GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        }
        OnButtonSelected.Invoke(text);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Arrow arrow))
        {
            arrow.DestroyArrow();
            if (buttonAlreadyPressed) return;
            ButtonSelected();
        }
    }

    private void ButtonSelected()
    {
        buttonAlreadyPressed = true;
        string text = string.Empty;
        if (GetComponentInChildren<TMPro.TextMeshProUGUI>())
        {
            text = GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        }
        OnButtonSelected.Invoke(text);
        if (buttonRenderer != null)
        {
            StartCoroutine(SelectedColoring());
        }
    }

    IEnumerator SelectedColoring()
    {
        buttonRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        buttonRenderer.material.color = Color.white;
    }
}
