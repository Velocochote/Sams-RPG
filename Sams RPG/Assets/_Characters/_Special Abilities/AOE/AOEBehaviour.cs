﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters {
	public class AOEBehaviour : AbilityBehaviour {
		
		public override void Use(GameObject target) {
			DealRadialDamage ();
			PlayAbilitySound ();
			PlayParticleEffect ();
		}

		private void DealRadialDamage() {
			// Static SphereCast For Targets
			RaycastHit[] hits = Physics.SphereCastAll (
				transform.position, 
				(config as AOEConfig).GetRadius (), 
				Vector3.up, 
				(config as AOEConfig).GetRadius ()
			);

			foreach (RaycastHit hit in hits) {
				var damageable = hit.collider.gameObject.GetComponent<HealthSystem> ();
				bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerMovement> ();
				if (damageable != null && !hitPlayer) {
					float damageToDeal = (config as AOEConfig).GetDamageToEachTarget ();
					damageable.TakeDamage (damageToDeal);
				}
			}
		}
	}
}