using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    private Material normalMaterial;
    private Material highlightedMaterial;
    private MeshRenderer meshRenderer;
    private Material currentMaterial;
    private GameObject currentTarget;
    private GridPathFinding gridPathFinding;

    private void Start()
    {
        normalMaterial = GetComponentInParent<GridController>().normalMaterial;
        highlightedMaterial = GetComponentInParent<GridController>().highlightedMaterial;
        currentTarget = GetComponentInParent<GridController>().target;
        gridPathFinding = GetComponentInParent<GridController>().gridPathFinding;
        meshRenderer = GetComponent<MeshRenderer>();
        currentMaterial = meshRenderer.material;
        meshRenderer.material = normalMaterial;
    }

    private void OnMouseEnter()
    {
        meshRenderer.material = highlightedMaterial;
        currentTarget.transform.position = new Vector3(transform.position.x, currentTarget.transform.position.y, transform.position.z);
        gridPathFinding.CalculateNewPath();
    }
    private void OnMouseExit()
    {
        meshRenderer.material = normalMaterial;
    }
    private void OnMouseDown()
    {
        StartCoroutine(gridPathFinding.TravelPathWithDelay(0.2f));
    }
}
