using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GridPathFinding : MonoBehaviour
{
    public Transform target;
    public Transform objectToMove;
    public GameObject calculatedPathPrefab;
    public GameObject pathHolder;
    [SerializeField]
    public int objectToMoveIndex = 0;
    public List<Transform> characterGroup1;
    public List<Transform> characterGroup2;
    public List<Transform> selectedCharacterGroup;
    private List<Vector3> currentPath;
    private bool calculatingNewPath = false;
    private bool isMoving = false;

    Path path;
    Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        pathHolder = new GameObject();
        selectedCharacterGroup = new List<Transform>();
        selectedCharacterGroup = characterGroup1;
        objectToMove = selectedCharacterGroup[0];
        pathHolder.name = "PathHolder of " + objectToMove.gameObject.name;
        currentPath = new List<Vector3>();
    }

    /// <summary>
    /// Callback when the path is calculated
    /// </summary>
    /// <param name="p"></param>
    private void OnPathComplete(Path p)
    {
        calculatingNewPath = false;
        if (!p.error)
        {
            path = p;
        }
        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            currentPath.Add(path.vectorPath[i]);
            Instantiate(calculatedPathPrefab, path.vectorPath[i], Quaternion.identity, pathHolder.transform);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isMoving)
            {
                if (objectToMoveIndex == selectedCharacterGroup.Count - 1)
                {
                    objectToMoveIndex = 0;
                }
                else
                {
                    objectToMoveIndex++;
                }
                objectToMove = selectedCharacterGroup[objectToMoveIndex];
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleCharacterGroup();
        }
    }

    /// <summary>
    /// Calculates new path
    /// </summary>
    public void CalculateNewPath()
    {
        if (!isMoving)
        {
            ClearPath();
            calculatingNewPath = true;
            currentPath.Clear();
            seeker.StartPath(objectToMove.position, target.position, OnPathComplete);
        }
    }

    /// <summary>
    /// Travel path with a delay between points
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator TravelPathWithDelay(float delay)
    {
        isMoving = true;
        foreach (Vector3 point in currentPath)
        {
            objectToMove.position = point;
            yield return new WaitForSeconds(delay);
        }

        isMoving = false;
        ClearPath();
    }

    /// <summary>
    /// Clear path of tiles
    /// </summary>
    private void ClearPath()
    {
        foreach (Transform tile in pathHolder.transform)
        {
            Destroy(tile.gameObject, 0.2f);
        }
    }

    /// <summary>
    /// Toggles the current selected group
    /// </summary>
    private void ToggleCharacterGroup()
    {
        if (selectedCharacterGroup == characterGroup1)
        {
            selectedCharacterGroup = characterGroup2;
        }
        else
        {
            selectedCharacterGroup = characterGroup1;
        }
        objectToMove = selectedCharacterGroup[0];
    }

}
