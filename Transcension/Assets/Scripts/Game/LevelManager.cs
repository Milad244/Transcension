using UnityEngine;

/// <summary>
/// A serializable class that holds all the data needed to represent a level.
/// </summary>
[System.Serializable]
public class Level
{
    public int level;
    public Transform ground;
    public Transform spawn;
    public Transform hardSpawn;
    public Transform wallMinLimit;
    public Transform wallMaxLimit;
    public GameObject transcend;
    public string mindLevel;
    public GameObject content;

    public Vector3 spawnRevive
    {
        get
        {
            return CalculateAdjustedPosition(spawn);
        }
    }

    public Vector3 hardSpawnRevive
    {
        get
        {
            return CalculateAdjustedPosition(hardSpawn);
        }
    }

    public float wallMinLimitX
    {
        get{
            return wallMinLimit.position.x;
        }
    }

    public float wallMaxLimitX
    {
        get{
            return wallMaxLimit.position.x;
        }
    }

    /// <summary>
    /// Calculates an adjusted spawn position from the transform of a spawn to account for the player's height.
    /// </summary>
    /// <param name="spawnTransform">The spawn transform.</param>
    /// <returns></returns>
    private Vector3 CalculateAdjustedPosition(Transform spawnTransform)
    {
        if (spawnTransform != null)
        {
            return new Vector3(
                spawnTransform.position.x,
                spawnTransform.position.y + 2.228477f - 1,
                spawnTransform.position.z
            );
        }
        Debug.LogWarning("Could not get spawn position!");
        return Vector3.zero;
    }
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] public Level[] levels;
}
