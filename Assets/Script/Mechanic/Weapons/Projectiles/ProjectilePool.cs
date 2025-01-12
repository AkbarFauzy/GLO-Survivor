using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    public class ProjectilePool : MonoBehaviour
    {
        private List<GameObject> pool = new List<GameObject>();
        private GameObject projectilePrefab;

        public int Count { get => pool.Count; }

        public void Initialize(Weapon originalWeapon,GameObject prefab, int initialSize)
        {
            projectilePrefab = prefab;
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(projectilePrefab);
                obj.GetComponent<Projectile>().SetPool(this);
                obj.GetComponent<Projectile>().AddObserver(originalWeapon);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        public GameObject GetProjectile()
        {
            // Find an inactive projectile in the pool
            foreach (GameObject projectile in pool)
            {
                if (!projectile.activeInHierarchy)
                {
                    return projectile;
                }
            }

            return null;
        }

        public void ReturnProjectile(GameObject obj)
        {
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

}
