using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 4f;


    private CharacterController characterController;
    private Transform cameraTransform;

    private float verticalRotation = 0f;

    public bool isJumping, isGrounded = false;
    float rotationSpeed = 4;

    float xaxis, yaxis;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        xaxis = Input.GetAxis("Vertical");
        isJumping = Input.GetKeyDown(KeyCode.Space);

        if (isJumping && isGrounded)
        {
            Debug.Log(this.ToString() + " isJumping = " + isJumping);
            // Simulate jump using CharacterController's Move method
            characterController.Move(Vector3.up * jumpForce);
            isGrounded = false; // Update grounded status
        }


        if ((Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f) && !isJumping && isGrounded)
        {
            if (Input.GetAxis("Vertical") >= 0)
                transform.Rotate(new Vector3(0, xaxis * rotationSpeed, 0));
            else
                transform.Rotate(new Vector3(0, -xaxis * rotationSpeed, 0));

        }
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entered");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exited");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void Update()
    {
        // Player Movement
        float xaxis = Input.GetAxis("Horizontal");
        float yaxis = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * xaxis + transform.forward * yaxis;
        moveDirection.y = 0f; // Ensure the player stays grounded
        moveDirection.Normalize(); // Normalize the direction to avoid faster movement diagonally

        characterController.SimpleMove(moveDirection * movementSpeed);

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
