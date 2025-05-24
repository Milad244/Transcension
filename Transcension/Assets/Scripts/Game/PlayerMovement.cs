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

        // Setup initial spawn
        loadLevel(levelManager.levels[currentLevel]);
    }

    private void Update()
    {
        if (globalSceneManager.isBlocked)
            return;

        if (dying)
        {
            // Code to check dying anim and if not happening then start it
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Die")
            {
                anim.SetTrigger("die");
            }
            return;
        }

        // Horizontal movement  
        horizontalInput = Input.GetAxis("Horizontal");

        // Flipping character
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Animation params
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Applying movement
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        handlePush();

        if (Input.GetKey(globalSceneManager.keyBinds[GlobalSceneManager.Binds.Jump]))
            Jump();
    }

    /// <summary>
    /// Makes player jump with a set jumpPower y-velocity and triggers the jumping animation. Only jumps if player is on a ground.
    /// </summary>
    private void Jump()
    {
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
    }

    /// <summary>
    /// Pushes the player down with a set ceilingPushPower y-velocity.
    /// </summary>
    private void ceilingPush()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, -ceilingPushPower);
    }

    /// <summary>
    /// Pushes the player up with a set ceilingPushPower y-velocity.
    /// </summary>
    private void floorPush()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, ceilingPushPower);
    }

    /// <summary>
    /// Checks if the player is standing on a ground layer using a raycast.
    /// </summary>
    /// <returns>True if player is grounded; otherwise false.</returns>
    private bool isGrounded()
    {
        float rayLength = 0.1f;
        Vector2 rayOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
        RaycastHit2D rayCastHit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundLayer);
        return rayCastHit.collider != null;
    }

    /// <summary>
    /// If player is below a Push, pushes player down using ceilingPush(). If player is above a Push, pushes player up using floorPush().
    /// </summary>
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

    /// <summary>
    /// Starts player death, increments death count, resets movement, and triggers the death animation. 
    /// Only runs if player is not already dying.
    /// </summary>
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

    /// <summary>
    /// Teleports player back to their spawn position, resets the animation to idle, and sets the dying variable back to false.
    /// </summary>
    public void reviveMovement()
    {
        transform.localPosition = revivePos;
        anim.SetTrigger("returnIdle");
        dying = false;
    }

    /// <summary>
    /// Toggles the slow speed effect on or off, adjusting player speed accordingly, and stops any active speed particle effects.
    /// </summary>
    /// <param name="slow">True to slow down the player, false to return to normal speed.</param>
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

    /// <summary>
    /// Sets a new speed for the player as part of a speed boost and starts the speed particles effect.
    /// </summary>
    /// <param name="newSpeed">The new boosted speed value.</param>
    public void setSpeedBoost(float newSpeed)
    {
        speed = newSpeed;
        speedParticles.Play();
    }

    /// <summary>
    /// Resets the player speed to default if not slowed and stops the speed particles effect.
    /// </summary>
    public void setSpeedDefault()
    {
        speedParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (!isSlow)
        {
            speed = 10;
        }
    }

    /// <summary>
    /// Teleports player to a given transform's position.
    /// </summary>
    /// <param name="position">The target transform to teleport the player to.</param>
    public void tellTo(Transform position)
    {
        transform.localPosition = position.position;
    }

    /// <summary>
    /// Calculates an adjusted Y position for the camera floor limit based on the floor's position and fixed offsets.
    /// </summary>
    /// <param name="floor">The transform of the floor to base the adjustment on.</param>
    /// <returns>A float representing the adjusted Y position floor limit.</returns>
    private float adjustFloorLimit(Transform floor) //So camera doesn't show beneath floor
    {
        return 2 + floor.position.y + 1; // Y-Size of ground + Y position of floor + player height. DOES NOT WORK FOR ANY SCALE OTHER THAN 2!
    }

    /// <summary>
    /// Activates the content of the given level and deactivates all the other levels' content.
    /// </summary>
    private void loadContent(Level levelToLoad)
    {
        levelToLoad.content.SetActive(true);

        foreach (Level level in levelManager.levels)
        {
            if (level != levelToLoad)
            {
                level.content.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Loads a level: sets the player's spawn position and teleports the player there.
    /// Updates the camera's floor and wall limits based on the level.
    /// Loads the level's content and transitions into the mind if its the first visit.
    /// Starts the boss fight if the level is 5.
    /// </summary>
    /// <param name="level">The level to load.</param>
    public void loadLevel(Level level)
    {
        revivePos = level.spawnRevive;
        transform.localPosition = revivePos;

        cameraController.changeFloorLimit(adjustFloorLimit(level.ground));
        cameraController.changeWallLimit(level.wallMinLimitX, level.wallMaxLimitX);

        loadContent(level);

        if (!level.mindLevel.Equals("") && currentLevel != level.level) // only go to mind if its the first time visiting the level
        {
            uiControl.mindTransition();
            globalSceneManager.loadMindScene(level.mindLevel);
        }

        if (currentLevel == 5)
        {
            StartCoroutine(startBoss());
        }
    }

    /// <summary>
    /// Sets the current level based on the given transcend object's associated level and loads it.
    /// </summary>
    /// <param name="transcendObject">The GameObject representing the transcend object linked to a level.</param>
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

    /// <summary>
    /// Sets the spawnpoint of the player to the current level's hard spawn.
    /// </summary>
    public void hardSpawn()
    {
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

    /// <summary>
    /// Starts the boss fight. If player is dying, waits until they stop dying before starting.
    /// </summary>
    public IEnumerator startBoss()
    {
        yield return new WaitWhile(() => dying == true); // waiting if player is dead
        GameObject.Find("Boss").GetComponent<Boss>().startBossFight();
    }
}
