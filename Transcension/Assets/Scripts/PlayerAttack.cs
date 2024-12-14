using JetBrains.Annotations;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement playerMovement;
    [SerializeField] private UIControl uiControl;
    [SerializeField] private float attackCD;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private float attackCDTimer;
    private float attackCharge;
    private bool attackMouseDown;
    private float holdChargeTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (attackCDTimer > 0)
        {
            attackCDTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            attackMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            attackMouseDown = false;

            if (attackCDTimer <= 0 && playerMovement.canAttack()) 
                attack();
        }
        
        // Adding charge if mouse button is being held down
        if (attackMouseDown)
        {
            if (attackCharge < 1)
                attackCharge += Time.deltaTime * 2;

            holdChargeTime += Time.deltaTime;
            
            if (holdChargeTime > 3) // Making sure user cannot indefinitely hold charge
            {
                holdChargeTime = 0;
                attackCharge = 0;
            } 
        }
        else
        { // Important. If cannot attack, it does not retain charge
            holdChargeTime = 0; 
            attackCharge = 0;
        }
            
        uiControl.updateCharge(attackCharge);
    }

    private void attack()
    {
        anim.SetTrigger("attack");

        fireballs[findFireball()].transform.position = firePoint.position;
        fireballs[findFireball()].GetComponent<Fireball>().setFireball(Mathf.Sign(transform.localScale.x), attackCharge);

        attackCDTimer = attackCD;
        attackCharge = 0;
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
