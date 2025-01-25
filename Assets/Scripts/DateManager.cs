using Convai.Scripts.Runtime.Core;
using Convai.Scripts.Runtime.UI;
using Meta.WitAi.Dictation;
using NaughtyAttributes;
using ReadyPlayerMe.Core;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    [SerializeField] private DictationService _dictation;
    [SerializeField] private ConvaiChatUIHandler convai;
    [SerializeField] string testPrompt = "Hello, how are you?";
    [SerializeField] ConvaiInputManager convaiInputManager;
    public AvatarData currentAvatar;

    // Start is called before the first frame update
    void Start()
    {
        _dictation.Activate();
    }

    [Button]
    private void TestPrompt()
    {
        convai.SendPlayerText(testPrompt);
        convaiInputManager.sendText?.Invoke();
        currentAvatar.GetComponent<ConvaiPlayerInteractionManager>().HandleInputSubmission(testPrompt);
    }
}
