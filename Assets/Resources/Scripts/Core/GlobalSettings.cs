using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FunctionsController;
using static Service;

public class GlobalSettings : MonoBehaviour
{
    // Start is called before the first frame update
    public static float refreshTime = 2;
    public static float longRefreshTime = 5;

    public GameObject globalSettingsPanel;



    [SerializeField]
    public float ShortRateTimer
    {
        get => refreshTime;
        set
        {
            refreshTime = value;
        }
    }

    [SerializeField]
    public float LongRateTimer
    {
        get => longRefreshTime;
        set
        {
            longRefreshTime = value;
        }
    }

    [SerializeField]
    public int ChoosedTranslationID
    {
        get => TranslationID;
        set
        {
            Debug.Log("Translation changing pt.1");
            TranslationID = value;
        }
    }

    private void Awake()
    {
        UpdatePanel();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePanel()
    {
        foreach (Transform toTrash in globalSettingsPanel.transform)
        {
            Destroy(toTrash.gameObject);
        }

        GetVariables(gameObject, globalSettingsPanel);
    }
}
