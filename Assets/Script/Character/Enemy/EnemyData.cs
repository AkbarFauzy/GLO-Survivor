using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Character.Enemies {
    [CreateAssetMenu(menuName = "Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public float Speed;
        public float Health;
        public float Damage;
        public float JumpForce = 10f;
    }
}

