using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoodSpawner;
using static WorldSettings;

public class Mollusk : EdibleFood
{
    public float splitCooldown;

    public override void Awake()
    {
        base.Awake();
        foodColor = new Color(0.84f, 0.74f, 0.61f);
        splitCooldown = Random.Range(0.75f, 1.5f) * molluskReproductionTime;
        InvokeRepeating("Split", splitCooldown, splitCooldown);
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public void Split()
    {
        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0f);
        if (curAllFood < maxAllFood && Physics2D.OverlapCircle(spawnPos, 0.25f) == null)
        {
            Quaternion spawnRot = Quaternion.Euler(new Vector3(0, 0, Random.Range(-360f, 360f)));
            Instantiate(gameObject, spawnPos, spawnRot).GetComponent<Food>().spawned = false;
            curAllFood++;
        }
    }
}
