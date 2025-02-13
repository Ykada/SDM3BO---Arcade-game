using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    private Vector3 offset;
    private Rigidbody rb;
    private float rotationX = 0;
    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        if (cameraTransform != null)
        {
            offset = cameraTransform.position - transform.position;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;
        controller.Move(move);

        bool isMoving = move.magnitude > 0;
        if (animator.HasParameter("isWalking"))
        {
            animator.SetBool("isWalking", isMoving);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Approximately(rb.velocity.y, 0))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (animator.HasParameter("Jump"))
            {
                animator.SetTrigger("Jump");
            }
        }

        if (cameraTransform != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}

public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
}

