using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public RectTransform uiRect;
    public HexGridChunk chunk;public HexCell GetNeighbor(HexDirection direction) { return neighbors[(int)direction]; }
    public Vector3 Position { get { return transform.localPosition; } }
    public Color Color { get { return HexMetrics.colors[terrainTypeIndex]; } }
    public bool HasIncomingRiver { get { return hasIncomingRiver; } }
    public bool HasOutgoingRiver { get { return hasOutgoingRiver; } }
    public bool HasRiver { get { return hasIncomingRiver || hasOutgoingRiver; } }
    public bool HasRiverBeginOrEnd { get { return hasIncomingRiver != hasOutgoingRiver; } }
    public bool IsUnderwater { get { return waterLevel > elevation; } }
    public bool IsSpecial { get { return specialIndex > 0; } }

    public bool HasRoads
    {
        get
        {
            for (int i = 0; i < roads.Length; i++)
            {
                if (roads[i])
                {
                    return true;
                }
            }

            return false;
        }
    }

    public bool HasRoadThroughEdge(HexDirection direction)
    {
        return roads[(int)direction];
    }

    public bool Walled
    {
        get { return walled; }

        set
        {
            if (walled != value)
            {
                walled = value;
                Refresh();
            }
        }
    }

    public bool HasRiverThroughEdge(HexDirection direction)
    {
        return hasIncomingRiver && incomingRiver == direction || hasOutgoingRiver && outgoingRiver == direction;
    }

    public int urbanLevel, farmLevel, plantLevel;

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            if (elevation == value)
            {
                return;
            }

            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y += (HexMetrics.SampleNoise(position).y * 2f - 1f) * HexMetrics.elevationPerturbStrength;
            transform.localPosition = position;
            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;

            ValidateRivers();

            for (int i = 0; i < roads.Length; i++)
            {
                if (roads[i] && GetElevationDifference((HexDirection)i) > 1)
                {
                    SetRoad(i, false);
                }
            }

            Refresh();
        }
    }

    public int GetElevationDifference(HexDirection direction)
    {
        int difference = elevation - GetNeighbor(direction).elevation;
        return difference >= 0 ? difference : -difference;
    }

    public int UrbanLevel
    {
        get { return urbanLevel; }

        set
        {
            if (urbanLevel != value)
            {
                urbanLevel = value;
                RefreshSelfOnly();
            }
        }
    }

    public int FarmLevel
    {
        get { return farmLevel; }

        set
        {
            if (farmLevel != value)
            {
                farmLevel = value;
                RefreshSelfOnly();
            }
        }
    }

    public int PlantLevel
    {
        get { return plantLevel; }

        set
        {
            if (plantLevel != value)
            {
                plantLevel = value;
                RefreshSelfOnly();
            }
        }
    }

    public int WaterLevel
    {
        get { return waterLevel; }

        set
        {
            if (waterLevel == value) return;
            waterLevel = value;
            ValidateRivers();
            Refresh();
        }
    }

    public int SpecialIndex
    {
        get { return specialIndex; }

        set
        {
            if(specialIndex != value && !HasRiver)
            {
                specialIndex = value;
                RemoveRoads();
                RefreshSelfOnly();
            }
        }
    }

    public int TerrainTypeIndex
    {
        get { return terrainTypeIndex; }

        set
        {
            if(terrainTypeIndex != value)
            {
                terrainTypeIndex = value;
                Refresh();
            }
        }
    }

    public float WaterSurfaceY { get { return (waterLevel + HexMetrics.waterElevationOffset) * HexMetrics.elevationStep; } }
    public float RiverSurfaceY { get { return (elevation + HexMetrics.waterElevationOffset) * HexMetrics.elevationStep; } }
    public float StreamBedY { get { return (elevation + HexMetrics.streamBedElevationOffset) * HexMetrics.elevationStep; } }
    public HexDirection RiverBeginOrEndDirection { get { return hasIncomingRiver ? incomingRiver : outgoingRiver; } }
    public HexDirection IncomingRiver { get { return incomingRiver; } }
    public HexDirection OutgoingRiver { get { return outgoingRiver; } }

    public HexEdgeType GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
    }

    public HexEdgeType GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(elevation, otherCell.elevation);
    }

    [SerializeField] HexCell[] neighbors;
    bool IsValidRiverDestination(HexCell neighbor) { return neighbor && (elevation >= neighbor.elevation || waterLevel == neighbor.elevation); }
    [SerializeField] bool[] roads;
    bool hasIncomingRiver, hasOutgoingRiver;
    bool walled;
    int elevation = int.MinValue;
    int waterLevel;
    int specialIndex;
    int terrainTypeIndex;
    HexDirection incomingRiver, outgoingRiver;

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void RemoveIncomingRiver()
    {
        if (!hasIncomingRiver)
        {
            return;
        }

        hasIncomingRiver = false;
        RefreshSelfOnly();
        HexCell neighbor = GetNeighbor(incomingRiver);
        neighbor.hasOutgoingRiver = false;
        neighbor.RefreshSelfOnly();
    }

    public void RemoveOutgoingRiver()
    {
        if (!hasOutgoingRiver)
        {
            return;
        }

        hasOutgoingRiver = false;
        RefreshSelfOnly();
        HexCell neighbor = GetNeighbor(outgoingRiver);
        neighbor.hasIncomingRiver = false;
        neighbor.RefreshSelfOnly();
    }

    public void RemoveRiver()
    {
        RemoveOutgoingRiver();
        RemoveIncomingRiver();
    }

    public void SetOutgoingRiver(HexDirection direction)
    {
        if (hasOutgoingRiver && outgoingRiver == direction)
        {
            return;
        }

        HexCell neighbor = GetNeighbor(direction);

        if (!IsValidRiverDestination(neighbor))
        {
            return;
        }

        RemoveOutgoingRiver();

        if (hasIncomingRiver && incomingRiver == direction)
        {
            RemoveIncomingRiver();
        }

        hasOutgoingRiver = true;
        outgoingRiver = direction;
        specialIndex = 0;
        neighbor.RemoveIncomingRiver();
        neighbor.hasIncomingRiver = true;
        neighbor.incomingRiver = direction.Opposite();
        neighbor.specialIndex = 0;
        SetRoad((int)direction, false);
    }

    public void AddRoad(HexDirection direction)
    {
        if (
                !roads[(int)direction] && !HasRiverThroughEdge(direction) && 
                !IsSpecial && !GetNeighbor(direction).IsSpecial && 
                GetElevationDifference(direction) <= 1
           )
        {
            SetRoad((int)direction, true);
        }
    }

    public void RemoveRoads()
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (roads[i])
            {
                SetRoad(i, false);
            }
        }
    }

    void ValidateRivers()
    {
        if (hasOutgoingRiver && !IsValidRiverDestination(GetNeighbor(outgoingRiver))) RemoveOutgoingRiver();
        if (hasIncomingRiver && !GetNeighbor(incomingRiver).IsValidRiverDestination(this)) RemoveIncomingRiver();
    }

    void SetRoad(int index, bool state)
    {
        roads[index] = state;
        neighbors[index].roads[(int)((HexDirection)index).Opposite()] = state;
        neighbors[index].RefreshSelfOnly();
        RefreshSelfOnly();
    }

    void Refresh()
    {
        if (chunk)
        {
            chunk.Refresh();

            for (int i = 0; i < neighbors.Length; i++)
            {
                HexCell neighbor = neighbors[i];

                if (neighbor != null && neighbor.chunk != chunk)
                {
                    neighbor.chunk.Refresh();
                }
            }
        }
    }

    void RefreshSelfOnly()
    {
        chunk.Refresh();
    }
}