using UnityEngine;

[System.Serializable]
public class Level
{
    public Transform ground;
    public Transform spawn;
    public Transform hardSpawn;
    public Transform wallMinLimit;
    public Transform wallMaxLimit;
    public GameObject transcend;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] public Level[] levels;
}
