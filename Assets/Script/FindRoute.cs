using System.Collections;
using UnityEngine;


public class FindRoad : MonoBehaviour
{
    public NextCube[] route;
    public CapsuleCollider c;
    public NextCube tmp;
    public NextCube pos;
    public int turn = 3;
    RaycastHit hit;
    IEnumerator cou;
    // Start is called before the first frame update
    void Start()
    {
        route = new NextCube[5];
        //Vi tri ban dau
        c = GetComponent<CapsuleCollider>();
        Physics.Raycast(new Ray(c.transform.position, new Vector3(0, -1, 0)), out hit);
        //pos la vi tri gan nhat cua capsule, tmp la o bat dau de tao duong di
        pos = tmp = (hit.collider).GetComponent<NextCube>();
        
        //doi den khi khoi tao xong ban do
        cou = WaitingForLove();
        StartCoroutine(cou);

        NextCube tmp2;

        int i = 0;
        turn = 1;
        tmp = pos;

        while (i < 4)
        {
            route[i] = tmp;
            if (turn == 1)
            {
                if (!tmp.right)
                {
                    route[i + 1] = null;
                    break;
                }
                tmp2 = tmp.right.GetComponent<NextCube>();
          
                if (tmp2.canMove)
                {
                    tmp = tmp.right.GetComponent<NextCube>();
                    ++i;
                }
                else turn = 2;
            }
            else if (turn == 2)
            {
                if (!tmp.bottom)
                {
                    route[i + 1] = null;
                    break;
                }
                tmp2 = tmp.bottom.GetComponent<NextCube>();
                
                if (tmp2.canMove)
                {
                    tmp = tmp.bottom.GetComponent<NextCube>();
                    ++i;
                }
                else turn = 3;
            }
            else if (turn == 3)
            {
                if (!tmp.left)
                {
                    route[i + 1] = null;
                    break;
                }
                tmp2 = tmp.left.GetComponent<NextCube>();
                
                if (tmp2.canMove)
                {
                    tmp = tmp.left.GetComponent<NextCube>();
                    ++i;
                }
                else turn = 4;
            }
            else
            {
                if (!tmp.top)
                {
                    route[i + 1] = null;
                    break;
                }
                tmp2 = tmp.top.GetComponent<NextCube>();
                
                if (tmp2.canMove)
                {
                    tmp = tmp.top.GetComponent<NextCube>();
                    ++i;
                }
                else turn = 1;
            }
        }
        route[i] = tmp;
    }
    
    private void OnDrawGizmos()
    {
        if (route.Length == 0)
        {
            Gizmos.DrawCube(c.transform.position, new Vector3(1, 1, 1));
            return;
        }
        
        Gizmos.color = Color.red;

        for (int j = 0; j < 5; j++)
        {
            if (route[j] == null) break;
            Gizmos.DrawSphere(route[j].transform.position + new Vector3(0, 0.5f, 0), 0.05f);
        }

        Physics.Raycast(new Ray(c.transform.position, new Vector3(0, -1, 0)), out hit);
        pos = (hit.collider).GetComponent<NextCube>();


        NextCube tmp2;

            int i = 0;
            turn = 1;
            tmp = pos;
            
            
            
            while(i < 4)
            {
                route[i] = tmp;
                if (turn == 1)
                {
                if (!tmp.right)
                {
                    route[i + 1] = null;
                    break;
                }
                tmp2 = tmp.right.GetComponent<NextCube>();
                
                if (tmp2.canMove)
                    {
                        tmp = tmp.right.GetComponent<NextCube>();
                        ++i;
                    }
                    else turn = 2;
                }
                else if (turn == 2)
                {
                if (!tmp.bottom)
                {
                    route[i + 1] = null;
                    break;
                }
                tmp2 = tmp.bottom.GetComponent<NextCube>();
                
                if (tmp2.canMove)
                    {
                        tmp = tmp.bottom.GetComponent<NextCube>();
                        ++i;
                    }
                    else turn = 3;
                }
                else if (turn == 3)
                {
                if (!tmp.left)
                {
                    route[i + 1] = null;
                    break;
                }
                tmp2 = tmp.left.GetComponent<NextCube>();
                
                if (tmp2.canMove)
                    {
                        tmp = tmp.left.GetComponent<NextCube>();
                        ++i;
                    }
                    else turn = 4;
                }
                else
                {
                if (!tmp.top)
                {
                    route[i + 1] = null;
                    break;
                }
                tmp2 = tmp.top.GetComponent<NextCube>();
                
                if (tmp2.canMove)
                    {
                        tmp = tmp.top.GetComponent<NextCube>();
                        ++i;
                    }
                    else turn = 1;
                }
            }
        route[i] = tmp;
    }
    
    bool isDone()
    {
        return tmp.done;
    }

    IEnumerator WaitingForLove()
    {
        Debug.Log("Wait");
        yield return new WaitUntil(isDone);
    }
}
