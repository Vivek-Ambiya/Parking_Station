using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -8f);
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        FollowWithRotation();
    }

    private void FollowWithRotation()
    {
        Vector3 rotatedOffset = target.rotation * offset;

        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smooth follow
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        // Look at player
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}