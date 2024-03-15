using UnityEngine;

public class Playing : MonoBehaviour
{
    public surface chosen;
    public Material highlightMaterial;
    public bool isPlaying = false;

    private void Start()
    {
        chosen.isChosen = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            // Apply highlight to the current chosen surface
            chosen.ApplyHighlight(highlightMaterial);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isPlaying = true;
            MoveToAdjacentSurface(0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isPlaying = true;
            MoveToAdjacentSurface(2);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isPlaying = true;
            MoveToAdjacentSurface(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isPlaying = true;
            MoveToAdjacentSurface(3);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            chosen.PlaceObstacle(); // Place object on the chosen surface
            Debug.Log("E");
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            chosen.RemoveObstacle(); // Remove object from the chosen surface
            Debug.Log("F");
        }
    }

    void MoveToAdjacentSurface(int index)
    {
        if (chosen.surfaceTransform[index] != null)
        {
            chosen.isChosen = false;
            chosen.ResetHighlight();
            chosen = chosen.surfaceTransform[index].GetComponent<surface>();
            chosen.isChosen = true;
            isPlaying = true;
        }
    }
}
