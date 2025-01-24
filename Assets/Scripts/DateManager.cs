using Meta.WitAi.Dictation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateManager : MonoBehaviour
{
    [SerializeField] private DictationService _dictation;

    // Start is called before the first frame update
    void Start()
    {
        _dictation.Activate();
    }
}
