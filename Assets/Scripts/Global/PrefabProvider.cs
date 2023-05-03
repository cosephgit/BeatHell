using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// singleton class which contains references to important universal prefabs

public class PrefabProvider : MonoBehaviour
{
    public static PrefabProvider instance;
    [field: SerializeField]public EffectTimed particlePop { get; private set; }
    [field: SerializeField]public GameObject[] enemyPrefab { get; private set; }
    [field: SerializeField]public float[] enemyStrength { get; private set; }

    void Awake()
    {
        if (instance)
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
            instance = this;
    }
}
