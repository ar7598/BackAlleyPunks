using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: ProjectileFire
// Author: Alexander Zotov with additions by Ajay Ramnarine
// Purpose: Set the angles and directions to fire the projectiles
//          The projectiles are gotten from the pool and set to active to be fired off in a circle
// Restrictions: Fires only in a circular shape, no distinct pattern
//               The pool of projectiles is created called from the ProjectilePool script
public class ProjectileFire : MonoBehaviour
{
    [SerializeField]
    private int projectileAmount = 10;

    [SerializeField]
    private float startAngle = 0f;

    [SerializeField]
    private float endAngle = 359f;

    private Vector2 projMoveDirection;

    Animator bossAnim;

    // Start is called before the first frame update
    void Start()
    {
        bossAnim = GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>();

        // projectiles will be fired at a rate of every 2 seconds
        InvokeRepeating("Fire", 1f, 2f);
    }
    
    private void Fire()
    {
        if (bossAnim.GetBool("IsFiring"))
        {
            // calculates the value to evenly distribute how the projectiles are fired
            float angleStep = (endAngle - startAngle) / projectileAmount;
            float angle = startAngle;

            // create a loop that will calculate the coordinates for each projectile
            // increase the projectile amount by 1 to make sure the last projectile fires in the opposite direction of the first one
            for (int i = 0; i < projectileAmount + 1; i++)
            {
                // calculate the x and y coordinates of end point
                float projDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float projDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                // get the direction vector for the projectile to travel
                Vector3 projMoveDirection = new Vector3(projDirX, projDirY, 0f);

                // normalize the direction that the projectile will travel
                Vector2 projDirection = (projMoveDirection - transform.position).normalized;

                // get a projectile from the pool
                GameObject proj = ProjectilePool.projectilePoolInstance.GetProjectiles();

                // set the position and rotation of the projectile from the pool 
                proj.transform.position = transform.position;
                proj.transform.rotation = transform.rotation;

                // set the projectile to be active
                proj.SetActive(true);

                // set the direction of the projectile to travel
                proj.GetComponent<Projectile>().SetMoveDirection(projDirection);

                // increase the angle by the step for the next projectile in the list
                angle += angleStep;
            }
        }
    }
}
