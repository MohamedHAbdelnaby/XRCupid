using Convai.Scripts.Runtime.Core;
using Convai.Scripts.Runtime.UI;
using Meta.WitAi.Dictation;
using NaughtyAttributes;
using Oculus.Voice.Dictation;
using ReadyPlayerMe.Core;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    [SerializeField] private DictationService _dictation;
    [SerializeField] private ConvaiChatUIHandler convai;
    [SerializeField] string testPrompt = "Hello, how are you?";
    [SerializeField] ConvaiInputManager convaiInputManager;
    public ConvaiNPC currentAvatar;
    [SerializeField] string avatarCanvasName = "Convai Transcript Canvas - XR Chat(Clone)";
    private CanvasGroup avatarCanvas;
    [SerializeField] SkinnedMeshRenderer graceRenderer;
    [SerializeField] AppDictationExperience dictation;
    [SerializeField] Animator avatarAnimator;
    [SerializeField] private bool avatarWasSpeaking;
    [SerializeField] ConvaiChatUIHandler chatHandler;
    bool firstTranscriptionHappened;
    bool firstTalkHappened;
    [SerializeField] GameObject micIcon;

    [TextArea(3, 10)]
    public string currentTranscription = "";

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(GetCanvasDelayed), 0.2f);
        dictation.AudioEvents.OnMicStoppedListening.AddListener(SendTranscriptionToChat);
    }

    private void SendTranscriptionToChat()
    {
        if(currentTranscription != "" && firstTranscriptionHappened)
        {
            string originalString = currentTranscription;
            string stringToRemove = "Hey Facebook";

            // Remove the specific sentence from the original string
            string modifiedString = originalString.Replace(stringToRemove, "");

            print("sending text to convai avatar");
            SendMsg(modifiedString);
            currentTranscription = "";
        }
        else if(currentTranscription == "" && firstTranscriptionHappened)
        {
            dictation.Activate();
        }
        firstTranscriptionHappened = true;
    }

    [Button]
    private void RestartVoice()
    {
        currentTranscription = "";
        dictation.Activate();
        Debug.Log("Voice restarted");
    }

    private void GetCanvasDelayed()
    {
        avatarCanvas = GameObject.Find(avatarCanvasName).GetComponent<CanvasGroup>();
        avatarCanvas.alpha = 0f;
    }

    private void Update()
    {
        if(avatarCanvas != null)
        {
            avatarCanvas.alpha = graceRenderer.enabled? 1f : 0f;
        }

        bool isAvatarSpeaking = avatarAnimator.GetBool("Talk");
        if (!firstTalkHappened)
        {
            if (isAvatarSpeaking)
            {
                firstTalkHappened = true;
            }
        }
        micIcon.SetActive(!isAvatarSpeaking && graceRenderer.enabled == true && firstTalkHappened);

        if (!isAvatarSpeaking)
        {
            if (avatarWasSpeaking)
            {
                avatarWasSpeaking = false;
                RestartVoice();
            }
        }
        else
        {
            avatarWasSpeaking = true;
        }
    }

    public void ReceiveTranscription(string text)
    {
        currentTranscription += text + " ";
    }

    [Button]
    private void TestPrompt()
    {
        convai.SendPlayerText(testPrompt);
        convaiInputManager.sendText?.Invoke();
        currentAvatar.GetComponent<ConvaiPlayerInteractionManager>().HandleInputSubmission(testPrompt);
    }
    [Button]
    private void TestPromptHidden()
    {
        currentAvatar.GetComponent<ConvaiPlayerInteractionManager>().HandleInputSubmissionHidden(testPrompt);
    }

    public void SendMsg(string message)
    {
        //convaiInputManager.sendText?.Invoke();
        currentAvatar.GetComponent<ConvaiPlayerInteractionManager>().HandleInputSubmission(message);
    }
    public void SendMsgHidden(string message)
    {
        //convaiInputManager.sendText?.Invoke();
        currentAvatar.GetComponent<ConvaiPlayerInteractionManager>().HandleInputSubmissionHidden(message);
    }
    public void OnTranscriptionUpdated(string transcription)
    {
        SendMsg(transcription);
    }
}
