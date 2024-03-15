using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Vector3 offset;
    public Transform start;
    public Playing playing;
    private void Start()
    {
        offset = transform.position - start.position;
    }
    public void Follow(Transform target)
    {
        transform.Translate((target.position + offset - transform.position) * Time.deltaTime, Space.World);
    }
    private void Update()
    {
        if(playing.isPlaying)
        {
            Follow(playing.chosen.transform);
        }
    }
}