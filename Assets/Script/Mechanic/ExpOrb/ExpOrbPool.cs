using System.Collections;
using System.Collections.Generic;
using Survivor.Character.Enemies;
using Survivor.Character.Player;
using Survivor.Manager;
using UnityEngine;


namespace Survivor.Mechanic {
    public class ExpOrbPool : Observer
    {
        public GameObject ExpOrbPrefab;
        [SerializeField] private int _poolSize = 500;
        private List<GameObject> _expOrbPool = new List<GameObject>();

        private Player _player;

        private void Start()
        {
            _player = FindFirstObjectByType<Player>();
            GameManager.Instance.AddObserver(this);


            for (int i = 0; i < _poolSize; i++)
            {
                GameObject expOrbObject = Instantiate(ExpOrbPrefab, transform);
                expOrbObject.SetActive(false);
                _expOrbPool.Add(expOrbObject);
                ExpOrb expOrb = expOrbObject.GetComponent<ExpOrb>();
                expOrb.AddObserver(this);
                expOrb.AddObserver(_player);
            }

            Subscribe<Enemy>(Events.EnemyDied, SpawnExpOrbAtEnemy);
            Subscribe<ExpOrb>(Events.ExpOrbPickedUp, AddExpOrbToPool);
            Subscribe<Vector2>(Events.SpawnExpOrb, SpawnExpOrb);
            
        }

        private void OnDestroy()
        {
            Unsubscribe<Enemy>(Events.EnemyDied, SpawnExpOrbAtEnemy);
            Unsubscribe<ExpOrb>(Events.ExpOrbPickedUp, AddExpOrbToPool);
            Unsubscribe<Vector2>(Events.SpawnExpOrb, SpawnExpOrb);
        }


        private void SpawnExpOrbAtEnemy(Enemy enemy)
        {
            var location = enemy.transform.position;
            _expOrbPool[0].SetActive(true);
            _expOrbPool[0].transform.position = location;
            _expOrbPool.RemoveAt(0);
        }

        private void SpawnExpOrb(Vector2 location) {
            _expOrbPool[0].SetActive(true);
            _expOrbPool[0].transform.position = location;
            _expOrbPool.RemoveAt(0);
        }

        private void AddExpOrbToPool(ExpOrb expOrb) {
            expOrb.gameObject.SetActive(false);
            _expOrbPool.Add(expOrb.gameObject);
        }

    }
}
