using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TODO (aoyeola): Unserialize private fields once ranges are finalized;
    [SerializeField]
    public GameObject target;
    
    [HideInInspector]
    public Vector2 lookVector;

    private Camera regularCamera;
    private Vector2 cameraOrbitAngle;
    private Vector3 focusPoint;
    private Quaternion lookRotation;
    

    [SerializeField, Range(1f, 20f)]
    private float cameraDistance = 8f;

    [SerializeField, Range(1f, 25f)]
    private float cameraPanRadius = 2f;

    [SerializeField, Min(0f)]
    private float cameraResetDelay = 5f;

    [SerializeField, Range(0f, 1f)]
    private float cameraResetSpeed = 0.75f;

    [SerializeField, Range(1f, 100f)]
    private float cameraRotateSpeed = 30f;

    [SerializeField, Range(-15f, 80f)]
    private float cameraMinYAngle = -5f, cameraMaxYAngle = 45f;

    [SerializeField, Range(0f, 1f)]
    private float cameraSensitivity = 0.4f;

    // NOTE (aoyeola): Useful for locking camera pos during cinematic events
    [SerializeField]
    private bool canMove = true;

    // TODO (aoyeola): Expose these flags in an Options menu
    [SerializeField]
    private bool invertXAxis, invertYAxis;

    [SerializeField]
    private LayerMask clippingMask = -1;

    private Transform obstruction;
    private float zoomSpeed = 2f;

    public Vector3 CameraHalf
    {
        get
        {
            Vector3 halfExtends;
            halfExtends.y = regularCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
            halfExtends.x = halfExtends.y * regularCamera.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }

    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("Target gameobject must be set to Player!");
        }

        cameraOrbitAngle = new Vector2(45f, 0f);
        lookVector = Vector2.zero;
        transform.localRotation = Quaternion.Euler(cameraOrbitAngle);
        regularCamera = GetComponent<Camera>();

        // NOTE (aoyeola): Only for debugging/playtesting
        // Forced Pokemon style stationary camera
        if (canMove.Equals(false))
        {
            cameraDistance = 8f;
            cameraPanRadius = 2f;
            cameraResetSpeed = 0.9f;
        }

        #if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        #endif
        
        focusPoint = target.transform.position;
        obstruction = target.transform;

        // Ignore camera raycasts on Player layer
        clippingMask = clippingMask ^ LayerMask.GetMask("Player");
    }

    private void LateUpdate()
    {

        UpdateCameraInput();
        UpdateCameraTracking();
        //ViewObstructed();
        
        Vector3 lookDir = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDir * cameraDistance;

        Vector3 rectOffset = lookDir * regularCamera.nearClipPlane;
        Vector3 rectPosition = lookPosition + rectOffset;
        Vector3 castFrom = target.transform.position;
        Vector3 castLine = rectPosition - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;

        // camera collision - cast a box from camera to the target. If a raycast hit is detected, 
        // reposition the camera within distance to the near clipping plane
        if (Physics.BoxCast(focusPoint, CameraHalf, castDirection, out RaycastHit hitinfo, lookRotation, castDistance, clippingMask))
        {
            rectPosition = castFrom + castDirection * hitinfo.distance;
            lookPosition = rectPosition - rectOffset;
        }
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    private void OnValidate()
    {
        if (cameraMaxYAngle < cameraMinYAngle)
        {
            cameraMaxYAngle = cameraMinYAngle;
        }
    }

    private void UpdateCameraTracking()
    {
        Vector3 targetPosition = target.transform.position;

        if (cameraPanRadius > 0f)
        {
            // Recenter the camera on target logarithmically (instead of linearly)
            // s.t. distance is halved each time. Default is 0.5 but can be
            // any arbitrary scalar.
            float targetDist = Vector3.Distance(targetPosition, focusPoint);
            float t = 1f;
            if ((targetDist >= 0.01f) && (cameraResetSpeed > 0f))
            {
                t = Mathf.Pow(1f - cameraResetSpeed, Time.unscaledDeltaTime);
            }

            if (targetDist > cameraPanRadius)
            {
                t = Mathf.Min(t, cameraPanRadius / targetDist);
            }

            focusPoint = Vector3.Lerp(targetPosition, focusPoint, t);
        } 
        else
        {
            focusPoint = targetPosition;
        }
    }

    private void UpdateCameraInput()
    {
        if (canMove)
        {
            CheckXAndYAxisInversion();

            if (ManualRotation()) {

                // constrain orbit angles
                cameraOrbitAngle.x = Mathf.Clamp(cameraOrbitAngle.x, cameraMinYAngle, cameraMaxYAngle);
                if (cameraOrbitAngle.y < 0f)
                {
                    cameraOrbitAngle.y += 360f;
                }
                else if (cameraOrbitAngle.y >= 360f)
                {
                    cameraOrbitAngle.y -= 360f;
                }

                lookRotation = Quaternion.Euler(cameraOrbitAngle);
            } 
            else
            {
                lookRotation = transform.localRotation;
            }
        }
    }

    private bool ManualRotation()
    {
        // Flip x and y values for lookVector so lookVector.x defines the 
        // vertical orientation along the Y axis (e.g. tilt), and lookVector.y defines the 
        // horizontal orientation along the Z axis (e.g. pan)

        Vector2 camInput;
        camInput.x = lookVector.y * cameraSensitivity;
        camInput.y = lookVector.x * cameraSensitivity;

        // TODO (aoyeola): Make proper right stick deadzone values
        const float dz = 0.001f;
        if (camInput.x < -dz || camInput.x > dz || camInput.y < -dz || camInput.y > dz)
        {
            cameraOrbitAngle += cameraRotateSpeed * Time.unscaledDeltaTime * camInput;
            return true;
        }
        return false;
    }

    private void CheckXAndYAxisInversion()
    {
        if (invertYAxis)
        {
            lookVector.y *= -1;
        }
        if (invertXAxis)
        {
            lookVector.x *= -1;
        }
    }
    
    private void ViewObstructed()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, 4.5f))
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                obstruction = hit.transform;
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                if ((Vector3.Distance(obstruction.position, transform.position) >= 3f)
                  && ( Vector3.Distance(transform.position, target.transform.position) >= 1.5f))
                {
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
                }
                else
                {
                    MeshRenderer meshRenderer = obstruction.gameObject.GetComponent<MeshRenderer>();
                    meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    if (Vector3.Distance(transform.position, target.transform.position) < 4.5f)
                    {
                        transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                    }
                }
            }
        }
    }
}
