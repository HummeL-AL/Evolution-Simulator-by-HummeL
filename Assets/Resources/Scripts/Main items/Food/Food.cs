using System.Collections;
using System.Collections.Generic;
using static WorldSettings;
using static FoodSpawner;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float energyValue;
    public float healValue;
    public Color foodColor;

    public TypesOfFood sort;
    public Sprite sprite;

    public SpriteRenderer spr;

    public bool foodTypeChanged = false;
    public bool spawned = false;
    // Start is called before the first frame update
    public virtual void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    public virtual void Start()
    {
        if (!foodTypeChanged)
        {
            switch (sort)
            {
                case TypesOfFood.meat:
                    {
                        SetVariables((Food)gameObject.AddComponent(typeof(Meat)));
                        break;
                    }
                case TypesOfFood.plant:
                    {
                        SetVariables((Food)gameObject.AddComponent(typeof(Plant)));
                        break;
                    }
                case TypesOfFood.crystal:
                    {
                        SetVariables((Food)gameObject.AddComponent(typeof(Mineral)));
                        break;
                    }
                case TypesOfFood.mollusk:
                    {
                        SetVariables((Food)gameObject.AddComponent(typeof(Mollusk)));
                        break;
                    }
                case TypesOfFood.corpse:
                    {
                        Food addedFood = (Food)gameObject.AddComponent(typeof(Corpse));
                        addedFood.sort = sort;
                        addedFood.energyValue = energyValue;
                        addedFood.healValue = healValue;
                        addedFood.foodColor = foodColor;
                        addedFood.sprite = sprite;
                        break;
                    }
                case TypesOfFood.sunlight:
                    {
                        SetVariables((Food)gameObject.AddComponent(typeof(Sunlight)));
                        break;
                    }
            }
            Destroy(this);
        }
        else
        {
            RelinkVariablesScript();
            SetTexture();
        }
    }

    public void SetVariables(Food food)
    {
        food.sort = sort;
        food.energyValue = baseEnergy * energyCoefficients[(int)food.sort];
        food.healValue = baseHeal * healCoefficients[(int)food.sort];
    }

    public virtual void RelinkVariablesScript()
    {
        GetComponent<FoodVariables>().linkedFood = this;
    }

    public virtual void SetTexture()
    {
        spr.color = foodColor;
        spr.sprite = sprite;
    }
}
