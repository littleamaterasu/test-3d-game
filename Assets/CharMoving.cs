using System.Collections;
using UnityEngine;

public class CharMoving : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public Transform spawnPoint;
    public float fallDuration = 1.4f;
    LayerMask obstacleLayer;
    public float interactionDistance = 2f;
    public KeyCode pickUpKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q; // Added drop key
    public Transform carryPosition;
    public Vector3 cur;

    private Vector3 movement;
    private bool isFalling = false;
    private GameObject carriedObject;
    private float fallTimer = 0f;
    private Material originalMaterial;

    private void Start()
    {
        obstacleLayer = 1 << 2;
        obstacleLayer = ~obstacleLayer;
    }

    void Update()
    {
        if (!isFalling)
        {
            MoveCharacter();
            RotateCharacter();
            CheckFall();
            TryPickUp();
            TryDropObject();
        }
        else
        {
            FallAndRespawn();
        }
    }

    void MoveCharacter()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            cur = movement;
        }

        if (!CheckObstacleInMovement())
        {
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void RotateCharacter()
    {
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }

    bool CheckObstacleInMovement()
    {
        RaycastHit hit;
        Vector3[] dirCheck = new Vector3[3];
        dirCheck[0] = new Vector3(0, 0, 0);
        if (movement.x != 0)
        {
            dirCheck[1] = new Vector3(0, -.25f, .5f);
            dirCheck[2] = new Vector3(0, -.25f, -.5f);
        }
        else if (movement.z != 0)
        {
            dirCheck[1] = new Vector3(.5f, -.25f, 0);
            dirCheck[2] = new Vector3(-.5f, -.25f, 0);
        }
        for (int i = 0; i < 3; i++)
        {
            if (Physics.Raycast(transform.position + dirCheck[i], movement, out hit, .5f, obstacleLayer))
            {
                return true;
            }
        }
        return false;
    }

    void CheckFall()
    {
        RaycastHit hit;
        Vector3[] dirCheck = new Vector3[8];
        dirCheck[0] = new Vector3(0, 0, 0.5f);
        dirCheck[1] = new Vector3(0, 0, -0.5f);
        dirCheck[2] = new Vector3(0.5f, 0, 0);
        dirCheck[3] = new Vector3(-0.5f, 0, 0);
        dirCheck[4] = new Vector3(0.5f, 0, 0.5f);
        dirCheck[5] = new Vector3(-0.5f, 0, -0.5f);
        dirCheck[6] = new Vector3(-0.5f, 0, 0.5f);
        dirCheck[7] = new Vector3(0.5f, 0, -0.5f);

        for (int i = 0; i < 8; i++)
        {
            if (Physics.Raycast(transform.position + dirCheck[i], Vector3.down, out hit, 1f))
            {
                isFalling = false;
                return;
            }
        }
        isFalling = true;
        fallTimer = 0f;
    }

    void FallAndRespawn()
    {
        fallTimer += Time.deltaTime;
        Vector3 movementt = new Vector3(0, -2f, 0);
        transform.Translate(movementt * moveSpeed * Time.deltaTime);
        if (fallTimer >= fallDuration)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = spawnPoint.position;
        isFalling = false;
        fallTimer = 0f;
    }

    void TryPickUp()
    {
        if (Input.GetKeyDown(pickUpKey))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, -.25f, 0), cur, out hit, interactionDistance))
            {
                if (hit.collider.CompareTag("pick-up"))
                {
                    PickUpObject(hit.collider.gameObject);
                }
            }
        }
    }

    void PickUpObject(GameObject pickedObject)
    {
        carriedObject = pickedObject;
        carriedObject.GetComponent<Collider>().enabled = false;
        carriedObject.transform.position = carryPosition.position + cur + new Vector3(0, .5f, 0);
        carriedObject.transform.parent = transform;
    }

    void TryDropObject()
    {
        if (Input.GetKeyDown(dropKey) && carriedObject != null)
        {
            DropObject();
        }
    }

    void DropObject()
    {
        carriedObject.GetComponent<Collider>().enabled = true;
        carriedObject.transform.parent = null;

        carriedObject.transform.position += new Vector3(0, -.85f, 0);

        carriedObject = null;
    }
}