using UnityEngine;

public class PlayerController : MonoBehaviour, IWeaponUser
{
    private CharacterController controller;
    public Transform cameraTransform;

    [Header("Human Data")]
    [SerializeField] private HumanData humanData;

    [Header("Movement Settings")]
    public float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f;

    [Header("Camera Settings")]
    public Vector3 cameraOffset = new Vector3(0f, 7f, -5f);
    public Vector3 cameraRotation = new Vector3(45f, 0f, 0f);

    [Header("Weapon Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Weapon currentWeapon;
    public WeaponData[] availableWeapons;
    private int currentWeaponIndex = 0;

    private HealthModule healthModule;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        cameraTransform.rotation = Quaternion.Euler(cameraRotation);
        gameObject.tag = "Player";

        healthModule = GetComponent<HealthModule>();
        healthModule.SetHealthData(humanData);

        if (availableWeapons.Length > 0)
        {
            currentWeapon.SetWeaponData(availableWeapons[currentWeaponIndex]);
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        MovePlayer();
        RotatePlayer();
        UpdateCamera();
        SwitchWeapon();

        if (currentWeapon != null)
        {
            currentWeapon.ManualUpdate();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        if (move.magnitude >= 0.1f)
        {
            controller.Move(move * humanData.moveSpeed * Time.deltaTime);
        }
    }

    void RotatePlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0f;
            targetDirection.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void UpdateCamera()
    {
        cameraTransform.position = transform.position + cameraOffset;
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q) && availableWeapons.Length > 1)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % availableWeapons.Length;
            currentWeapon.SetWeaponData(availableWeapons[currentWeaponIndex]);
        }
    }

    public bool ShouldFire()
    {
        if (availableWeapons[currentWeaponIndex].weaponName == "Assault Rifle")
        {
            return Input.GetButton("Fire1");
        }
        else if (availableWeapons[currentWeaponIndex].weaponName == "Grenade Launcher")
        {
            return Input.GetButtonDown("Fire1") || Input.GetButtonUp("Fire1");
        }
        return false;
    }
}