using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bacteria;

public class BacteriaVariables : VisibleVariables
{
    public Bacteria linkedBacteria;

    [SerializeField]
    public float PhysicHealth
    {
        get => linkedBacteria.physicHealth;
        set
        {
            linkedBacteria.physicHealth = value;
        }
    }

    [SerializeField]
    public float MaxPhysicHealth
    {
        get => linkedBacteria.maxPhysicHealth;
        set
        {
            linkedBacteria.maxPhysicHealth = value;
        }
    }


    [SerializeField]
    public float AttackStrength
    {
        get => linkedBacteria.attackStrength;
        set
        {
            linkedBacteria.attackStrength = value;
        }
    }

    [SerializeField]
    public float AttackCooldown
    {
        get => linkedBacteria.attackCooldown;
        set
        {
            linkedBacteria.attackCooldown = value;
        }
    }

    [SerializeField]
    public float AttackRange
    {
        get => linkedBacteria.attackRange;
        set
        {
            linkedBacteria.attackRange = value;
        }
    }

    [SerializeField]
    public float MinEnergyForAttack
    {
        get => linkedBacteria.minEnergyForAttack;
        set
        {
            linkedBacteria.minEnergyForAttack = value;
        }
    }

    [SerializeField]
    public float AttackEnergy
    {
        get => linkedBacteria.attackEnergy;
        set
        {
            linkedBacteria.attackEnergy = value;
        }
    }

    [SerializeField]
    public float FoodRadius
    {
        get => linkedBacteria.foodRadius;
        set
        {
            linkedBacteria.foodRadius = value;
        }
    }

    [SerializeField]
    public float EnemyRadius
    {
        get => linkedBacteria.enemyRadius;
        set
        {
            linkedBacteria.enemyRadius = value;
        }
    }

    [SerializeField]
    public float FriendlyRadius
    {
        get => linkedBacteria.friendlyRadius;
        set
        {
            linkedBacteria.friendlyRadius = value;
        }
    }

    [SerializeField]
    public float EnemyTolerance
    {
        get => linkedBacteria.enemyTolerance;
        set
        {
            linkedBacteria.enemyTolerance = value;
        }
    }

    [SerializeField]
    public float SpeedOfRotation
    {
        get => linkedBacteria.speedOfRotation;
        set
        {
            linkedBacteria.speedOfRotation = value;
        }
    }

    [SerializeField]
    public float SpeedOfMove
    {
        get => linkedBacteria.speedOfMove;
        set
        {
            linkedBacteria.speedOfMove = value;
        }
    }

    [SerializeField]
    public float CloseDistance
    {
        get => linkedBacteria.closeDistance;
        set
        {
            linkedBacteria.closeDistance = value;
        }
    }

    [SerializeField]
    public float FarDistance
    {
        get => linkedBacteria.farDistance;
        set
        {
            linkedBacteria.farDistance = value;
        }
    }

    [SerializeField]
    public float EnergyFromSun
    {
        get => linkedBacteria.energyFromSun;
        set
        {
            linkedBacteria.energyFromSun = value;
        }
    }

    [SerializeField]
    public float CurrentEnergy
    {
        get => linkedBacteria.energy;
        set
        {
            linkedBacteria.energy = value;
        }
    }

    [SerializeField]
    public float MaxEnergy
    {
        get => linkedBacteria.maxEnergy;
        set
        {
            linkedBacteria.maxEnergy = value;
        }
    }

    [SerializeField]
    public float EnergyForSeparation
    {
        get => linkedBacteria.energyForSeparation;
        set
        {
            linkedBacteria.energyForSeparation = value;
        }
    }

    [SerializeField]
    public float SeparationValue
    {
        get => linkedBacteria.separationValue;
        set
        {
            linkedBacteria.separationValue = value;
        }
    }

    [SerializeField]
    public float ChanceOfMutation
    {
        get => linkedBacteria.chanceOfMutation;
        set
        {
            linkedBacteria.chanceOfMutation = value;
        }
    }

    [SerializeField]
    public float SmallMutationRange
    {
        get => linkedBacteria.mutationRangeSmall;
        set
        {
            linkedBacteria.mutationRangeSmall = value;
        }
    }

    [SerializeField]
    public float MediumMutationRange
    {
        get => linkedBacteria.mutationRangeMedium;
        set
        {
            linkedBacteria.mutationRangeMedium = value;
        }
    }

    [SerializeField]
    public float BigMutationRange
    {
        get => linkedBacteria.mutationRangeBig;
        set
        {
            linkedBacteria.mutationRangeBig = value;
        }
    }

    [SerializeField]
    public int Generation
    {
        get => linkedBacteria.generation;
        set
        {
            linkedBacteria.generation = value;
        }
    }

    [SerializeField]
    public int[] Priorities
    {
        get => linkedBacteria.priorities;
        set
        {
            linkedBacteria.priorities = value;
        }
    }

    [SerializeField]
    public List<Type> EdibleFood
    {
        get => linkedBacteria.edibleFood;
        set
        {
            linkedBacteria.edibleFood = value;
        }
    }

    [SerializeField]
    public IDictionary<int, int[]> ConditionsReactions
    {
        get => linkedBacteria.conditionReactions;
        set
        {
            linkedBacteria.conditionReactions = value;
        }
    }

    void Awake()
    {
        linkedBacteria = gameObject.GetComponent<Bacteria>();
    }
}
