using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

using static FoodSpawner;
using static WorldSettings;
using static GlobalSettings;
using static EnergyMapGenerator;

public class Bacteria : MonoBehaviour
{
    [Header("Physic features")]
    public float physicHealth;
    public float maxPhysicHealth;
    public float attackStrength;
    public float attackCooldown;
    public float attackRange;
    public float minEnergyForAttack;
    public float attackEnergy;
    [HideInInspector]
    public float attackTimer;

    [Header("Features")]
    public float foodRadius;
    public float enemyRadius;
    public float friendlyRadius;
    public float enemyTolerance;
    public float speedOfRotation;
    public float speedOfMove;
    public float closeDistance;
    public float comfortableDistance;
    public float farDistance;
    public float energyFromSun;
    public float energy;
    public float maxEnergy;
    public float energyForSeparation;
    public float separationValue;

    [Header("Mutation Features")]
    public float chanceOfMutation;
    public float mutationRangeSmall;
    public float mutationRangeMedium;
    public float mutationRangeBig;

    [Header("Arrays")]
    public int[] priorities = new int[9];
    public bool[] stagesChecker = new bool[9];
    public List<Type> edibleFood = new List<Type>();
    public IDictionary<int, int[]> conditionReactions = new Dictionary<int, int[]>();
    public IDictionary<int, GameObject> targets = new Dictionary<int, GameObject>();
    public IDictionary<int, reaction> reactions = new Dictionary<int, reaction>();

    [Header("Targets")]
    public Food closestFood;
    public Bacteria closestEnemy;
    public Bacteria closestFriendly;

    [Header("Service")]
    public int generation;
    [HideInInspector]
    public GameObject target = null;
    [HideInInspector]
    public Vector3 targetPos = Vector3.zero;
    [HideInInspector]
    public Quaternion targetRot;
    [HideInInspector]
    public bool atTravel;

    public float counter = 0;
    public float countTime = 0;
    public float countRange = 0;
    public float energyPerSecond = 0;

    public enum reaction { none, attack, goThere, stayAtDistance }; 

    void Awake()
    {
        atTravel = false;
        comfortableDistance = (closeDistance + farDistance) * 0.5f;
        attackEnergy = ((attackStrength * attackRange * attackRange) / (attackCooldown * attackCooldown)) * 0.4f;

        SetConditionReactions();
        SetReactions();
        SetTargets();

        InvokeRepeating("DoTimerTick", 0f, refreshTime);
    }

    public void SetConditionReactions()
    {
        //enemies 0 = close, 1 = middle, 2 = far
        conditionReactions[0] = new int[] { 1, 2 };
        conditionReactions[1] = new int[] { 3, 2 };
        conditionReactions[2] = new int[] { 2, 0 };

        //friendlies 0 = close, 1 = middle, 2 = far
        conditionReactions[3] = new int[] { 3, 1 };
        conditionReactions[4] = new int[] { 2, 0 };
        conditionReactions[5] = new int[] { 3, 1 };

        //food 0 = close, 1 = middle, 2 = far
        conditionReactions[6] = new int[] { 2, 0 };
        conditionReactions[7] = new int[] { 2, 0 };
        conditionReactions[8] = new int[] { 2, 0 };
    }

    public void SetReactions()
    {
        reactions[0] = reaction.none;
        reactions[1] = reaction.attack;
        reactions[2] = reaction.goThere;
        reactions[3] = reaction.stayAtDistance;
    }

    public void SetTargets()
    {
        if (closestFood)
        {
            targets[0] = closestFood.gameObject;
        }
        else
        {
            targets[0] = null;
        }

        if (closestFriendly)
        {
            targets[1] = closestFriendly.gameObject;
        }
        else
        {
            targets[1] = null;
        }

        if (closestEnemy)
        {
            targets[2] = closestEnemy.gameObject;
        }
        else
        {
            targets[2] = null;
        }
    }

    void Start()
    {
        if (generation == 0)
        {
            edibleFood.Add(foodsList[Random.Range(0, foodsList.Count)]);
        }
        foreach (Type food in edibleFood)
        {
            Debug.Log(food);
        }
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        CheckReactionsConditions();
        CalculateEnergyLosts();

        if (edibleFood.Contains(typeof(Sunlight)))
        {
            Photosynthesis();
        }
        DrawLines();

        CheckDeathConditions();
    }

