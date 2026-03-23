using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region Config

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float brakeForce = 10f;

    [Header("Physics")]
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airControlMultiplier = 0.4f;

    [SerializeField] private Transform cameraTransform;

    #endregion

    #region Dependencies

    private Rigidbody rb;
    private IInputProvider inputProvider;

    #endregion

    #region State

    private Vector3 currentVelocity;
    private bool isGrounded;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Dependency injection (can replace later)
        inputProvider = new DefaultInputProvider();
    }

    private void FixedUpdate()
    {
        UpdateGroundState();
        HandleMovement();
        ApplyDrag();
    }

    #endregion

    #region Core Logic

    private void HandleMovement()
    {
        Vector2 input = inputProvider.GetMovementInput();
        // Break System
        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, brakeForce * Time.fixedDeltaTime);
            return;
        }

        Vector3 desiredDirection = new Vector3(input.x, 0f, input.y).normalized;

        if (desiredDirection.sqrMagnitude < 0.01f)
            return;

        Vector3 targetVelocity = desiredDirection * moveSpeed;

        float controlMultiplier = isGrounded ? 1f : airControlMultiplier;

        Vector3 velocityChange = (targetVelocity - rb.linearVelocity) * acceleration * controlMultiplier * Time.fixedDeltaTime;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        RotateTowards(desiredDirection);
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion smoothedRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothedRotation);
    }

    private void ApplyDrag()
    {
        rb.linearDamping = isGrounded ? groundDrag : 0f;
    }

    private void UpdateGroundState()
    {
        // Simple ground check (replace with better system if needed)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    #endregion
}

#region Input Abstraction

public interface IInputProvider
{
    Vector2 GetMovementInput();
}

public class DefaultInputProvider : IInputProvider
{
    public Vector2 GetMovementInput()
    {
        return new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );
    }
}

#endregion