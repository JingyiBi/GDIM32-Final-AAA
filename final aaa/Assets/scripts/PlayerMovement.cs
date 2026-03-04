using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float mouseSensitivity = 150f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;

    private CharacterController cc;
    private float xRotation = 0f; 
    private Transform mainCamera;
    private Vector3 velocity;


    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        mainCamera.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void HandleGravityAndJump()
    {
        if (cc.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked");
        }
    }
}