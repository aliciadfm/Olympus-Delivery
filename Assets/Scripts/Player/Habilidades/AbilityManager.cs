using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    private HashSet<AbilityType> unlockedAbilities = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Unlock(AbilityType.Run);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Unlock(AbilityType ability)
    {
        if (unlockedAbilities.Add(ability))
        {
            Debug.Log("Habilidad desbloqueada: " + ability);
        }
    }

    public bool HasAbility(AbilityType ability)
    {
        return unlockedAbilities.Contains(ability);
    }
}
