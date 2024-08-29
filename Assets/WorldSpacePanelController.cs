using UnityEngine;
using UnityEngine.XR;

public class WorldSpacePanelController : MonoBehaviour
{
    public Transform vrCamera;
    public float smoothness = 5f;
    public float viewAngleThreshold = 30f;
    public float distanceFromCamera = 2f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        if (vrCamera == null)
        {
            Debug.LogError("VR Camera reference is missing!");
            return;
        }

        // Initialize the panel position in front of the camera
        UpdateTargetTransform();
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    void Update()
    {
        if (vrCamera == null) return;

        // Calculate the angle between the camera's forward and the direction to the panel
        Vector3 toPanelDirection = transform.position - vrCamera.position;
        float angle = Vector3.Angle(vrCamera.forward, toPanelDirection);

        // If the angle is too large, update the target position and rotation
        if (angle > viewAngleThreshold)
        {
            UpdateTargetTransform();
        }

        // Smoothly move and rotate the panel towards the target
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothness);
    }

    void UpdateTargetTransform()
    {
        // Project the camera's forward vector onto the horizontal plane
        Vector3 cameraForwardHorizontal = Vector3.ProjectOnPlane(vrCamera.forward, Vector3.up).normalized;

        // Calculate the target position in front of the camera
        targetPosition = vrCamera.position + cameraForwardHorizontal * distanceFromCamera;

        // Maintain the original height
        targetPosition.y = transform.position.y;

        // Calculate the target rotation to face the camera
        targetRotation = Quaternion.LookRotation(targetPosition - vrCamera.position, Vector3.up);
    }
}