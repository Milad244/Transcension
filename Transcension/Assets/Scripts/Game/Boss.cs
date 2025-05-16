using System;
using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private UIControl uiControl;
    [SerializeField] private Transform firePointLeftLim;
    [SerializeField] private Transform firePointRightLim;
    private Vector3 worldFirePointLeftLim;
    private Vector3 worldFirePointRightLim;
    private int firePointCount = 20;
    private Vector3[] firePoints;
    [SerializeField] private GameObject fireballHolder;
    private int fireballCount = 100;
    [SerializeField] private GameObject fireballPrefab;
    private GameObject[] fireballs;
    private Coroutine attackSetCoroutine;
    void Awake()
    {
        buildFirePoints();
        buildFireballs();
    }

    private void buildFirePoints()
    {
        worldFirePointLeftLim = firePointLeftLim.position;
        worldFirePointRightLim = firePointRightLim.position;

        firePoints = new Vector3[firePointCount]; 
        float firePointRange = worldFirePointRightLim.x - worldFirePointLeftLim.x;
        float intervalLength = firePointRange / firePointCount;
        for (int i = 0; i < firePointCount; i++)
        {
            float x = worldFirePointLeftLim.x + intervalLength * i;
            firePoints[i] = new Vector3(x, worldFirePointLeftLim.y, worldFirePointLeftLim.z);
            //Debug.DrawRay(firePoints[i], Vector3.up * 2f, Color.red, 15f);
        }
    }

    private void buildFireballs()
    {
        fireballs = new GameObject[fireballCount];
        for (int i = 0; i < fireballCount; i++)
        {
            GameObject fireball = Instantiate(fireballPrefab, fireballHolder.transform);
            fireball.SetActive(false);
            fireballs[i] = fireball;
        }
    }

    private void stopFireballs()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].SetActive(false);
        }
    }

    private void startAttackCoroutine(IEnumerator attackSet)
    {
        if (attackSetCoroutine != null)
        {
            StopCoroutine(attackSetCoroutine);
        }
        attackSetCoroutine = StartCoroutine(attackSet);
    }

    public void startBossFight() //called from player movement
    {
        stopFireballs();
        startAttackCoroutine(attackSet1());
    }

    private IEnumerator attackSet1()
    {
        float setTime = 20f;
        float elapsed = 0f;

        float interval = 0.1f;
        int count = 0;
        while (elapsed < setTime)
        {
            if (elapsed >= count * interval)
            {
                int firePointI = UnityEngine.Random.Range(0, firePoints.Length);
                Vector3 velocity = new Vector3(0, -10, 0);
                attack(firePointI, velocity);
                count++;
            }

            yield return null;
            elapsed += Time.deltaTime;
        }
        startAttackCoroutine(attackSet2());
    }

    private IEnumerator attackSet2()
    {
        float setTime = 20f;
        float elapsed = 0f;

        float interval = 0.25f;
        int count = 0;
        while (elapsed < setTime)
        {
            if (elapsed >= count * interval)
            {
                int firePointI = UnityEngine.Random.Range(1, firePoints.Length - 1);
                Vector3 velocity = new Vector3(0, -8, 0);
                attack(firePointI - 1, velocity);
                attack(firePointI, velocity);
                attack(firePointI + 1, velocity);
                count++;
            }

            yield return null;
            elapsed += Time.deltaTime;
        }
        Debug.Log("YOU WON!");
    }
    
    private void attack(int firePointI, Vector3 velocity)
    {
        int index = findFireball();
        fireballs[index].transform.position = firePoints[firePointI];
        fireballs[index].GetComponent<Fireball>().setFireball(velocity);
    }

    private int findFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if(!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
