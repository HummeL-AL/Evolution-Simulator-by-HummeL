using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslatedText : Text
{
    public string originalText;

    [SerializeField]
    public string OriginalText
    {
        get => originalText;
        set
        {
            originalText = value;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }
}
