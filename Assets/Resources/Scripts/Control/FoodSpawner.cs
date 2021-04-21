using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldSettings;
using static Service;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class FoodSpawner : MonoBehaviour
{
    public static int curFood = 0;
    public static float[] spawnPercentage;
    public static float totalValue;
    public GameObject foodPrefab;
    public WorldSettings worldSettings;

    public static List<Type> foodsList = new List<Type>();
    public static List<Type> absorbableFoodsList = new List<Type>();
    public static List<Type> edibleFoodsList = new List<Type>();
    public static Sprite[] foodSprites;
    public static Sprite[] foodSpawnerSprites;

    public enum TypesOfFood
    {
        meat,
        plant,
        crystal,
        mollusk,
        corpse,
        sunlight
    }

    public void GetFoodSubclasses()
    {
        Type[] types = typeof(Food).Assembly.GetTypes();

        foreach (Type type in types)
        {
            if (type.BaseType == typeof(EdibleFood) && !foodsList.Contains(type) && !edibleFoodsList.Contains(type))
            {
                foodsList.Add(type);
                if (type != typeof(Corpse))
                {
                    edibleFoodsList.Add(type);
                }
            }

            if (type.BaseType == typeof(AbsorbableFood) && !foodsList.Contains(type) && !absorbableFoodsList.Contains(type))
            {
                foodsList.Add(type);
                absorbableFoodsList.Add(type);
            }
        }
    }

    void Awake()
    {
        Initialize();

        GetFoodSubclasses();
        int numOfSprites = 0;
        foreach (Object spr in Resources.LoadAll("Textures/Food", typeof(Sprite)))
        {
            numOfSprites++;
        }

        foodSprites = new Sprite[numOfSprites];
        int i = 0;
        foreach (Sprite spr in Resources.LoadAll("Textures/Food", typeof(Sprite)))
        {
            foodSprites[i] = spr;
            i++;
        }

        numOfSprites = 0;
        foreach (Object spr in Resources.LoadAll("Textures/FoodSpawners"))
        {
            numOfSprites++;
        }

        foodSpawnerSprites = new Sprite[numOfSprites];
        i = 0;
        foreach (Sprite spr in Resources.LoadAll("Textures/FoodSpawners", typeof(Sprite)))
        {
            foodSpawnerSprites[i] = spr;
            i++;
        }
    }

    public static void UpdatePercentages()
    {
        totalValue = 0;
        foreach (int i in spawnRatio)
        {
            totalValue += i;
        }

        for (int i = 0; i < spawnRatio.Length; i++)
        {
            spawnPercentage[i] = spawnRatio[i] / totalValue;
        }
    }

    public void SpawnFood()
    {
        if (curFood < maxFood)
        {
            float spawnX = Random.Range(-hBorder, hBorder);
            float spawnY = 0f;
            switch (worldSettings.worldType)
            {
                case WorldSettings.WorldType.square:
                    {
                        spawnY = Random.Range(-vBorder, vBorder);
                        break;
                    }
                case WorldSettings.WorldType.circle:
                    {
                        float secondVBorder = Mathf.Sqrt(Mathf.Pow(vBorder, 2) * (-(Mathf.Pow(spawnX, 2) / Mathf.Pow(hBorder, 2)) + 1));
                        spawnY = Random.Range(-secondVBorder, secondVBorder);
                        break;
                    }
            }

            Vector3 spawnPos = new Vector3(spawnX, spawnY);
            Quaternion spawnRot = Quaternion.Euler(new Vector3(0, 0, Random.Range(-360f, 360f)));
            GameObject spawnedFood = Instantiate(foodPrefab, spawnPos, spawnRot);

            float foodChooser = Random.Range(0f, 1f);
            float previousPercent = 0;

            for (int i = 0; i < spawnPercentage.Length; i++)
            {
                if (foodChooser > spawnPercentage[i] + previousPercent)
                {
                    previousPercent += spawnPercentage[i];
                }
                else
                {
                    spawnedFood.GetComponent<Food>().sort = (TypesOfFood)i;
                    previousPercent = 0;
                    break;
                }
            }

            curFood++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPercentage = new float[spawnRatio.Length];
        UpdatePercentages();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        
    }
}
