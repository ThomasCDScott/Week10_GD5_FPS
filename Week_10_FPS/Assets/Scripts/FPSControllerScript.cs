using UnityEngine;
using UnityEngine.UI;
using System.Collections;


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
    public float chargeRate;
    private Coroutine recharge;
    public bool running = false;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();

        //cursor settings
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        

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
            running = true;

            Stamina -= runCost * Time.deltaTime;
            if (Stamina < 0) Stamina = 0;
            staminaBar.fillAmount = Stamina / maxStamina;


            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= runMultiplier;
        }




        #endregion

     
    }


    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while(Stamina < maxStamina)
        {
            Stamina += chargeRate / 10f;
            if (Stamina > maxStamina) Stamina = maxStamina;
            staminaBar.fillAmount = Stamina / maxStamina;
            yield return new WaitForSeconds(.1f);

            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }
    }

    void TakeDamage(int Damage)
    {
        currentHealth -= Damage;

        healthBar.SetHealth(currentHealth);
    }

}

