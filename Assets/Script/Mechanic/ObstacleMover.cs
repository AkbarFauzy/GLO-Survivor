using UnityEngine;

namespace Survivor.Mechanic
{
    public class ObstacleMover : MonoBehaviour
    {
        private Vector3 startPoint;
        private Vector3 endPoint;
        private float moveDuration;
        private float elapsedTime;

        public void Initialize(Vector3 start, Vector3 end, float duration)
        {
            startPoint = start;
            endPoint = end;
            moveDuration = duration;
            elapsedTime = 0f;
        }

        private void Update()
        {
            if (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / moveDuration;

                // Interpolate position between start and end
                transform.position = Vector3.Lerp(startPoint, endPoint, t);
            }
            else
            {
                // Destroy the obstacle once it reaches the endpoint
                Destroy(gameObject);
            }
        }
    }
}
