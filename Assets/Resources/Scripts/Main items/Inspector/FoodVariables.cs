using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodVariables : VisibleVariables
{
    public Food linkedFood;

    [SerializeField]
    public float EnergyValue
    {
        get => linkedFood.energyValue;
        set
        {
            linkedFood.energyValue = value;
        }
    }

    [SerializeField]
    public float HealValue
    {
        get => linkedFood.healValue;
        set
        {
            linkedFood.healValue = value;
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        linkedFood = gameObject.GetComponent<Food>();
    }
}
