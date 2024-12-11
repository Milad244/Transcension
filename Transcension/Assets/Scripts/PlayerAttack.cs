using UnityEngine;

public class PlayerAttack : MonoBehaviour //Make it so the longer you hold down the bigger the fireball and dmg but larger CD and lower speed
{
    private Animator anim;
    private PlayerMovement playerMovement;
    [SerializeField] private float attackCD;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private float attackCDTimer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (attackCDTimer <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0) && playerMovement.canAttack())
                attack();
        } else
            attackCDTimer -= Time.deltaTime;
    }

    private void attack()
    {
        anim.SetTrigger("attack");
        attackCDTimer = attackCD;

        fireballs[findFireball()].transform.position = firePoint.position;
        fireballs[findFireball()].GetComponent<Fireball>().setDirection(Mathf.Sign(transform.localScale.x));
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
