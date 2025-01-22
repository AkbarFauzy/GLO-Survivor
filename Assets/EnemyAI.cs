using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Survivor.Character.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Pathfinding")]
        public Transform target;
        public float activateDistance = 50f;
        public float pathUpdateSeconds = 0.5f;

        [Header("Physics")]
        public float speed = 200f, jumpForce = 100f;
        public float nextWaypointDistance = 3f;
        public float jumpNodeHeightRequirement = 0.8f;

        [Header("Custom Behavior")]
        public bool followEnabled = true;
        public bool jumpEnabled = true, isJumping, isInAir;
        public bool directionLookEnabled = true;
        public Transform enemyGFX;

        [Header("Checker")]
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
        [SerializeField] private LayerMask _groundLayer;

        private Path path;
        private int currentWaypoint = 0;
        private Vector2 currentVelocity;
        private bool isGrounded;
        Seeker seeker;
        Rigidbody2D rb;
        private bool isOnCoolDown;

        public void Start()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
            isJumping = false;
            isInAir = false;
            isOnCoolDown = false;

            InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        }

        private void FixedUpdate()
        {
            if (TargetInDistance() && followEnabled)
            {
                PathFollow();
            }
        }

        private void UpdatePath()
        {
            if (followEnabled && TargetInDistance() && seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }

        private void PathFollow()
        {
            if (path == null)
            {
                return;
            }

            // Reached end of path
            if (currentWaypoint >= path.vectorPath.Count)
            {
                return;
            }

            // See if colliding with anything
            isGrounded = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);

            // Direction Calculation
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed;

            // Jump
            if (jumpEnabled && isGrounded && !isInAir && !isOnCoolDown)
            {
                if (direction.y > jumpNodeHeightRequirement)
                {
                    if (isInAir) return;
                    isJumping = true;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    StartCoroutine(JumpCoolDown());

                }
            }
            if (isGrounded)
            {
                isJumping = false;
                isInAir = false;
            }
            else
            {
                isInAir = true;
            }

            // Movement
            rb.velocity = Vector2.SmoothDamp(rb.velocity, force, ref currentVelocity, 0.5f);

            // Next Waypoint
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            // Direction Graphics Handling
            if (directionLookEnabled)
            {
                if (rb.velocity.x >= 0.01f)
                {
                    enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
                }
                else if (rb.velocity.x <= -0.01f)
                {
                    enemyGFX.localScale = new Vector3(1f, 1f, 1f);
                }
            }
        }

        private bool TargetInDistance()
        {
            return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        IEnumerator JumpCoolDown()
        {
            isOnCoolDown = true;
            yield return new WaitForSeconds(1f);
            isOnCoolDown = false;
        }
    }
}

