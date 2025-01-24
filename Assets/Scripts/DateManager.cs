using Convai.Scripts.Runtime.UI;
using Meta.WitAi.Dictation;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    [SerializeField] private DictationService _dictation;
    [SerializeField] private ConvaiChatUIHandler convai;
    [SerializeField] string testPrompt = "Hello, how are you?";

    // Start is called before the first frame update
    void Start()
    {
        _dictation.Activate();
    }

    [Button]
    private void TestPrompt()
    {
        convai.SendPlayerText(testPrompt);
    }
}
