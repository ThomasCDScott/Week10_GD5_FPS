using UnityEngine;
using UnityEngine.UI;


public class FPSControllerScript : MonoBehaviour
{
    private Camera playerCamera;
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6;
    [SerializeField] float runMultiplier = 1.5f;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float gravity = 9.8f;

    public float mouseSensitivity = 2f;
    [SerializeField] float lookXLimit = 60;
    
    float rotationX = 0;
    Vector3 moveDirection;
    CharacterController characterController;

    public Image staminaBar;

    public float Stamina, maxStamina;
    public float runCost;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();

        //cursor settings
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Camera Rotation
        //Looking left and right
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime);

        //Looking up and down
        rotationX += -Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, - lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        #endregion

        #region movement
        if (characterController.isGrounded)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            float moveDirectionY = moveDirection.y;

            moveDirection = (horizontalInput * transform.right) + (verticalInput * transform.forward);

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
            else
            {
                moveDirection.y = moveDirectionY;   
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= runMultiplier;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= runMultiplier;
        }

        Stamina -= runCost * Time.deltaTime;
        if (Stamina < 0) Stamina = 0;
        staminaBar.fillAmount = Stamina / maxStamina;

        
    }
#endregion

}

