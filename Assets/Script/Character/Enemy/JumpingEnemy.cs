using UnityEngine;

namespace Survivor.Character.Enemies
{
    public class JumpingEnemy : Enemy
    {
        public float jumpForce = 5f; // Force applied for each jump
        public float jumpCooldown = 2f; // Time between jumps

        private float _jumpTimer = 0f;

        protected override void Update()
        {
            base.Update();
            HandleJumping();
        }

        protected override void FixedUpdate()
        {

        }

        private void HandleJumping()
        {
            if (IsJumping) return;
            // Increment the timer
            _jumpTimer += Time.deltaTime;

            // Check if enough time has passed to jump
            if (_jumpTimer >= jumpCooldown && CanJump())
            {
                Jump();
                _jumpTimer = 0f; // Reset the timer
            }
        }

/*        private void PerformJump()
        {

            if (_rb != null && Mathf.Abs(_rb.velocity.y) < 0.01f) // Jump only if on the ground
            {
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }*/
    }
}
