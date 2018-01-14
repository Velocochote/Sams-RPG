﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.Core;

namespace RPG.Characters {
	public class Enemy : MonoBehaviour, IDamageable { // TODO Remove interface

	    [SerializeField] float chaseRadius = 6f;

	    [SerializeField] float attackRadius = 4f;
	    [SerializeField] float damagePerShot = 9f;
	    [SerializeField] float secondsBetweenShots = 0.5f;
		[SerializeField] float variationBetweenShots = 0.1f;
	    [SerializeField] GameObject projectileToUse;
	    [SerializeField] GameObject projectileSocket;
	    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

	    bool isAttacking = false;
		Player player = null;

	    void Start()
	    {
			player = FindObjectOfType<Player> ();
	    }

		public void TakeDamage(float amount) { // TODO Remove
		}

	    void Update()
	    {
	        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
	        if (distanceToPlayer <= attackRadius && !isAttacking)
	        {
	            isAttacking = true;
				float randomDelay = Random.Range (secondsBetweenShots - variationBetweenShots, secondsBetweenShots + variationBetweenShots);
				InvokeRepeating("SpawnProjectile", 0f, randomDelay); // TODO switch to coroutines
	        }
	        
	        if (distanceToPlayer > attackRadius)
	        {
	            isAttacking = false;
	            CancelInvoke();
	        }

	        if (distanceToPlayer <= chaseRadius)
	        {
	            // aiCharacterControl.SetTarget(player.transform);
	        }
	        else
	        {
	        	// aiCharacterControl.SetTarget(transform);
	        }
	    }

	    void SpawnProjectile() // TODO Seperate charachter firing logic
	    {
	        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
	        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
	        projectileComponent.SetDamage (damagePerShot);
			projectileComponent.SetShooter (gameObject);

	        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
			float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
	        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
	    }

	    void OnDrawGizmos()
	    {
	        // Draw attack sphere 
	        Gizmos.color = new Color(255f, 0, 0, .5f);
	        Gizmos.DrawWireSphere(transform.position, attackRadius);

	        // Draw chase sphere 
	        Gizmos.color = new Color(0, 0, 255, .5f);
	        Gizmos.DrawWireSphere(transform.position, chaseRadius);
	    }
	}
}