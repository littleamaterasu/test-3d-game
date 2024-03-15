using UnityEngine;

public class surface : MonoBehaviour
{
    public Transform[] surfaceTransform = new Transform[4];
    public bool[] noNeighbor = new bool[4];
    public bool canMove = true;
    public bool isChosen = false;
    public bool isOcc = false;
    public GameObject obstaclePrefab; // Prefab của chướng ngại vật
    public Material highlight;
    Ray ray;
    Vector3 rayDir = Vector3.up;
    LayerMask layerMask;

    private GameObject obstacle; // Instance của chướng ngại vật

    private void Start()
    {
        layerMask = 1 << 2;
        layerMask = ~layerMask;
    }

    public void ApplyHighlight(Material highlightMaterial)
    {
        if (highlightMaterial != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && renderer.materials.Length > 1)
            {
                Material[] materials = renderer.materials;
                materials[1] = highlightMaterial;
                renderer.materials = materials;
            }
        }
    }

    // Reset highlight material of the surface renderer
    public void ResetHighlight()
    {
        if (!isChosen)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && renderer.materials.Length > 1)
            {
                Material[] materials = renderer.materials;
                materials[1] = null;
                renderer.materials = materials;
            }
        }
    }

    // Place object on the surface if it's not occupied
    public void PlaceObstacle()
    {
        if (!isOcc && obstaclePrefab != null)
        {
            // Instantiate chướng ngại vật tại vị trí của bề mặt
            obstacle = Instantiate(obstaclePrefab, transform.position + new Vector3(0, 3.5f, 0), Quaternion.identity);

            isOcc = true; // Đánh dấu bề mặt là đã bị chiếm
        }
    }

    // Loại bỏ chướng ngại vật từ bề mặt
    public void RemoveObstacle()
    {
        if (isOcc && obstacle != null)
        {
            // Xóa đối tượng được tạo bởi Instantiate()
            Destroy(obstacle);
            isOcc = false; // Đánh dấu bề mặt là không bị chiếm
        }
    }

    private void Update()
    {
        ray = new Ray(transform.position, rayDir);
        if (Physics.Raycast(ray, 1f, layerMask))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }
}
