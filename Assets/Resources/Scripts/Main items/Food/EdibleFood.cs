using System.Collections;
using System.Collections.Generic;
using static FoodSpawner;
using UnityEngine;

public class EdibleFood : Food
{
    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        foodTypeChanged = true;
        if ((int)sort < foodSprites.Length)
        {
            sprite = foodSprites[(int)sort];
        }
        base.Start();
    }

    public virtual void Eaten(GameObject eater)
    {
        if (foodTypeChanged)
        {
            Bacteria eaterProperties = eater.GetComponent<Bacteria>();
            eaterProperties.energy += energyValue;
            eaterProperties.physicHealth += healValue;
            eaterProperties.counter += energyValue;

            if (eaterProperties.energy > eaterProperties.maxEnergy)
            {
                eaterProperties.energy = eaterProperties.maxEnergy;
            }
            if (eaterProperties.physicHealth > eaterProperties.maxPhysicHealth)
            {
                eaterProperties.physicHealth = eaterProperties.maxPhysicHealth;
            }

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (foodTypeChanged)
        {
            if (spawned)
            {
                curSpawnedFood--;
            }
            curAllFood--;
        }
    }
}
