using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private ParticleSystem speedParticles;

    [SerializeField] private GameObject cameraHolder;
    private CameraController cameraController;

    [SerializeField] private GameObject gameManager;
    private LevelManager levelManager;

    private float speed = 10;
    private float jumpPower = 20;
    private float ceilingPushPower = 30;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask pushLayer;

    private float horizontalInput;
    public bool dying;
    private Vector3 revivePos;
    private GlobalSceneManager globalSceneManager;
    [SerializeField] private UIControl uiControl;
    private int currentLevel;
    private bool isSlow = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        speedParticles = GetComponent<ParticleSystem>();

        cameraController = cameraHolder.GetComponent<CameraController>();
        levelManager = gameManager.GetComponent<LevelManager>();

        globalSceneManager = GameObject.Find("GlobalManager").GetComponent<GlobalSceneManager>();

        setSpeedDefault();

        currentLevel = globalSceneManager.level;

        // Dealing with initial spawn
        loadLevel(levelManager.levels[currentLevel]);
    }

    private void Update()
    {
        if (globalSceneManager.isBlocked)
        {
            return;
        }
        else if (dying)
        {
            // Code to check dying anim and if not happening then start it
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Die")
            {
                anim.SetTrigger("die");
            }
            return;
        }

        // horizontal movement  
        horizontalInput = Input.GetAxis("Horizontal");

        // flipping character
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // animation params
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // applying movement
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        handlePush();

        if (Input.GetKey(KeyCode.Space))
            Jump();
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
    }

    private void ceilingPush()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, -ceilingPushPower);
    }
    private void floorPush()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, ceilingPushPower);
    }

    private bool isGrounded()
    {
        float rayLength = 0.1f;
        Vector2 rayOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
        RaycastHit2D rayCastHit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundLayer);
        return rayCastHit.collider != null;
    }

    private void handlePush()
    {
        RaycastHit2D rayCastCeilingHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, pushLayer);
        RaycastHit2D rayCastFloorHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, pushLayer);
        if (rayCastCeilingHit)
        {
            ceilingPush();
        }
        else if (rayCastFloorHit)
        {
            floorPush();
        }
    }

    public void dieMovement()
    {
        if (dying)
            return;
        dying = true;
        globalSceneManager.addToDeathCount();
        body.linearVelocity = new Vector2(0, 0);
        setSpeedDefault();
        anim.SetTrigger("die");
    }

    public void reviveMovement()
    {
        transform.localPosition = revivePos;
        anim.SetTrigger("returnIdle");
        dying = false;
    }

    public void toggleSpeedSlow(bool slow)
    {
        speedParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        isSlow = slow;
        if (slow)
        {
            speed = 1.5f;
        }
        else
        {
            speed = 10;
        }
    }

    // only used for speed boost
    public void setSpeedBoost(float newSpeed)
    {
        speed = newSpeed;
        speedParticles.Play();
    }

    // only used for speed boost
    public void setSpeedDefault()
    {
        speedParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (!isSlow)
        {
            speed = 10;
        }
    }

    public void tellTo(Transform position)
    {
        transform.localPosition = position.position;
    }

    private float adjustFloorLimit(Transform floor) //So camera doesn't show beneath floor
    {
        return 2 + floor.position.y + 1; // Y-Size of ground + y position of ground + playerheight DOES NOT WORK FOR ANY SCALE OTHER THAN 2!
    }

    public void loadLevel(Level level)
    {
        revivePos = level.spawnRevive;
        transform.localPosition = revivePos;

        cameraController.changeFloorLimit(adjustFloorLimit(level.ground));
        cameraController.changeWallLimit(level.wallMinLimitX, level.wallMaxLimitX);

        if (!level.mindLevel.Equals("") && currentLevel != level.level)
        {
            uiControl.mindTransition();
            globalSceneManager.loadMindScene(level.mindLevel);
        }

        if (currentLevel == 5)
        {
            StartCoroutine(startBoss());
        }
    }

    public void transcend(GameObject transcendObject)
    {
        foreach (Level level in levelManager.levels)
        {
            if (level.transcend == transcendObject)
            {
                globalSceneManager.setLevel(level.level);
                loadLevel(level);
                break;
            }
        }
    }

    public void hardSpawn()
    {
        int currentLevel = globalSceneManager.level;
        Level currentLevelObject = null;

        foreach (Level level in levelManager.levels)
        {
            if (level.level == currentLevel)
            {
                currentLevelObject = level;
                break;
            }
        }

        if (this != null && currentLevelObject != null)
        {
            revivePos = currentLevelObject.hardSpawnRevive;
        }
        else
        {
            Debug.LogWarning("Current level not found in level manager.");
        }
    }

    public IEnumerator startBoss()
    {
        yield return new WaitWhile(() => dying == true); //waiting if player is dead
        GameObject.Find("Boss").GetComponent<Boss>().startBossFight();
    }
}
