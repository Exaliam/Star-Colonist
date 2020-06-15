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

    HexCell[] cells;
    Canvas gridCanvas;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++) // i = index number
        {
            for(int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
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

        Text label = Instantiate<Text>(cellLabelPrefab); //Instantiate and then show coordinates
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = x.ToString() + "\n" + z.ToString();
    }
}
