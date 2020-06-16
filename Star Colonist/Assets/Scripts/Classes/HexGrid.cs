using System.Collections;
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

        //Connecting cell neighbors
        if (x > 0) 
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if(z > 0)
        {
            if((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);

                if(x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);

                if(x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        //Instantiate cell and then show coordinates
        Text label = Instantiate<Text>(cellLabelPrefab); 
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        cell.uiRect = label.rectTransform;
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2; //convert the cell coordinates to an array index
        return cells[index];
    }

    public void Referesh()
    {
        hexMesh.Triangulate(cells);
    }
}