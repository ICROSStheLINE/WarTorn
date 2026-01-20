using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 10.0f;
    [SerializeField]
    [Range(0, 1)]
    private float rotationSmoothing = 0.2f;
    [Range(0, 1)]
    private float followingSmoothing = 0.2f;

    [SerializeField]
    private float pitchMin = -50.0f, pitchMax = 50.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public Quaternion rotation;

    [SerializeField]
    private float distance = 3;

    [SerializeField]
    private Vector3 cameraOffset = Vector3.zero;

    [SerializeField]
    private Transform cameraTransform;
    InputAction lookAction;
    [SerializeField]
    private Transform playerTransform;


    void Awake()
    {
        transform.parent = null;
        rotation = transform.rotation;
        lookAction = InputSystem.actions.FindAction("Look");
    }

    void LateUpdate()
    {
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        yaw += lookValue.x * sensitivity * Time.deltaTime;
        pitch += lookValue.y * sensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSmoothing);
        cameraTransform.localPosition = cameraOffset + new Vector3(0, 0, -distance);
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, followingSmoothing);
    }

    public void EnableCamera(bool enable)
    {
        cameraTransform.gameObject.SetActive(enable);
    }
}
