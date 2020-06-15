using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexCell : MonoBehaviour
{
    public HexCoordinate coordinate;

    public Color color;

    public RectTransform uiRect;
	
	public bool buildable;

    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Material material;

    public HexType hexType;

	public Building building;
    public Pawn pawn;
    public Indicator indicator;
	public GameEventDisplayer gameEventDisplayer;

    public int Distance { get; set; }
    public int heuristicDistance;
    public HexCell prevCell;
    
    // index in hexMap.cells
    public int index;
	
    public void InitHexCell()
    {
        hexType = HexType.Plain;
		hexNeighbours =new HexCell[6];
    }

    public void GenerateMesh()
    {
        mesh = gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
        mesh.name = "HexMesh";
        
        mesh.Clear();
        meshRenderer = GetComponent<MeshRenderer>();
        int i = 0;
        vertices = new Vector3[3 * 6];
        triangles = new int[3 * 6];
        uvs = new Vector2[3 * 6];
        for (HexDirection dir = HexDirection.NE; dir <= HexDirection.NW; dir++)
        {
            int currentIndex = i;
            vertices[i++] = transform.position;
            vertices[i++] = transform.position + HexMetrics.GetFirstSolidCorner(dir);
            vertices[i++] = transform.position + HexMetrics.GetSecondSolidCorner(dir);
            triangles[currentIndex] = currentIndex;
            triangles[currentIndex + 1] = currentIndex + 1;
            triangles[currentIndex + 2] = currentIndex + 2;

            float uvOffsetX = HexMetrics.OuterRadius - transform.position.x;
            float uvOffsetZ = HexMetrics.InnerRadius - transform.position.z;
            uvs[currentIndex] = new Vector2(vertices[currentIndex].x + uvOffsetX, vertices[currentIndex].z + uvOffsetZ) / (HexMetrics.OuterRadius * 2);
            uvs[currentIndex + 1] = new Vector3(vertices[currentIndex + 1].x + uvOffsetX, vertices[currentIndex + 1].z + uvOffsetZ) / (HexMetrics.OuterRadius * 2);
            uvs[currentIndex + 2] = new Vector3(vertices[currentIndex + 2].x + uvOffsetX, vertices[currentIndex + 2].z + uvOffsetZ) / (HexMetrics.OuterRadius * 2);
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void SetMaterial(Material material)
    {
        if(meshRenderer != null)
        {
            meshRenderer.material = material;
        }
    }

    [SerializeField]
    public HexCell[] hexNeighbours;

    public void SetNeighbour(HexDirection direction, HexCell cell)
    {
        hexNeighbours[(int)direction] = cell;
        if(cell != null)
            cell.hexNeighbours[(int)direction.Opposite()] = this;
    }

    public HexCell GetNeighbour(HexDirection direction)
    {
        return hexNeighbours[(int)direction];
    }

    public int DistanceTo(HexCell cell)
    {
        if (cell == this)
            return 0;
        return (((coordinate.X > cell.coordinate.X) ? coordinate.X - cell.coordinate.X : cell.coordinate.X - coordinate.X) +
            ((coordinate.Y > cell.coordinate.Y) ? coordinate.Y - cell.coordinate.Y : cell.coordinate.Y - coordinate.Y) +
            ((coordinate.Z > cell.coordinate.Z) ? coordinate.Z - cell.coordinate.Z : cell.coordinate.Z - coordinate.Z)) / 2;
    }

    public bool CanbeDestination()
    {
		//return (hexType != HexType.Mountain && pawn == null && building == null && gameEventDisplayer == null);
		return (hexType != HexType.Mountain&&hexType!=HexType.Stones&&hexType!=HexType.Thorns && pawn == null );
    }
	
	public bool CanbeEventDestination()
	{
		return CanbeDestination()&& building == null && gameEventDisplayer == null;
	}

    public bool CanbeCellConstructTarget()
    {
        // stub
        // Todo: add requirments for utilizing a cell
        return CanbeDestination() && hexType == HexType.Plain &&building==null &&gameEventDisplayer == null;
    }

    public bool CanbeAttackTargetOf(HexCell fromCell)
    {
        return (pawn != null && fromCell.pawn != null && fromCell.pawn.pawnType != pawn.pawnType);
    }

}
