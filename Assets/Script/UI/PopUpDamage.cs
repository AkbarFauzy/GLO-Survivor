using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Survivor.Mechanic.UI {
    public class PopUpDamage : MonoBehaviour
    {
        public TextMeshPro textMesh; // Reference to TextMeshPro component
        public float lifetime = 1f; // Time before returning to the pool
        public float moveSpeed = 2f; // Speed at which the popup moves upward
        public float fadeSpeed = 2f; // Speed at which the popup fades out

        private Color originalColor;
        private System.Action<GameObject> returnToPoolCallback;

        private void Awake()
        {
            if (textMesh != null)
            {
                originalColor = textMesh.color;
            }
        }

        private void OnEnable()
        {
            if (textMesh != null)
            {
                textMesh.color = originalColor;
            }
            Invoke(nameof(ReturnToPool), lifetime);
        }

        private void Update()
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            if (textMesh != null)
            {
                Color color = textMesh.color;
                color.a -= fadeSpeed * Time.deltaTime;
                textMesh.color = color;
            }
        }

        public void Setup(float damage, System.Action<GameObject> returnToPool)
        {
            if (textMesh != null)
            {
                textMesh.text = Mathf.CeilToInt(damage).ToString();
            }

            returnToPoolCallback = returnToPool;
        }

        private void ReturnToPool()
        {
            returnToPoolCallback?.Invoke(gameObject);
            CancelInvoke();
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}

