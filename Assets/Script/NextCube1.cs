using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class NextCube : MonoBehaviour
{
    public bool isStair = false;
    public bool canMove = false;
    public bool End = false;
    public BoxCollider c;
    public BoxCollider left;
    public BoxCollider right;
    public BoxCollider top;
    public BoxCollider bottom;
    public bool up;
    Vector3 L = new Vector3(1, 0, 0), 
        R = new Vector3(-1, 0, 0), 
        T = new Vector3(0, 0, 1), 
        B = new Vector3(0, 0, -1), 
        U = new Vector3(0, 1, 0);
    RaycastHit hit;
    Ray ray;
    LayerMask layerMask = 1 << 3;
    public bool done = false;
    
    // Neu raycast hit ben tren thi 
    // Neu khong phai cau thang thi khong di duoc
    // Neu la cau thang thi thay vi di len o do se di len cau thang
    void Start()
    {
        layerMask = ~layerMask;
        c = GetComponent<BoxCollider>();
        ray = new Ray(c.transform.position, U);

        // Kiem tra o ben tren
        Physics.Raycast(ray, out hit, 1, layerMask);
        if (isStair) return;

        // Neu ben tren la vat can thi canMove = false
        if (hit.collider != null)
        {
            canMove = false;
        }
        else canMove = true;
        
        // Tim cac o ben canh neu di duoc
        ray = new Ray(c.transform.position, L);
        if (Physics.Raycast(ray, out hit, 1))
        {
            left = hit.collider as BoxCollider;
        }
        ray = new Ray(c.transform.position, R);
        if (Physics.Raycast(ray, out hit, 1))
        {
            right = hit.collider as BoxCollider;
        }
        ray = new Ray(c.transform.position, T);
        if (Physics.Raycast(ray, out hit))
        {
            top = hit.collider as BoxCollider;
        }
        ray = new Ray(c.transform.position, B);
        if (Physics.Raycast(ray, out hit, 1))
        {
            bottom = hit.collider as BoxCollider;
        }

        done = true;
    }
    private void Update()
    {
        ray = new Ray(c.transform.position, U);

        // Kiem tra o ben tren
        Physics.Raycast(ray, out hit, 2, layerMask);

        // Neu ben tren la vat can thi canMove = false
        if (hit.collider != null)
        {
            up = true;
            canMove = false;
        }
        else canMove = true;
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (left != null)
            Gizmos.DrawSphere(new Vector3(left.transform.position.x, left.transform.position.y + .5f, left.transform.position.z), .1f);
        if (right != null)
            Gizmos.DrawSphere(new Vector3(right.transform.position.x, right.transform.position.y + .5f, right.transform.position.z), .1f);
        if (top != null)
            Gizmos.DrawSphere(new Vector3(top.transform.position.x, top.transform.position.y + .5f, top.transform.position.z), .1f);
        if (bottom != null)
            Gizmos.DrawSphere(new Vector3(bottom.transform.position.x, bottom.transform.position.y + .5f, bottom.transform.position.z), .1f);
    }
    */
    // Update is called once per frame
}
