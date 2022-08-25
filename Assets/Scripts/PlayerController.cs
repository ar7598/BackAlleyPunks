using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: PlayerController
// Author: Ajay Ramnarine
// Purpose: Contains all variables and methods associated with moving the player character
// Restrictions: None
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public GameObject player;
    private GameManager gameManager;

    public GameObject boss;

    public float speed = 5f;
    public int health = 100;
    int currentHealth;

    public HealthBar healthBar;

    // parameters for dashing
    public float dashSpeed = 20f;
    public Vector2 dashDirection;
    public float startDashTime = 1f;
    float currentDashTime;
    bool isDashing = false;

    // parameters for knockback
    public float knockbackSpeed;
    public float startKnockback = 0.7f;
    float currentKnockback;
    public Vector2 knockbackDirection;
    bool isKnockback = false;
    float bossXDirection = 100000f;
    float bossYDirection = 100000f;

    // boolean to check if the speed has been reduced to zero
    public bool isSpeed = true;

    // vector to track player movement in the x and y directions
    Vector2 movement;

    // parameters for flipping the sprite of the player
    public bool isFlipped = false;
    Vector3 flipped = new Vector3(-1f, 1f, 1f);
    Vector3 notFlipped = new Vector3(1f, 1f, 1f);

    // Start is called before the first frame update
    private void Start()
    {
        // get the game manager
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // set the current health to the max health
        currentHealth = health;

        // sets the health bar to its maximum value
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        // gets player input for vertical and horizontal movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // checks to see if the speed is set to 0 or not
        if(!isSpeed)
        {
            StartCoroutine(SpeedCheck());

            // set the isSpeed bool to true
            isSpeed = true;
        }

        // check to see if the player's health has dropped to or below zero
        if (currentHealth <= 0 && isKnockback == false)
        {
            // set the animator bool for being defeated to true
            animator.SetBool("IsDefeated", true);

            // set the velocity of the players rigidbody to zero
            rb.velocity = Vector2.zero;

            Debug.Log("Player has been defeated");

            // wait a few seconds before setting the lose screen active
            StartCoroutine(EndLoseGame());

            // disable the player script so they can't move while defeated after knockback has been set to false
            this.enabled = false;
        }

        // check to see if the boss is defeated
        if(boss.gameObject == null)
        {
            return;
        }
        else if (boss.GetComponent<Animator>().GetBool("IsDefeated"))
        {
            // set the player to their victory pose
            animator.SetBool("IsBossDefeated", true);

            // set the velocity of the player to zero
            rb.velocity = Vector2.zero;

            // wait a few secconds for the boss animation to end
            StartCoroutine(EndWinGame());
        }

    }

    // Fixed Update is called to update positions
    private void FixedUpdate()
    {
        if (boss.gameObject != null)
        {
            if (!isKnockback)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && (movement.x != 0 || movement.y != 0))
                {
                    // set the dash boolean to true
                    isDashing = true;

                    // set the current dash time to the starting dash time
                    currentDashTime = startDashTime;

                    // set the velocity of the player to zero
                    rb.velocity = Vector2.zero;

                    dashDirection = movement;

                    // play the animation
                    animator.SetTrigger("Dash");


                }
                else
                {
                    // move the character based on their movement vector
                    rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                }


                // play the animation for the movement based on the player speed
                if (movement.x != 0 || movement.y != 0)
                {
                    animator.SetFloat("Speed", speed);
                }
                if (movement.x == 0 && movement.y == 0)
                {
                    animator.SetFloat("Speed", 0);
                }

                // dashing 
                if (isDashing)
                {
                    // add a velocity to the rigid body of the player
                    rb.velocity = transform.right * dashDirection * dashSpeed;

                    // decrement the current dash time until it reaches zero
                    currentDashTime -= Time.deltaTime;

                    // set isDashing to false when the current dash time reaches zero
                    if (currentDashTime <= 0)
                    {
                        isDashing = false;
                    }
                }
            }
            // knockback
            else if (isKnockback)
            {
                // checks to make sure that the direction is only changed from the point of being hit
                if (bossXDirection == 100000)
                {
                    // get the x and y coordinates of the boss when the player is attacked
                    bossXDirection = boss.GetComponent<Rigidbody2D>().transform.position.x;
                    bossYDirection = boss.GetComponent<Rigidbody2D>().transform.position.y;
                }

                // calculate the direction that the player will be knocked back based on their position and the enemy position
                if (bossXDirection >= rb.position.x)
                {
                    knockbackDirection.x = -1f;
                }
                else
                {
                    knockbackDirection.x = 1f;
                }

                if (bossYDirection >= rb.position.y)
                {
                    knockbackDirection.y = -1f;
                }
                else
                {
                    knockbackDirection.y = 1f;
                }

                // add a velocity to the rigid body of the player
                rb.velocity = knockbackDirection * knockbackSpeed * Time.deltaTime;

                // decrement the current knockback timer
                currentKnockback -= Time.deltaTime;

                // set isKnockback to false once the timer reaches zero
                if (currentKnockback <= 0)
                {
                    isKnockback = false;

                    // set the boss positions back to their original values
                    bossXDirection = 100000f;
                    bossYDirection = 100000f;

                    // set the isDamaged boolean in the animator to false
                    animator.SetBool("IsDamaged", false);
                }
            }

            // flip character to face left if movement.x is less than 0
            if (movement.x < 0)
            {
                transform.localScale = flipped;
                isFlipped = true;
            }
            if (movement.x > 0)
            {
                transform.localScale = notFlipped;
                isFlipped = false;
            }

            // flip idle if the character was previously flipped
            if (isFlipped)
            {
                transform.localScale = flipped;
            }
            if (!isFlipped)
            {
                transform.localScale = notFlipped;
            }
        }
        
        
    }
    
    // Method: Damaged
    // Purpose: Reduce the health of the player when they are hit by the enemy
    // Restrictions: None
    public void Damaged(int damage)
    {
        // play the damaged animation
        animator.SetBool("IsDamaged", true);

        // decrease the player health by the damage they take
        currentHealth -= damage;

        // set the health bar to the current health
        healthBar.SetHealth(currentHealth);

        // set the knockback boolean to true
        isKnockback = true;

        // set the current knockback time to the start knockback time
        currentKnockback = startKnockback;
    }

    IEnumerator SpeedCheck()
    {
        // wait for 0.45 second
        yield return new WaitForSeconds(0.45f);

        // set the speed back to 5f
        speed = 5f;

        animator.ResetTrigger("Dash");
    }

    IEnumerator EndLoseGame()
    {
        // wait for the boss animation to end
        yield return new WaitForSeconds(3f);

        // transition to the lose screen
        gameManager.loseScreen.SetActive(true);

        // enable the final score
        gameManager.finalScore.enabled = true;
    }

    IEnumerator EndWinGame()
    {
        // wait for the boss animation to end
        yield return new WaitForSeconds(3f);

        // transition to the win screen
        gameManager.winScreen.SetActive(true);

        // enable the final score
        gameManager.finalScore.enabled = true;
    }
}
