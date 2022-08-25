using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: PlayerCombat
// Author: Ajay Ramnarine
// Purpose: Contains all variables and methods associated with the attacks of the player character
// Restrictions: None
public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    private GameManager gameManager;

    // attack points
    public Transform nAttackPoint;
    public Vector2 nAttackRange = new Vector2(0.5f, 0.5f);
    public int nDamage = 10;
    public float nKnockbackSpeed = 20f;
    public float nAttackRate = 2f;
    float nNextAttack = 0f;

    public Transform fAttackPoint;
    public Vector2 fAttackRange = new Vector2(0.5f, 0.5f);
    public int fDamage = 50;
    public float fKnockbackSpeed = 80f;
    public float fAttackRate = 0.2f;
    float fNextAttack = 0f;

    public Transform sAttackPoint;
    public Vector2 sAttackRange = new Vector2(0.5f, 0.5f);
    public int sDamage = 40;
    public float sKnockbackSpeed = 40f;
    public float sAttackRate = 0.3f;
    float sNextAttack = 0f;

    public Transform uAttackPoint;
    public Vector2 uAttackRange = new Vector2(0.5f, 0.5f);
    public int uDamage = 35;
    public float uKnockbackSpeed = 56f;
    public float uAttackRate = 0.3f;
    float uNextAttack = 0f;

    // boss layer
    public LayerMask bossLayer;

    // projectile layer
    public LayerMask projLayer;

    // Start is called before the first frame update
    void Start()
    {
        // get the game manager
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the cooldown for netural attack is done
        if (Time.time >= nNextAttack)
        {
            if (Input.GetKeyDown("space"))
            {
                // character does a neutral attack when the spacebar is pressed
                NeutralAttack();

                // set the cooldown for the attack
                nNextAttack = Time.time + 1f / nAttackRate;
            }
        }

        // checks if the cooldown for the forward attack is done
        if (Time.time >= fNextAttack)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z))
            {
                // character does a forward attack when either J or Z is pressed
                ForwardAttack();

                // set the cooldown for the attack
                fNextAttack = Time.time + 1f / fAttackRate;
            }
        }

        // checks if the cooldown for the slam attack is done
        if (Time.time >= sNextAttack)
        {
            if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X))
            {
                // character does a slam attack when either K or X is pressed
                SlamAttack();

                // set the cooldown for the attack
                sNextAttack = Time.time + 1f / sAttackRate;
            }
        }

        // checks if the cooldown for the uppercut attack is done
        if (Time.time >= uNextAttack)
        {
            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.C))
            {
                // character does an uppercut attack when either L or C is pressed
                UppercutAttack();

                // set the cooldown for the attack
                uNextAttack = Time.time + 1f / uAttackRate;
            }
        }
    }

    // Method: NeutralAttack
    // Purpose: Player performs a netural attack when the method is called
    // Restrictions: None
    void NeutralAttack()
    {
        // Play attack animation and set the player speed to zero
        animator.SetTrigger("NeutralAttack");
        GetComponent<PlayerController>().speed = 0f;
        GetComponent<PlayerController>().isSpeed = false;

        // Detect nearby enemies that will be damaged by attack
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(nAttackPoint.position, nAttackRange, 0f, bossLayer);

        if (hitEnemies != null)
        {
            // Damage and knockback the enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<BossController>().Damaged(nDamage);
                enemy.GetComponent<BossController>().knockbackSpeed = nKnockbackSpeed;

                // increase the score and the multiplier
                gameManager.IncreaseScore(nDamage);
                gameManager.IncreaseMultiplier();
            }
        }

    }

    // Method: SlamAttack
    // Purpose: Player performs a slam attack when the method is called
    // Restrictions: None
    void SlamAttack()
    {
        // Play attack animation and set the player speed to zero
        animator.SetTrigger("SlamAttack");
        GetComponent<PlayerController>().speed = 0f;
        GetComponent<PlayerController>().isSpeed = false;

        // Detect nearby enemies that will be damaged by attack
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(sAttackPoint.position, sAttackRange, 0f, bossLayer);

        // Detect nearby projectiles that will be destoryed by attack
        Collider2D[] hitProjectiles = Physics2D.OverlapBoxAll(sAttackPoint.position, sAttackRange, 0f, projLayer);

        if (hitEnemies != null)
        {
            // Damage and knockback the enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<BossController>() != null)
                {
                    enemy.GetComponent<BossController>().Damaged(sDamage);
                    enemy.GetComponent<BossController>().knockbackSpeed = sKnockbackSpeed;

                    // increase the score and the multiplier
                    gameManager.IncreaseScore(sDamage);
                    gameManager.IncreaseMultiplier();
                }
            }
        }

        if(hitProjectiles != null)
        {
            // explode each projectile that is hit by the attack
            foreach(Collider2D projectile in hitProjectiles)
            {
                projectile.GetComponent<Projectile>().Explode();

                // increase the score and the multiplier
                gameManager.IncreaseScore(sDamage);
                gameManager.IncreaseMultiplier();
            }
        }
    }

    // Method: ForwardAttack
    // Purpose: Player performs a forward attack when the method is called
    // Restrictions: None
    void ForwardAttack()
    {
        // Play attack animation and set the player speed to zero
        animator.SetTrigger("ForwardAttack");
        GetComponent<PlayerController>().speed = 0f;
        GetComponent<PlayerController>().isSpeed = false;

        // Detect nearby enemies that will be damaged by attack
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(fAttackPoint.position, fAttackRange, 0f, bossLayer);

        if (hitEnemies != null)
        {
            // Damage and knockback the enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<BossController>().Damaged(fDamage);
                enemy.GetComponent<BossController>().knockbackSpeed = fKnockbackSpeed;

                // increase the score and the multiplier
                gameManager.IncreaseScore(fDamage);
                gameManager.IncreaseMultiplier();
            }
        }
    }

    // Method: UppercutAttack
    // Purpose: Player performs an uppercut attack when the method is called
    // Restrictions: None
    void UppercutAttack()
    {
        // Play attack animation and set the player speed to zero
        animator.SetTrigger("UppercutAttack");
        GetComponent<PlayerController>().speed = 0f;
        GetComponent<PlayerController>().isSpeed = false;

        // Detect nearby enemies that will be damaged by attack
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(uAttackPoint.position, uAttackRange, 0f, bossLayer);

        // Detect nearby projectiles that will be reflected by the attack
        Collider2D[] hitProjectiles = Physics2D.OverlapBoxAll(uAttackPoint.position, uAttackRange, 0f, projLayer);

        if (hitEnemies != null)
        {
            // Damage and knockback the enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<BossController>().Damaged(uDamage);
                enemy.GetComponent<BossController>().knockbackSpeed = uKnockbackSpeed;

                // increase the score and the multiplier
                gameManager.IncreaseScore(uDamage);
                gameManager.IncreaseMultiplier();
            }
        }

        if(hitProjectiles != null)
        {
            // reflect each projectile that is hit by the attack
            foreach(Collider2D projectile in hitProjectiles)
            {
                projectile.GetComponent<Projectile>().Reflect();

                // increase the score and the multiplier
                gameManager.IncreaseScore(uDamage);
                gameManager.IncreaseMultiplier();
            }
        }
    }

    // Method: OnDrawGizmosSelected
    // Purpose: Used to draw the shape of the attacks so that their hit boxes can be adjusted visually
    // Restrictions: None
    void OnDrawGizmosSelected()
    {
        if (nAttackPoint == null || sAttackPoint == null || fAttackPoint == null || uAttackPoint == null)
            return;

        // draws the hitbox for neutral attack
        Gizmos.DrawWireCube(nAttackPoint.position, nAttackRange);

        // draws the hitbox for the slam attack
        Gizmos.DrawWireCube(sAttackPoint.position, sAttackRange);

        // draws the hitbox for the forward attack
        Gizmos.DrawWireCube(fAttackPoint.position, fAttackRange);

        // draws the hitbox for the uppercut attack
        Gizmos.DrawWireCube(uAttackPoint.position, uAttackRange);

    }
}
