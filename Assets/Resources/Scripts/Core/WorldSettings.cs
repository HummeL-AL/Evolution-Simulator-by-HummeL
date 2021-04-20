using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoodSpawner;
using static GlobalSettings;
using static FunctionsController;
using static EnergyMapGenerator;

public class WorldSettings : MonoBehaviour
{
    public static float hBorder = 150;
    public static float vBorder = 150;
    public static float maxFood = 600;

    public static int[] spawnRatio;
    public static float baseEnergy = 10;
    public static int[] energyCoefficients;
    public static float baseHeal = 10;
    public static int[] healCoefficients;

    public static float scale = 25;
    public static bool lightMapMoving = false;

    public static bool initializingEnded = false;

    public GameObject worldSettingsPanel;
    public EnergyMapGenerator mapGenerator;

    public enum WorldType { square, circle };

    [SerializeField]
    public float HBorder
    {
        get => hBorder;
        set
        {
            hBorder = value;
            noiseTexture.Resize((int)hBorder * 2, (int)vBorder * 2);
            mapGenerator.visibleLight.transform.localScale = new Vector3(HBorder * 0.85f, VBorder * 0.85f, 1f);
            GenerateMap();
        }
    }

    [SerializeField]
    public float VBorder
    {
        get => vBorder;
        set
        {
            vBorder = value;
            noiseTexture.Resize((int)hBorder * 2, (int)vBorder * 2);
            mapGenerator.visibleLight.transform.localScale = new Vector3(HBorder * 0.85f, VBorder * 0.85f, 1f);
            GenerateMap();
        }
    }

    [SerializeField]
    public float MaxFood
    {
        get => maxFood;
        set
        {
            maxFood = value;
        }
    }

    [SerializeField]
    public int[] SpawnRatio
    {
        get => spawnRatio;
        set
        {
            spawnRatio = value;
            if (initializingEnded)
            {
                UpdatePercentages();
            }
        }
    }

    [SerializeField]
    public float BaseEnergy
    {
        get => baseEnergy;
        set
        {
            baseEnergy = value;
        }
    }

    [SerializeField]
    public int[] EnergyCoefficients
    {
        get => energyCoefficients;
        set
        {
            energyCoefficients = value;
        }
    }

    [SerializeField]
    public float BaseHeal
    {
        get => baseHeal;
        set
        {
            baseHeal = value;
        }
    }

    [SerializeField]
    public int[] HealCoefficients
    {
        get => healCoefficients;
        set
        {
            healCoefficients = value;
        }
    }

    [SerializeField]
    public float LightMapScale
    {
        get => scale;
        set
        {
            scale = value;
            GenerateMap();
        }
    }

    [SerializeField]
    public bool LightMapMoving
    {
        get => lightMapMoving;
        set
        {
            lightMapMoving = value;
            if(lightMapMoving)
            {
                mapGenerator.InvokeRepeating("UpdateOffset", 0f, longRefreshTime);
            }
            else
            {
                mapGenerator.CancelInvoke("UpdateOffset");
            }
        }
    }

    [SerializeField]
    public WorldType worldType;

    private void Awake()
    {
        SpawnRatio = new int[edibleFoodsList.Count];
        for (int i = 0; i < SpawnRatio.Length; i++)
        {
            SpawnRatio[i] = 1;
        }

        EnergyCoefficients = new int[foodsList.Count];
        for (int i = 0; i < EnergyCoefficients.Length; i++)
        {
            EnergyCoefficients[i] = 1;
        }

        HealCoefficients = new int[foodsList.Count];
        for (int i = 0; i < HealCoefficients.Length; i++)
        {
            HealCoefficients[i] = 1;
        }

        UpdatePanel();
    }

    // Start is called before the first frame update
    void Start()
    {
        initializingEnded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePanel()
    {
        foreach (Transform toTrash in worldSettingsPanel.transform)
        {
            Destroy(toTrash.gameObject);
        }

        GetVariables(gameObject, worldSettingsPanel);
    }
}
