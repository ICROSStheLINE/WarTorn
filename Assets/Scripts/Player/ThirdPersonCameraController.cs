using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float sensitivity = 10f;
    [SerializeField, Range(0, 1)] private float rotationSmoothing = 0.2f;
    [SerializeField, Range(0, 1)] private float followingSmoothing = 0.2f;
    [SerializeField] private float distance = 3f;
    [SerializeField] private Vector3 cameraOffset = Vector3.zero;
    [SerializeField] private float pitchMin = -50f;
    [SerializeField] private float pitchMax = 50f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    public Transform CameraTransform => cameraTransform;
    [SerializeField] private Transform playerTransform;

    private InputAction lookAction;

    // Runtime
    private float yaw = 0f;
    private float pitch = 0f;
    private Quaternion targetRotation;

    public Quaternion Rotation => targetRotation; // read-only for other classes

    void Awake()
    {
        // Detach from parent
        transform.parent = null;

        // Initialize rotation
        targetRotation = transform.rotation;

        // Get Look Input Action (ensure it exists)
        lookAction = InputSystem.actions.FindAction("Look");
        if (lookAction == null)
            Debug.LogWarning("Look action not found in InputSystem");
    }

    void LateUpdate()
    {
        if (lookAction == null) return;

        HandleCameraRotation();
        HandleCameraPosition();
    }

    private void HandleCameraRotation()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime; // invert if needed
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        targetRotation = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothing);
    }

    private void HandleCameraPosition()
    {
        if (cameraTransform == null || playerTransform == null) return;

        Vector3 desiredPosition = playerTransform.position;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followingSmoothing);

        cameraTransform.localPosition = cameraOffset + new Vector3(0, 0, -distance);
    }

    public void EnableCamera(bool enable)
    {
        if (cameraTransform != null)
            cameraTransform.gameObject.SetActive(enable);
    }
}
