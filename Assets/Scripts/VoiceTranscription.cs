using Oculus.Voice;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class VoiceTranscription : MonoBehaviour
{
    public enum TranscriptionMode { Wit, Whisper }
    [SerializeField] TranscriptionMode transcriptionMode = TranscriptionMode.Whisper;
    public bool SendRequests = false;
    [SerializeField] private AppVoiceExperience appVoiceExp;
    [SerializeField] private string[] filterOutWords;
    [SerializeField] private int minimumCharacterLenght = 5;
    [SerializeField] private int maximumCharacterLenght = 50;
    [SerializeField] private int currentCharacters = 0;
    [SerializeField] private float maxTimeSilence = 5;
    [SerializeField] private float counter;
    [SerializeField] private TMPro.TextMeshProUGUI textCanvas;
    [TextArea(2, 5)]
    public string currentPrompt;
    private bool spawned;

    public event Action<string> OnPromptCreated;
    public event Action<string> OnWordTranscribed;

    public void Start()
    {
        spawned = true;

        if (transcriptionMode == TranscriptionMode.Wit)
        {
            appVoiceExp.TranscriptionEvents.OnFullTranscription.AddListener(FullTranscription);
            appVoiceExp.TranscriptionEvents.OnPartialTranscription.AddListener(PartialTranscription);
            appVoiceExp.VoiceEvents.OnMicAudioLevelChanged.AddListener((f) => { if (f > 0.02) counter = 0; });
            appVoiceExp.Activate();
        }
        else if (transcriptionMode == TranscriptionMode.Whisper)
        {
            //whisperApi.OnFullTranscription += FullTranscription;
            //whisperApi.StartTranscribing();
        }
        UpdateText();
    }

    private void Update()
    {
        if (!SendRequests) return;

        counter += Time.deltaTime;
        currentCharacters = currentPrompt.Length;
        if (counter >= maxTimeSilence && currentPrompt.Length >= minimumCharacterLenght || currentPrompt.Length >= maximumCharacterLenght)
        {
            counter = 0;
            Request();
        }

        if (transcriptionMode == TranscriptionMode.Wit)
        {
            if (!appVoiceExp.Active)
            {
                appVoiceExp.ActivateImmediately();
            }
        }
    }

    private void Request()
    {
        if (currentPrompt != string.Empty)
        {
            OnPromptCreated?.Invoke(currentPrompt);
        }
        currentPrompt = "";
        UpdateTextVisual(currentPrompt);
    }

    public void FullTranscription(string transcription)
    {
        //Separate string in multiple parts if bigger than 512 bytes
        Encoding encoding = Encoding.UTF8;
        if (encoding.GetByteCount(transcription) > 512)
        {
            var list = SplitString(transcription);
            foreach (var item in list)
            {
                AddMessage(item);
            }
        }
        else
        {
            //Don't send message if the transcript if just any of the filter out words
            if (filterOutWords.Length > 0)
            {
                if (filterOutWords.Any(word => word == transcription))
                {
                    return;
                }
            }
            //Invoke event
            OnWordTranscribed?.Invoke(transcription);
            AddMessage(transcription);
        }
    }

    public void AddMessage(string message)
    {
        currentPrompt += $"Person : {message}\n";
        UpdateText();
    }

    public void UpdateTextVisual(string message)
    {
        currentPrompt = message;
        UpdateText();
    }

    public void PartialTranscription(string transcription)
    {
    }

    private void PromptCreated(string prompt)
    {
        OnPromptCreated?.Invoke(prompt);
    }

    private void UpdateText()
    {
        textCanvas.text = currentPrompt;
    }

    // Function to split a string into parts, each less than 512 bytes
    public static List<string> SplitString(string input, int maxBytes = 512)
    {
        List<string> parts = new List<string>();
        StringBuilder currentPart = new StringBuilder();
        int currentBytes = 0;
        Encoding encoding = Encoding.UTF8;

        foreach (char c in input)
        {
            // Get the byte size of the current character
            int charSize = encoding.GetByteCount(new char[] { c });

            // If adding this character exceeds the byte limit, start a new part
            if (currentBytes + charSize > maxBytes)
            {
                parts.Add(currentPart.ToString()); // Add the current part to the list
                currentPart.Clear(); // Reset the StringBuilder
                currentBytes = 0; // Reset the byte counter
            }

            currentPart.Append(c);
            currentBytes += charSize;
        }

        // Add any remaining characters to the last part
        if (currentPart.Length > 0)
        {
            parts.Add(currentPart.ToString());
        }

        return parts;
    }

    private void OnDestroy()
    {
        if (transcriptionMode == TranscriptionMode.Wit)
        {
            appVoiceExp.TranscriptionEvents.OnFullTranscription.RemoveListener(FullTranscription);
            appVoiceExp.TranscriptionEvents.OnPartialTranscription.RemoveListener(PartialTranscription);
            appVoiceExp.VoiceEvents.OnMicAudioLevelChanged.RemoveListener((f) => { if (f > 0.02) counter = 0; });
        }
        else if (transcriptionMode == TranscriptionMode.Whisper)
        {
            //whisperApi.OnFullTranscription -= FullTranscription;
        }
    }
}