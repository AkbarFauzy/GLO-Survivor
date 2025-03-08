using TwoBitMachines.FlareEngine;
using UnityEngine;

namespace Survivor.Mechanic.Weapons {
    [CreateAssetMenu(fileName = "Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private string weaponName;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private WeaponLevel[] levels;

        public string WeaponName => weaponName;
        public Sprite Icon => icon;
        public GameObject ProjectilePrefab => projectilePrefab;
        public WeaponLevel[] Levels => levels;
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

