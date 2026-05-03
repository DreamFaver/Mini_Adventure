using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // Your player's transform
    public float followSpeed = 5f; // How quickly the camera catches up
    public Vector2Int referenceResolution = new Vector2Int(320, 180); // Your game's reference resolution
    public int pixelsPerUnit = 16; // Your pixel art's PPU
    
    private float halfPixelWidth;
    private float halfPixelHeight;

    void Start()
    {
        // Calculate the size of a single pixel on the screen in world units
        CalculatePixelSizes();
    }

    void CalculatePixelSizes()
    {
        // This calculation ensures that one pixel on the reference resolution
        // maps directly to a whole number of units in the game world.
        // We use halfWidth/Height because we'll be snapping to the center of pixels.
        halfPixelWidth = pixelsPerUnit * 0.5f;
        halfPixelHeight = pixelsPerUnit * 0.5f;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("PixelPerfectCameraFollow: Target is not assigned!");
            return;
        }

        // --- Smooth Following ---
        // Calculate desired camera position based on target and offset
        Vector3 desiredPosition = target.position + (Vector3)offset; // Use (Vector3)offset for clarity

        // Smoothly interpolate towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // --- Pixel Snapping ---
        // Snap the smoothed position to the nearest whole pixel grid
        smoothedPosition = SnapToPixel(smoothedPosition);

        // Ensure Z position remains constant (for 2D)
        smoothedPosition.z = transform.position.z;

        // Apply the final snapped and smoothed position
        transform.position = smoothedPosition;
    }

    // Snaps a world position to the nearest pixel grid
    Vector3 SnapToPixel(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / halfPixelWidth) * halfPixelWidth;
        float snappedY = Mathf.Round(position.y / halfPixelHeight) * halfPixelHeight;

        return new Vector3(snappedX, snappedY, position.z);
    }

    // Public offset for easy adjustment in the inspector
    public Vector3 offset = new Vector3(0f, 0.5f, -10f); // Default offset, adjust as needed
}
