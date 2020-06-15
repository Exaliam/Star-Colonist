﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [Header("Coordinates")]
    public int width = 6;
    public int height = 6;
    [SerializeField] private float offset = 0.5f;
    [Header("Prefabs")]
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    [Header("Colors")]
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;
    //References
    Canvas gridCanvas;
    HexMesh hexMesh;
    HexCell[] cells;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++) // i = index number
        {
            for(int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * offset - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * HexMetrics.outerRadius * 1.5f;

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        Text label = Instantiate<Text>(cellLabelPrefab); //Instantiate and then show coordinates
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2; //convert the cell coordinates to an array index
        HexCell cell = cells[index];
        cell.color = color; //change color and then re-triangulate the mesh
        hexMesh.Triangulate(cells);
        Debug.Log("touched at " + coordinates.ToString());
    }
}