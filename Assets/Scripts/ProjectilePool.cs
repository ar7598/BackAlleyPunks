using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: ProjectilePool
// Author: Alexander Zotov with additions made by Ajay Ramnarine
// Purpose: Creates a pool (list) of projectiles to be stored and fired
// Restrictions: The projectiles stored in the pool are of the Projectile script
//               The projectiles wihthin the pool are fired using the ProjectileFire script
public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool projectilePoolInstance;

    [SerializeField]
    private GameObject pooledProjectile;
    private bool lowAmmo = true;

    public List<GameObject> projectiles;

    private void Awake()
    {
        // assigns the instance of the projectile pool to "this" game object
        projectilePoolInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // initialize the list of projectiles to fill with projectiles
        projectiles = new List<GameObject>();
    }

    // Method: GetProjectiles
    // Purpose: Is invoked when the boss is firing projectiles when enraged
    // Restrictions: None
    public GameObject GetProjectiles()
    {
        // check to see if there are any projectiles in the pool
        if(projectiles.Count > 0)
        {
            for(int i = 0; i < projectiles.Count; i++)
            {
                // finds a projectile that is currently inactive and returns it
                if (!projectiles[i].activeInHierarchy && projectiles[i] != null)
                {
                    return projectiles[i];
                }

                /*
                if(projectiles[i] == null)
                {
                    // instantiates a new projectile
                    GameObject proj = Instantiate(pooledProjectile);

                    // set the projectile to inactive
                    proj.SetActive(false);

                    // adds the instantiated projectile to the pool
                    projectiles[i] = proj;

                    // returns the new projectile
                    return proj;
                }
                */
            }
        }

        // checks to see if there are no projectiles currently in the pool
        if (lowAmmo)
        {
            // instantiates a new projectile
            GameObject proj = Instantiate(pooledProjectile);

            // set the projectile to inactive
            proj.SetActive(false);

            // adds the instantiated projectile to the pool
            projectiles.Add(proj);

            // returns the new projectile
            return proj;
        }

        return null;
    }
}
