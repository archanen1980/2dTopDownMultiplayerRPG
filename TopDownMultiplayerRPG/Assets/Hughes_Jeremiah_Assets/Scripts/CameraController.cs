using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // The player's transform

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Camera's position relative to the player
    [Range(0, 1)] public float smoothSpeed = 0.125f; // How smoothly the camera moves
    public float cameraZoom = 10f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 desiredPosition;

    private void Awake()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("No object tagged Player found.");
            }
        }
        if (target != null)
        {
            desiredPosition = target.position + offset;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired camera position
        desiredPosition = target.position + offset;
        
        //Smoothdamp the camera
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
        Camera.main.orthographicSize = cameraZoom;
    }
}
