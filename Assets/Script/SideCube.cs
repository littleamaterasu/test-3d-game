using Cinemachine;
using System.Collections;
using UnityEngine;

public class SideCube : MonoBehaviour
{
    public Behaviour a;
    public Transform bird;
    public surface[] route;
    public surface current;
    public surface next;
    public int turn = 0;
    public Animator animator;
    public Vector3[] direct = new Vector3[4];
    public float fallDuration;
    public Transform spawnPoint;
    public float fallTimer = 0f;
    public Material highlight;
    public CinemachineBrain cinemachineBrain;
    public Camera mainCamera;
    Vector3 rayDir = Vector3.down;
    
    RaycastHit hit;
    bool fall = false;
    bool isFalling = false;
    private int fallCount = 0;
    public bool end = false;

    void Start()
    {
        direct[2] = new Vector3(1, 0, 0);
        direct[0] = new Vector3(-1, 0, 0);
        direct[1] = new Vector3(0, 0, 1);
        direct[3] = new Vector3(0, 0, -1);
        Ray ray = new(bird.position, rayDir);
        Physics.Raycast(ray, out hit);
        current = hit.collider.GetComponent<surface>();
        next = current.surfaceTransform[turn].GetComponent<surface>();
        Invoke("FindRoute", 1);
    }

    private void ResetHighlight()
    {
        // Reset highlight state for all objects
        foreach (var renderer in FindObjectsOfType<MeshRenderer>())
        {
            // Kiểm tra xem renderer có script Surface không
            var surfaceScript = renderer.GetComponent<surface>();
            if (surfaceScript != null && surfaceScript.isChosen)
            {
                // Nếu có và là đối tượng được chọn, bỏ qua việc đặt lại vật liệu
                continue;
            }

            // Đặt lại vật liệu cho các renderer không bị ảnh hưởng
            if (renderer != null)
            {
                Material[] materials = renderer.materials;
                if (materials.Length > 1)
                {
                    materials[1] = null; // Hoặc bạn có thể gán một vật liệu mặc định ở đây
                    renderer.materials = materials;
                }
            }
        }
    }

    void FindRoute()
    {
        if (!fall)
        {
            a.enabled = true;
            ResetHighlight();
            int cur = turn;
            route = new surface[5];
            Ray ray = new(bird.position, rayDir);
            if (Physics.Raycast(ray, out hit))
            {
                route[0] = hit.collider.GetComponent<surface>();
                Material[] tmp1 = route[0].GetComponent<MeshRenderer>().materials;
                tmp1[1] = highlight;
                route[0].GetComponent<MeshRenderer>().materials = tmp1;
                for (int i = 1; i < 5; ++i)
                {
                    if (!route[i - 1].surfaceTransform[cur]) break;
                    while (!route[i - 1].surfaceTransform[cur].GetComponent<surface>().canMove)
                    {
                        cur = (cur + 1) % 4;
                    }
                    route[i] = route[i - 1].surfaceTransform[cur].GetComponent<surface>();
                    Material[] tmp = route[i].GetComponent<MeshRenderer>().materials;
                    tmp[1] = highlight;
                    route[i].GetComponent<MeshRenderer>().materials = tmp;
                }
            }
        }
    }

    void TryMove()
    {
        if (fall)
        {
            a.enabled = false;
            CheckFall();
            if (isFalling)
            {
                ++fallCount;
                FallAndRespawn();
            }
            else Move(direct[turn]);
        }
        if (IsAtCenterOfNext())
        {
            current = next;
            if (!route[1])
            {
                Move(direct[turn]);
                fall = true;
            }
            else
            {
                next = route[1];
            }
        }

        else if (next != current)
        {
            if (current.transform.position.x - next.transform.position.x > 0)
            {
                turn = 0;
                transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }
            else if (current.transform.position.x - next.transform.position.x < 0)
            {
                turn = 2;
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
            else if (current.transform.position.z - next.transform.position.z > 0)
            {
                turn = 3;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                turn = 1;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            Move(direct[turn]);
            FindRoute();
        }
    }

    void CheckFall()
    {
        RaycastHit hit;
        Vector3[] dirCheck = new Vector3[8];
        dirCheck[0] = new Vector3(0, 0, 0.25f);
        dirCheck[1] = new Vector3(0, 0, -0.25f);
        dirCheck[2] = new Vector3(0.25f, 0, 0);
        dirCheck[3] = new Vector3(-0.25f, 0, 0);
        dirCheck[4] = new Vector3(0.25f, 0, 0.25f);
        dirCheck[5] = new Vector3(-0.25f, 0, -0.25f);
        dirCheck[6] = new Vector3(-0.25f, 0, 0.25f);
        dirCheck[7] = new Vector3(0.25f, 0, -0.25f);

        for (int i = 0; i < 8; i++)
        {
            if (Physics.Raycast(bird.transform.position + dirCheck[i], Vector3.down, out hit, 1f))
            {
                isFalling = false;
                return;
            }
        }
        isFalling = true;
        
    }

    void FallAndRespawn()
    {
        fallTimer += Time.deltaTime;
        Vector3 movementt = new Vector3(0, -2f, 0);
        transform.Translate(movementt * 10f * Time.deltaTime);
        if (fallTimer >= fallDuration)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = spawnPoint.position;
        isFalling = false;
        fall = false;
        turn = 0;
        transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        Ray ray = new Ray(bird.position, rayDir);
        Physics.Raycast(ray, out hit);
        current = hit.collider.GetComponent<surface>();
        next = current.surfaceTransform[turn].GetComponent<surface>();
        fallTimer = 0f;
    }

    void Move(Vector3 dir)
    {
        transform.Translate(dir * .5f * Time.deltaTime, Space.World);
    }

    bool IsAtCenterOfNext()
    {
        float distanceThreshold = 0.52f;
        return Vector3.Distance(transform.position, next.transform.position) < distanceThreshold;
    }

    void Update()
    {
        if (!end)
        {
            Ray ray = new Ray(bird.position, rayDir);
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("end"))
                {
                    end = true;
                    return;
                }
            }         
            TryMove();

            if (fallCount == 0)
            {
                cinemachineBrain.enabled = true;
                mainCamera.enabled = false;
            }
            else
            {
                mainCamera.enabled = true;
                cinemachineBrain.enabled = false;
            }
        }
        else
        {
            animator.enabled = false;
            cinemachineBrain.enabled = true;
            mainCamera.enabled = false;
        }
        
    }

}
