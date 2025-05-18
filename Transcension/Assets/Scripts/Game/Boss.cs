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
    private int fireballCount = 150;
    [SerializeField] private GameObject fireballPrefab;
    private GameObject[] fireballs;
    private Coroutine attackSetCoroutine;
    private int stage = 1;
    private float currentHealth = 500f;
    private float healthDropPerSet = 100f;
    private GlobalSceneManager globalSceneManager;
    void Awake()
    {
        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();
        buildFirePoints();
        buildFireballs();
    }

    private void buildFirePoints()
    {
        worldFirePointLeftLim = firePointLeftLim.position;
        worldFirePointRightLim = firePointRightLim.position;

        firePoints = new Vector3[firePointCount+1]; 
        float firePointRange = worldFirePointRightLim.x - worldFirePointLeftLim.x;
        float intervalLength = firePointRange / firePointCount;
        for (int i = 0; i <= firePointCount; i++)
        {
            float x = worldFirePointLeftLim.x + intervalLength * i;
            firePoints[i] = new Vector3(x, worldFirePointLeftLim.y, worldFirePointLeftLim.z);
            Debug.DrawRay(firePoints[i], Vector3.up * 2f, Color.red, 15f);
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
        switch (stage)
        {
            case 1:
                startAttackCoroutine(attackSet1());
                break;
            case 2:
                startAttackCoroutine(attackSet2());
                break;
            case 3:
                startAttackCoroutine(attackSet3());
                break;
            case 4:
                startAttackCoroutine(attackSet4());
                break;
            case 5:
                startAttackCoroutine(attackSet5());
                break;
            default:
                Debug.LogWarning("Boss stage not found");
                break;
        }
    }
    
    private bool anyFireballActive()
    {
        foreach (GameObject fireball in fireballs)
        {
            if (fireball.activeInHierarchy)
                return true;
        }
        return false;
    }

    public IEnumerator waitForAllFireballsInactive()
    {
        while (anyFireballActive())
        {
            yield return null;
        }
    }


    private IEnumerator attackSet1()
    {
        StartCoroutine(uiControl.updateHealthBar(currentHealth, currentHealth));
        yield return new WaitForSeconds(3);

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

        yield return StartCoroutine(waitForAllFireballsInactive());

        if (GameObject.Find("Player").GetComponent<PlayerMovement>().dying)
            yield break;

        StartCoroutine(uiControl.updateHealthBar(currentHealth, currentHealth - healthDropPerSet));
        yield return new WaitForSeconds(3);

        startAttackCoroutine(attackSet2());

        currentHealth -= healthDropPerSet;
        stage = 2;
    }

    private IEnumerator attackSet2() // 3 set fireballs
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

        yield return StartCoroutine(waitForAllFireballsInactive());

        if (GameObject.Find("Player").GetComponent<PlayerMovement>().dying)
            yield break;

        StartCoroutine(uiControl.updateHealthBar(currentHealth, currentHealth - healthDropPerSet));
        yield return new WaitForSeconds(3);

        startAttackCoroutine(attackSet3());
        
        currentHealth -= healthDropPerSet;
        stage = 3;
    }

    private IEnumerator attackSet3() // diagonal fireballs
    {
        float setTime = 20f;
        float elapsed = 0f;

        float interval = 0.15f;
        int count = 0;
        while (elapsed < setTime)
        {
            if (elapsed >= count * interval)
            {
                int firePointI = UnityEngine.Random.Range(0, firePoints.Length);
                float xPos = firePoints[firePointI].x;
                float diffLeft = Math.Abs(worldFirePointLeftLim.x - xPos);
                float diffRight = Math.Abs(worldFirePointRightLim.x - xPos);

                int x = UnityEngine.Random.Range(0, 15);
                if (diffLeft > diffRight)
                {
                    x = -x;
                }
                Vector3 velocity = new Vector3(x, -9, 0);
                attack(firePointI, velocity);
                count++;
            }

            yield return null;
            elapsed += Time.deltaTime;
        }

        yield return StartCoroutine(waitForAllFireballsInactive());

        if (GameObject.Find("Player").GetComponent<PlayerMovement>().dying)
            yield break;

        StartCoroutine(uiControl.updateHealthBar(currentHealth, currentHealth - healthDropPerSet));
        yield return new WaitForSeconds(3);

        startAttackCoroutine(attackSet4());
        
        currentHealth -= healthDropPerSet;
        stage = 4;
    }

    private IEnumerator attackSet4() // slow falling fireballs
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
                Vector3 velocity = new Vector3(0, -2, 0);
                attack(firePointI, velocity);
                count++;
            }

            yield return null;
            elapsed += Time.deltaTime;
        }

        yield return StartCoroutine(waitForAllFireballsInactive());

        if (GameObject.Find("Player").GetComponent<PlayerMovement>().dying)
            yield break;

        StartCoroutine(uiControl.updateHealthBar(currentHealth, currentHealth - healthDropPerSet));
        yield return new WaitForSeconds(3);

        startAttackCoroutine(attackSet5());
        
        currentHealth -= healthDropPerSet;
        stage = 5;
    }

    private IEnumerator attackSet5() //making it into continous fireballs
    {
        float setTime = 20f;
        float elapsed = 0f;

        float interval = 0.4f;
        int count = 0;

        while (elapsed < setTime)
        {
            if (elapsed >= count * interval)
            {
                int firePointI = count % firePoints.Length;
                int firePointI2 = 20 - firePointI;
                int firePointI3 = (firePointI + firePointI2) / 2;
                Vector3 velocity = new Vector3(0, -25, 0);
                attack(firePointI, velocity);
                attack(firePointI2, velocity);
                attack(firePointI3, velocity);
                count++;
            }

            yield return null;
            elapsed += Time.deltaTime;
        }

        yield return StartCoroutine(waitForAllFireballsInactive());

        if (GameObject.Find("Player").GetComponent<PlayerMovement>().dying)
            yield break;
            
        StartCoroutine(uiControl.updateHealthBar(currentHealth, currentHealth - healthDropPerSet));
        yield return new WaitForSeconds(3);

        currentHealth -= healthDropPerSet;
        winBoss();
    }

    private void winBoss()
    {
        uiControl.mindTransition();
        globalSceneManager.loadMindScene("tran5");
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
