using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Moving : MonoBehaviour
{
    // Start is called before the first frame update
    FindRoad character;
    RaycastHit hit;
    Ray checkEnd;
    bool end = false;
    public float offset = 0;
    
    void Start()
    {
        character = GetComponent<FindRoad>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!end)
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        NextCube tmp = character.route[1];
        checkEnd = new Ray(character.transform.position, new Vector3(
                tmp.transform.position.x - character.route[0].transform.position.x,
                0,
                tmp.transform.position.z - character.route[0].transform.position.z
                ));

        if(!end)
        if (Physics.Raycast(checkEnd, out hit, 1f))
        {
            if (hit.collider.GetComponent<NextCube>().End)
            {
                end = true;
                Debug.Log("Win!");
            }
        }

        offset = tmp.transform.position.y - character.route[0].transform.position.y;
        yield return new WaitForSeconds(2);
        if(offset > 0)
        {
            transform.Translate(new Vector3(0, offset, 0));
            yield return new WaitForSeconds(2);
            transform.Translate(new Vector3(
                tmp.transform.position.x - character.route[0].transform.position.x,
                0,
                tmp.transform.position.z - character.route[0].transform.position.z
                )
            );
        }
        else
        {
            transform.Translate(new Vector3(
                tmp.transform.position.x - character.route[0].transform.position.x,
                0,
                tmp.transform.position.z - character.route[0].transform.position.z
                )
            );
            yield return new WaitForSeconds(2);
            transform.Translate(new Vector3(0, offset, 0));
        }
        yield return new WaitForSeconds(2);
    }
}
