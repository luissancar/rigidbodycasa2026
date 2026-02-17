using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 0.15f;

    [Header("Pitch Clamp")]
    public float minPitch = -60f;
    public float maxPitch = 80f;

    private Vector2 lookInput;
    private float pitch;

    private Transform cameraTransform;
    private Camera currentCamera;
    private bool canMove = true;
    void Start()
    {
        UpdateActiveCamera();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!canMove) return;
        UpdateActiveCamera();

        if (cameraTransform == null) return;

        // YAW → rotación horizontal del player
        float yaw = lookInput.x * mouseSensitivity;
        transform.Rotate(0f, yaw, 0f, Space.Self);

        // PITCH → rotación vertical de la cámara
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    void UpdateActiveCamera()
    {
        if (Camera.main != currentCamera)
        {
            currentCamera = Camera.main;

            if (currentCamera != null)
            {
                cameraTransform = currentCamera.transform;

                // Reinicializar pitch según la nueva cámara
                pitch = cameraTransform.localEulerAngles.x;
                if (pitch > 180f) pitch -= 360f;
            }
        }
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void OnEnable()
    {
        lookInput = Vector2.zero;
    }
}



/////////////////////////////
/*
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Arrastra aquí la cámara (hija del player) o un pivot de cámara.")]
    public Transform cameraTransform;

    [Header("Sensitivity")]
    public float mouseSensitivity = 0.15f;

    [Header("Pitch Clamp")]
    public float minPitch = -60f;
    public float maxPitch =  80f;

    private Vector2 lookInput;
    private float pitch; // rotación vertical acumulada

    void Start()
    {
        if (cameraTransform == null)
        {
            // Intenta auto-encontrar una Camera en hijos si no has asignado nada
            Camera cam = GetComponentInChildren<Camera>();
            if (cam != null) cameraTransform = cam.transform;
        }

        // Inicializa pitch con la rotación actual de la cámara
        if (cameraTransform != null)
        {
            pitch = cameraTransform.localEulerAngles.x;
            if (pitch > 180f) pitch -= 360f; // convertir de 0..360 a -180..180
        }

        // (Opcional) Bloquear cursor para FPS/3ª persona
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Acción del Input System (Vector2): Mouse Delta / Stick
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // Yaw (giro horizontal) sobre el player
        float yaw = lookInput.x * mouseSensitivity;
        transform.Rotate(0f, yaw, 0f, Space.Self);

        // Pitch (inclinación vertical) sobre la cámara (local)
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void OnEnable()
    {
        lookInput = Vector2.zero;
    }
}*/