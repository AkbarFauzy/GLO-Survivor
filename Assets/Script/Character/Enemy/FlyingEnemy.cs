using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Survivor.Character.Enemies {
    public class FlyingEnemy : Enemy
    {
        protected override void FixedUpdate()
        {
            if (_player == null) return;

            FlyTowardPlayer();
        }

        protected virtual void FlyTowardPlayer()
        {
            Vector2 direction = (_player.position - transform.position).normalized;

            if ((direction.x > 0 && !IsFacingRight) || (direction.x < 0 && IsFacingRight))
            {
                Turn();
            }
            _rb.velocity = direction * Speed;
        }
    }
}

