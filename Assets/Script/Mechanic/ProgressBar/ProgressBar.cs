using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Mechanic {
    using UnityEngine;

    public class ProgressBar : MonoBehaviour
    {
        public float warningDuration = 1f;    // Duration of the warning
        private Transform progressBar;       // The progress bar Transform
        private Vector3 initialScale;        // Original scale of the progress bar

        private void Start()
        {
            // Find the ProgressBar child
            progressBar = transform.Find("ProgressBar");

            if (progressBar == null)
            {
                Debug.LogError("ProgressBar child object not found!");
                return;
            }

            // Store the initial scale of the progress bar
            initialScale = progressBar.localScale;

            // Start the progress animation
            StartCoroutine(UpdateProgressBar());
        }

        private IEnumerator UpdateProgressBar()
        {
            float elapsedTime = 0f;

            while (elapsedTime < warningDuration)
            {
                // Calculate the progress based on elapsed time
                float progress = elapsedTime / warningDuration;

                // Update the scale of the progress bar
                progressBar.localScale = new Vector3(initialScale.x * (1 - progress), initialScale.y, initialScale.z);

                // Wait for the next frame
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the bar is fully depleted at the end
            progressBar.localScale = new Vector3(0, initialScale.y, initialScale.z);

            // Destroy the warning indicator once the duration is complete
            Destroy(gameObject);
        }
    }
}
