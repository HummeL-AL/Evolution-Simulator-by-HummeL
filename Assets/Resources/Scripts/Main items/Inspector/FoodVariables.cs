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
    // Start is called before the first frame update
    private void Awake()
    {
        linkedFood = gameObject.GetComponent<Food>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
