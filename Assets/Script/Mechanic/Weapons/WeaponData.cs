using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    [CreateAssetMenu(fileName = "Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;
        public Sprite icon;
        public bool hasProjectile;
        public GameObject projectilePrefab;
        public WeaponLevel[] levels;
    }


    /*[CustomEditor(typeof(WeaponData))]
    public class WeaponDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Reference to the target scriptable object
            WeaponData weaponData = (WeaponData)target;

            // Draw default fields
            weaponData.weaponName = EditorGUILayout.TextField("Weapon Name", weaponData.weaponName);
            weaponData.icon = (Sprite)EditorGUILayout.ObjectField("Icon", weaponData.icon, typeof(Sprite), false);
            weaponData.hasProjectile = EditorGUILayout.Toggle("Has Projectile", weaponData.hasProjectile);

            // Conditionally show the projectilePrefab field
            if (weaponData.hasProjectile)
            {
                weaponData.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", weaponData.projectilePrefab, typeof(GameObject), false);
            }

            // Draw levels array
            SerializedProperty levels = serializedObject.FindProperty("levels");
            EditorGUILayout.PropertyField(levels, new GUIContent("Levels"), true);

            // Apply changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }*/

}

