using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Player;
using UnityEngine;

namespace Survivor.Mechanic.Weapons
{
    public class EnemyProjectile : Subject
    {
        private float _lifetime = 5f;
        private float _lifeTimer = 0f;

        private void OnEnable()
        {
            _lifeTimer = 0f;
        }

        protected virtual void Update()
        {
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= _lifetime){
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                NotifyEvents(Events.PlayerTakeDamage);
                gameObject.SetActive(false);
            }
        }
    }
}
