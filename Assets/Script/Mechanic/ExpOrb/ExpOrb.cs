using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Player;
using UnityEngine;

namespace Survivor.Mechanic {
    public class ExpOrb : Subject
    {
        public float Value = 2;

        private Transform _player;
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnEnable()
        {
            _player = null;
            _collider.isTrigger = false;
        }

        private void FixedUpdate()
        {
            if (_player == null) return;

            transform.position = Vector2.MoveTowards(transform.position, _player.position, 10f * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("ExpOrbMagnet") && _player == null)
            {
                Debug.Log("trigger magnet");
                _player = collision.gameObject.GetComponent<Transform>().parent;
                _collider.isTrigger = true;
            }

            if (collision.CompareTag("Player"))
            {
                Debug.Log("Orb Picked up");
                NotifyEvents(Events.ExpOrbPickedUp, this);   
            }
        }

    }
}
