using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    [TextArea(3, 10)]
    public string userInformation;

    public void ButtonPressedConversation(string text)
    {
        userInformation += "Conversation style: " + text + "\n";
    }
    public void ButtonPressedIdealDate(string text)
    {
        userInformation += "Ideal date: " + text + "\n";
    }
    public void ButtonPressedUserAge(string text)
    {
        userInformation += "Age is: " + text + "\n";
    }
}
