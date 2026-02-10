using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    /// <summary> Read-only reference to the minimap camera's transform. </summary>
    public Transform CameraTransform => cameraTransform;

    void Start()
    {
        InvokeRepeating(nameof(RefreshMinimap), 0.0f, 1 / 60.0f);
    }

    void RefreshMinimap()
    {
        RTImage(cameraTransform.GetComponent<Camera>());
    }

    Texture2D RTImage(Camera camera)
    {
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        camera.Render();

        Texture2D image = new(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = currentRT;
        return image;
    }

    public void EnableCamera(bool enable)
    {
        if (cameraTransform != null)
            cameraTransform.gameObject.SetActive(enable);
    }
}
