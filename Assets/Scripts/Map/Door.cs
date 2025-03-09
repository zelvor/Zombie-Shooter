using UnityEngine;

public class Door : MonoBehaviour
{
    public int requiredKeyId;
    public Room connectedRoom;
    public GameManager gameManager;
    public Collider triggerCollider;

    private bool isOpen = false;
    private bool playerInRange = false;
    private Collider doorCollider;

    void Start()
    {
        doorCollider = GetComponent<Collider>();
        doorCollider.isTrigger = false;

        triggerCollider.isTrigger = true;

        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (isOpen) return;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void TryOpenDoor()
    {
        if (gameManager.HasKey(requiredKeyId))
        {
            Open();
            if (connectedRoom != null)
            {
                connectedRoom.OpenDoor();
            }
        }
    }

    void Open()
    {
        isOpen = true;
        gameObject.SetActive(false);
    }
}