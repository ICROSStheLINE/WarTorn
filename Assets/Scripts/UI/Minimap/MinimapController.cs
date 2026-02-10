using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    /// <summary> Read-only reference to the minimap camera's transform. </summary>
    public Transform CameraTransform => cameraTransform;

    private Camera minimapCamera;

    void Start()
    {
        minimapCamera = cameraTransform.GetComponent<Camera>();
        InvokeRepeating(nameof(RefreshMinimap), 0.0f, 1 / 30.0f);
    }

    void RefreshMinimap()
    {
        minimapCamera.Render();
    }

    public void EnableCamera(bool enable)
    {
        if (cameraTransform != null)
            cameraTransform.gameObject.SetActive(enable);
    }
}