    private void FixedUpdate()
    {
        countTime += Time.fixedDeltaTime;
        CalculateEPS();
    }

    public void CalculateEPS()
    {
        energyPerSecond = counter / countTime;

        if(countTime >= countRange)
        {
            counter = 0;
            countTime = 0;
        }
    }

    public void DrawLines()
    {
        if (closestFood)
        {
            Debug.DrawLine(transform.position, closestFood.transform.position, Color.blue);
        }
        if (closestFriendly)
        {
            Debug.DrawLine(transform.position, closestFriendly.transform.position, Color.green);
        }
        if (closestEnemy)
        {
            Debug.DrawLine(transform.position, closestEnemy.transform.position, Color.red);
        }
        if (target)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.yellow);
        }
    }

    public void DoTimerTick()
    {
        UpdateFoods();
        UpdateLives();
        SetTargets();
    }

    public void CalculateEnergyLosts()
    {
        energy -= (Mathf.Pow(speedOfMove, 2f) + Mathf.Pow(speedOfRotation, 2f) + Mathf.Pow((energy + maxEnergy) * 0.1f, 0.5f)) * 0.2f * Time.deltaTime;
    }

    public void Photosynthesis()
    {
        energy -= Mathf.Pow(energyFromSun, 1.5f) * 0.2f * Time.deltaTime;
        energy += energyFromSun * noiseTexture.GetPixel((int)(transform.position.x + hBorder), (int)(transform.position.y + vBorder)).grayscale * Time.deltaTime;
    }

    public bool CheckEdibility(Food foodToCheck)
    {
        foreach (Type listFood in edibleFood)
        {
            if (foodToCheck.GetType() == listFood)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckDeathConditions()
    {
        if (energy <= 0 || physicHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Food corpse = gameObject.AddComponent<Food>();
        corpse.sort = TypesOfFood.corpse;
        corpse.energyValue = energy + maxEnergy * 0.1f;
        corpse.healValue = physicHealth + maxPhysicHealth * 0.1f;
        corpse.foodColor = GetComponent<SpriteRenderer>().color;
        corpse.sprite = GetComponent<SpriteRenderer>().sprite;

        Destroy(gameObject.GetComponent<BacteriaVariables>());
        gameObject.AddComponent<FoodVariables>();
        
        curFood++;
        Destroy(this);
    }

    public void CheckReactionsConditions()
    {
        Array.Clear(stagesChecker, 0, stagesChecker.Length);

        if (!atTravel)
        {
            if (closestFriendly)
            {
                float distanceToFriendly = Vector2.Distance(transform.position, closestFriendly.gameObject.transform.position);

                if (distanceToFriendly <= closeDistance && targets[conditionReactions[3][1]])
                {
                    stagesChecker[3] = true;
                }
                else
                {
                    if (distanceToFriendly >= farDistance && targets[conditionReactions[5][1]])
                    {
                        stagesChecker[5] = true;
                    }
                    else
                    {
                        stagesChecker[4] = true;
                    }
                }
            }

            if (closestEnemy)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, closestEnemy.gameObject.transform.position);

                if (distanceToEnemy <= closeDistance && targets[conditionReactions[0][1]])
                {
                    stagesChecker[0] = true;
                }
                else
                {
                    if (distanceToEnemy >= farDistance && targets[conditionReactions[2][1]])
                    {
                        stagesChecker[2] = true;
                    }
                    else
                    {
                        stagesChecker[1] = true;
                    }
                }
            }

            if (closestFood)
            {
                float distanceToFood = Vector2.Distance(transform.position, closestFood.gameObject.transform.position);

                if (distanceToFood <= closeDistance && targets[conditionReactions[6][1]])
                {
                    stagesChecker[6] = true;
                }
                else
                {
                    if (distanceToFood >= farDistance && targets[conditionReactions[8][1]])
                    {
                        stagesChecker[8] = true;
                    }
                    else
                    {
                        stagesChecker[7] = true;
                    }
                }
            }


            foreach (int chosedPriority in priorities)
            {
                if (stagesChecker[chosedPriority])
                {
                    if (targets[conditionReactions[chosedPriority][1]])
                    {
                        target = targets[conditionReactions[chosedPriority][1]];
                        targetPos = target.transform.position;
                    }
                    switch (reactions[conditionReactions[chosedPriority][0]])
                    {
                        case reaction.none:
                            {
                                atTravel = false;
                                DoNothing();
                                break;
                            }
                        case reaction.goThere:
                            {
                                atTravel = true;
                                StartCoroutine(GoThere(targetPos));
                                break;
                            }
                        case reaction.attack:
                            {
                                atTravel = true;
                                StartCoroutine(Attack(targetPos));
                                break;
                            }
                        case reaction.stayAtDistance:
                            {
                                atTravel = true;
                                StartCoroutine(StayAtDistance(targetPos, comfortableDistance));
                                break;
                            }
                    }
                    goto Skip;
                }
            }
            if (targets[0])
            {
                targetPos = targets[0].transform.position;
            }
            else
            {
                targetPos = transform.position + transform.up;
            }
            atTravel = true;

            StartCoroutine(GoThere(targetPos));
        }
    Skip:
        return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EdibleFood>())
        {
            EdibleFood collidedFood = collision.gameObject.GetComponent<EdibleFood>();
            if (CheckEdibility(collidedFood))
            {
                collidedFood.Eaten(gameObject);
                UpdateFoods();
                TryToSeparate();
            }
        }
    }

    public void TryToSeparate()
    {
        if (energy >= energyForSeparation)
        {
            energy -= separationValue * 0.5f;

            Bacteria spawnedBacteria = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<Bacteria>();

            spawnedBacteria.edibleFood = edibleFood.ToList();
            SpriteRenderer spr = spawnedBacteria.GetComponent<SpriteRenderer>();
            spawnedBacteria.energy = separationValue * 0.5f;
            spawnedBacteria.generation++;

            int numberOfMutations = 21;
            for (int i = 0; i < numberOfMutations + conditionReactions.Count; i++)
            {
                int numberOfMutation = Random.Range(0, numberOfMutations + conditionReactions.Count);
                if (Random.Range(0, 101) < chanceOfMutation)
                {
                    switch (numberOfMutation)
                    {
                        case 0:
                            {
                                spawnedBacteria.foodRadius = foodRadius * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #1");
                                break;
                            }
                        case 1:
                            {
                                spawnedBacteria.enemyRadius = enemyRadius * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #2");
                                break;
                            }
                        case 2:
                            {
                                spawnedBacteria.friendlyRadius = friendlyRadius * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #3");
                                break;
                            }
                        case 3:
                            {
                                spawnedBacteria.speedOfRotation = speedOfRotation * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #4");
                                break;
                            }
                        case 4:
                            {
                                spawnedBacteria.speedOfMove = speedOfMove * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #5");
                                break;
                            }
                        case 5:
                            {
                                spawnedBacteria.energy = energy * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #6");
                                break;
                            }
                        case 6:
                            {
                                spawnedBacteria.maxEnergy = maxEnergy * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #7");
                                break;
                            }
                        case 7:
                            {
                                spawnedBacteria.energyForSeparation = energyForSeparation * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #8");
                                break;
                            }
                        case 8:
                            {
                                spawnedBacteria.separationValue = separationValue * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #9");
                                break;
                            }
                        case 9:
                            {
                                spawnedBacteria.chanceOfMutation = chanceOfMutation * Random.Range(1f - mutationRangeSmall, 1f + mutationRangeSmall);
                                Debug.Log("Mutation #10");
                                break;
                            }
                        case 10:
                            {
                                spawnedBacteria.physicHealth = physicHealth * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #11");
                                break;
                            }
                        case 11:
                            {
                                spawnedBacteria.enemyTolerance = enemyTolerance * Random.Range(1f - mutationRangeSmall, 1f + mutationRangeSmall);
                                Debug.Log("Mutation #12");
                                break;
                            }
                        case 12:
                            {
                                spawnedBacteria.closeDistance = closeDistance * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #13");
                                break;
                            }
                        case 13:
                            {
                                spawnedBacteria.farDistance = farDistance * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #14");
                                break;
                            }
                        case 14:
                            {
                                spawnedBacteria.energyFromSun = energyFromSun * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #15");
                                break;
                            }
                        case 15:
                            {
                                spawnedBacteria.attackStrength = attackStrength * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #16");
                                break;
                            }
                        case 16:
                            {
                                spawnedBacteria.attackCooldown = attackCooldown * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #17");
                                break;
                            }
                        case 17:
                            {
                                spawnedBacteria.attackRange = attackRange * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #18");
                                break;
                            }
                        case 18:
                            {
                                spawnedBacteria.minEnergyForAttack = minEnergyForAttack * Random.Range(1f - mutationRangeMedium, 1f + mutationRangeMedium);
                                Debug.Log("Mutation #19");
                                break;
                            }
                        case 19:
                            {
                                int[] childPriorities = spawnedBacteria.priorities;
                                int swipePosition = Random.Range(0, childPriorities.Length-1);
                                int tempInt = childPriorities[swipePosition];
                                childPriorities[swipePosition] = childPriorities[swipePosition + 1];
                                childPriorities[swipePosition + 1] = tempInt;
                                Debug.Log("Mutation #20: " + swipePosition + childPriorities[swipePosition] + childPriorities[swipePosition + 1]);
                                break;
                            }
                        case 20:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[0][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            spawnedBacteria.conditionReactions[0][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #21");
                                break;
                            }
                        case 21:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[1][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            spawnedBacteria.conditionReactions[1][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #22");
                                break;
                            }
                        case 22:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[2][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            spawnedBacteria.conditionReactions[2][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #23");
                                break;
                            }
                        case 23:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[3][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            spawnedBacteria.conditionReactions[3][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #24");
                                break;
                            }
                        case 24:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[4][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            spawnedBacteria.conditionReactions[4][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #25");
                                break;
                            }
                        case 25:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[5][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            spawnedBacteria.conditionReactions[5][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #26");
                                break;
                            }
                        case 26:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[6][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            spawnedBacteria.conditionReactions[6][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #27");
                                break;
                            }
                        case 27:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[7][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            spawnedBacteria.conditionReactions[7][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #28");
                                break;
                            }
                        case 28:
                            {
                                switch (Random.Range(0, 2))
                                {
                                    case 0:
                                        {
                                            spawnedBacteria.conditionReactions[8][0] = Random.Range(0, reactions.Count);
                                            break;
                                        }
                                    case 1:
                                        {
                                            conditionReactions[8][1] = Random.Range(0, targets.Count);
                                            break;
                                        }
                                }
                                Debug.Log("Mutation #29");
                                break;
                            }
                        case 29:
                            {
                                Type foodInList = foodsList[Random.Range(0, foodsList.Count)];

                                if(spawnedBacteria.edibleFood.Contains(foodInList))
                                {
                                    spawnedBacteria.edibleFood.Remove(foodInList);
                                }
                                else
                                {
                                    spawnedBacteria.edibleFood.Add(foodInList);
                                }
                                Debug.Log("Mutation #30");
                                Debug.Log("Food changed!");
                                break;
                            }
                    }
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            {
                                spr.color = new Color(spr.color.r * Random.Range(1f - mutationRangeSmall, 1f + mutationRangeSmall), spr.color.g, spr.color.b);
                                break;
                            }
                        case 1:
                            {
                                spr.color = new Color(spr.color.r, spr.color.g * Random.Range(1f - mutationRangeSmall, 1f + mutationRangeSmall), spr.color.b);
                                break;
                            }
                        case 2:
                            {
                                spr.color = new Color(spr.color.r, spr.color.g, spr.color.b * Random.Range(1f - mutationRangeSmall, 1f + mutationRangeSmall));
                                break;
                            }
                    }
                }
            }
        }
    }

    public void UpdateFoods()
    {
        List<Food> closeFoods = new List<Food>();
        closestFood = null;
        float minDist = Mathf.Infinity;

        foreach (Collider2D food in Physics2D.OverlapCircleAll(transform.position, foodRadius))
        {
            Food currentFood = food.gameObject.GetComponent<Food>();
            if (currentFood)
            {
                closeFoods.Add(currentFood);
            }
        }

        foreach (Food food in closeFoods)
        {
            if (CheckEdibility(food))
            {
                float dist = Vector2.Distance(transform.position, food.gameObject.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestFood = food;
                }
            }
        }
    }

    public bool IsEnemy(Bacteria underChecking, Bacteria compareTo)
    {
        Color underCheckingColor = underChecking.gameObject.GetComponent<SpriteRenderer>().color;
        Color compareToColor = compareTo.gameObject.GetComponent<SpriteRenderer>().color;

        if ((underCheckingColor.r <= compareToColor.r * (1 + compareTo.enemyTolerance) && underCheckingColor.r >= compareToColor.r * (1 - compareTo.enemyTolerance)) || (underCheckingColor.g <= compareToColor.g * (1 + compareTo.enemyTolerance) && underCheckingColor.g >= compareToColor.g * (1 - compareTo.enemyTolerance)) || (underCheckingColor.b <= compareToColor.b * (1 + compareTo.enemyTolerance) && underCheckingColor.b >= compareToColor.b * (1 - compareTo.enemyTolerance)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void UpdateLives()
    {
        List<Bacteria> closeBacterias = new List<Bacteria>();
        closestEnemy = null;
        closestFriendly = null;
        float biggestRadius = 0;
        float minDistToEnemy = Mathf.Infinity;
        float minDistToFriendly = Mathf.Infinity;

        if (enemyRadius >= friendlyRadius)
        {
            biggestRadius = enemyRadius;
        }
        else
        {
            biggestRadius = friendlyRadius;
        }

        foreach (Collider2D bacteria in Physics2D.OverlapCircleAll(transform.position, biggestRadius))
        {
            Bacteria currentBacteria = bacteria.gameObject.GetComponent<Bacteria>();
            if (currentBacteria)
            {
                closeBacterias.Add(currentBacteria);
            }
        }

        foreach (Bacteria bacteria in closeBacterias)
        {
            float dist = Vector2.Distance(transform.position, bacteria.gameObject.transform.position);

            if (IsEnemy(bacteria, this))
            {
                if (dist < minDistToEnemy && dist <= enemyRadius)
                {
                    minDistToEnemy = dist;
                    closestEnemy = bacteria;
                }
            }
            else
            {
                if (dist < minDistToFriendly && bacteria != this && dist <= friendlyRadius)
                {
                    minDistToFriendly = dist;
                    closestFriendly = bacteria;
                }
            }
        }
    }

    public IEnumerator DoNothing()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, speedOfMove * Time.deltaTime);
        yield return null;
    }

    public IEnumerator GoThere(Vector3 targetPosition)
    {
        Vector3 tempRot = targetPosition - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, tempRot), speedOfRotation * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, speedOfMove * Time.deltaTime);
        atTravel = false;
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator Attack(Vector3 targetPosition)
    {
        while (Vector2.Distance(transform.position, targetPosition) > attackRange)
        {
            Vector3 tempRot = targetPosition - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, tempRot), speedOfRotation * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, speedOfMove * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (target)
        {
            if (target.GetComponent<Bacteria>())
            {
                Bacteria targetedBacteria = target.GetComponent<Bacteria>();
                if (attackTimer <= 0 && energy >= minEnergyForAttack)
                {
                    targetedBacteria.physicHealth -= attackStrength;
                    energy -= attackEnergy;
                    attackTimer = attackCooldown;
                }
            }
        }

        atTravel = false;
    }
    public IEnumerator StayAtDistance(Vector3 targetPosition, float targetDistance)
    {
        if (Vector3.Distance(transform.position, targetPosition) > targetDistance)
        {
            while (Vector3.Distance(transform.position, targetPosition) > targetDistance)
            {
                Vector3 tempRot = targetPosition - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, tempRot), speedOfRotation * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, speedOfMove * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (Vector3.Distance(transform.position, targetPosition) < targetDistance)
            {
                Vector3 tempRot = transform.position - targetPosition;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, tempRot), speedOfRotation * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, speedOfMove * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        atTravel = false;
    }
}
