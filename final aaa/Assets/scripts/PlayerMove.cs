using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 150f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;

    private CharacterController cc;
    private Transform mainCamera;
    private Vector3 velocity;
    
    
    private float yaw;
    private float pitch;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
    }

    private void Start()
    {
        yaw = transform.eulerAngles.y;
        Vector3 camRot = mainCamera.localEulerAngles;
        pitch = (camRot.x > 180) ? camRot.x - 360 : camRot.x;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraRotation();
        HandleGravityAndJump();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = transform.right * horizontal + transform.forward * vertical;
        cc.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        bool isRotating = Input.GetMouseButton(1);

        if (isRotating)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            mainCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleGravityAndJump()
    {
        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && cc.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}