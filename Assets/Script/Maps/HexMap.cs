using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{

    //public Material plainMat;
	//private Texture2D[] plainTex;
    private const string texturePath = "Environment/GroundTex";

    private List<Material> plainMat=new List<Material>();
	public Material myMat;

    public int mapWidth = 40;
    public int mapHeight = 30;

    [Range(0, 100)]
    public int swampFactor = 20;
    [Range(0, 100)]
    public int mountFactor = 20;
    [Range(0, 100)]
    public int forestFactor = 20;
    [Range(0, 100)]
    public int thornsFactor = 1;
    [Range(0, 100)]
    public int stoneSFactor = 1;

    public HexTypeInfo hexPrefab_forest;
    public HexTypeInfo hexPrefab_swamp;
    public HexTypeInfo hexPrefab_mountain;
    public Indicator indicatorPrefab;
    public HexTypeInfo hexPrefab_stones;
    public HexTypeInfo hexPrefab_thorns;
    public HexCell hexCellPrefab;

    private HexCell[] cells;
    private HexType[] hexTypes;
    private Indicator[] indicators;

    private List<HexCell> currentRoutes;
    private List<HexCell> reachableCells;
    private List<HexCell> attackableCells;
    private List<HexCell> hiddenCells;
    private List<HexCell> friendCells;
    private List<HexCell> emptyCells;

    public int revealRadius = 2;
	
	private int pathLength;
	
    public void OnEnable()
    {
        
    }

    public void GenerateCells()
    {
        cells = new HexCell[mapHeight * mapWidth];
        hexTypes = new HexType[mapHeight * mapWidth];
        indicators = new Indicator[mapHeight * mapWidth];
        currentRoutes = new List<HexCell>();
        reachableCells = new List<HexCell>();
        attackableCells = new List<HexCell>();
        friendCells = new List<HexCell>();
        hiddenCells = new List<HexCell>();
        emptyCells = new List<HexCell>();
		pathLength=0;
		CreateMat();
        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                HexCell cell = cells[x + z * mapWidth] = Instantiate<HexCell>(hexCellPrefab);
                CreateCell(cell, x, z);
                GenerateHexType(cell, x, z);
                CreateAttachIndicator(cell, x, z);
            }
        }

        ConnectNeighbours();
    }

    private void ConnectNeighbours()
    {
        // Setting neighbours
        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (x < mapWidth - 1)
                    cells[x + z * mapWidth].SetNeighbour(HexDirection.E, cells[x + z * mapWidth + 1]);
                if (z % 2 == 1 && z < mapHeight - 1)
                {
                    if (x < mapWidth - 1)
                    {
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NW, cells[x + z * mapWidth + mapWidth]);
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NE, cells[x + z * mapWidth + mapWidth + 1]);
                    }
                    else
                    {
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NW, cells[x + z * mapWidth + mapWidth]);
                    }

                }
                else if (z % 2 == 0 && z < mapHeight - 1)
                {
                    if (x > 0)
                    {
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NE, cells[x + z * mapWidth + mapWidth]);
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NW, cells[x + z * mapWidth + mapWidth - 1]);
                    }
                    else
                    {
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NE, cells[x + z * mapWidth + mapWidth]);
                    }
                }
            }
        }
    }
    /*
     * Fog of Hexs
     */
    public void HideCells()
    {
        foreach(HexCell cell in hiddenCells)
        {
            cell.gameObject.SetActive(false);
        }
    }

    public void RevealCell(HexCell cell)
    {
        cell.gameObject.SetActive(true);
    }

    public void RevealCellsFrom(HexCell cell)
    {
        Queue<HexCell> revealCells = new Queue<HexCell>();
        revealCells.Enqueue(cell);

        while(revealCells.Count > 0)
        {
            HexCell cellToReveal = revealCells.Dequeue();
            RevealCell(cellToReveal);
            hiddenCells.Remove(cellToReveal);
            for (HexDirection dir = (HexDirection)0; dir <= HexDirection.NW; dir++)
            {
                HexCell nextCell = cellToReveal.GetNeighbour(dir);
                if(nextCell != null && nextCell.DistanceTo(cell) <= revealRadius)
                {
                    if(hiddenCells.Contains(nextCell))
                        revealCells.Enqueue(nextCell);
                }
            }
        }

    }

    public void UpdateHideCells()
    {
        // stub
    }
	
	public void CreateMat()
	{
		Object[] go = Resources.LoadAll(texturePath, typeof(Texture2D));
		for(int i=0;i<go.Length;i++)
		{
			Material mat=new Material(Shader.Find("Custom/HexCell"));
            //plainTex[i] = (Texture2D)go[i];
			mat.SetTexture("_MainTex", (Texture2D)go[i]);
			plainMat.Add(mat);
		}
	}

    private void CreateCell(HexCell cell, int x, int z)
    {
		int ran = Random.Range(0, plainMat.Count);
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2);
        position.y = 0;
        position.z = z * (HexMetrics.OuterRadius * 1.5f);

        cell.InitHexCell();
        cell.GenerateMesh();
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.SetMaterial(plainMat[ran]);
		//Debug.Log("cell created");
        cell.coordinate = HexCoordinate.FromOffsetCoordinate(x, z);
        hiddenCells.Add(cell);
    }

    private void GenerateHexType(HexCell cell, int x, int z)
    {
        HexType type = GetRandomHexType();
        hexTypes[x + z * mapWidth] = cell.hexType = type;
        
        if (type != HexType.Plain)
        {
            HexTypeInfo gm = null;
            if(type == HexType.Forest)
                gm = Instantiate(hexPrefab_forest);
            else if(type == HexType.Swamp)
                gm = Instantiate(hexPrefab_swamp);
            else if(type == HexType.Mountain)
                gm = Instantiate(hexPrefab_mountain);
			else if(type==HexType.Stones)
				gm = Instantiate(hexPrefab_stones);
			else if(type==HexType.Thorns)
				gm = Instantiate(hexPrefab_thorns);

            if(gm != null)
            {
                gm.ChangeType(type);
                gm.gameObject.transform.SetParent(cell.transform);
                gm.gameObject.transform.localPosition = Vector3.zero;
            }
        }
    }


    private void CreateAttachIndicator(HexCell cell, int x, int z)
    {
        Indicator indicator = cell.indicator = indicators[x + z * mapWidth] = Instantiate(indicatorPrefab);
        indicator.transform.SetParent(cell.transform);
        indicator.SetColor(Indicator.EndColor);
        indicator.transform.localPosition = Vector3.zero;
        indicator.gameObject.SetActive(false);
    }

    public HexType GetRandomHexType()
    {
        float probability = (float)Random.Range(1, 100);
        
        if (probability < swampFactor)
            return HexType.Swamp;
        else if (probability < swampFactor + mountFactor)
            return HexType.Mountain;
        else if (probability < swampFactor + mountFactor + forestFactor)
            return HexType.Forest;
		else if(probability < swampFactor + mountFactor + forestFactor+stoneSFactor)
			return HexType.Stones;
		else if(probability < swampFactor + mountFactor + forestFactor+stoneSFactor+thornsFactor)
			return HexType.Thorns;
        return HexType.Plain;
    }

    public Vector3 GetCenterPoint()
    {
        Vector3 point;
        point.x = HexMetrics.InnerRadius * 2 * (mapWidth / 2);
        point.y = 0;
        point.z = HexMetrics.OuterRadius * 1.5f * (mapHeight / 2);
        return point;
    }

    public HexCell selectedCell;

    public void SelectHex(Vector3 point)
    {
        UnselectHex();
        HexCoordinate hexCoord = HexCoordinate.FromPosition(transform.InverseTransformPoint(point));
        int index = hexCoord.X + hexCoord.Z * mapWidth + hexCoord.Z / 2;
        selectedCell = cells[index];
        selectedCell.indicator.gameObject.SetActive(true);
        selectedCell.indicator.SetColor(Indicator.StartColor);
    }

    public void UnselectHex()
    {
        if(selectedCell != null)
        {
            selectedCell.indicator.gameObject.SetActive(false);
        }
    }

    public Rect GetBorder()
    {
        Rect borders = new Rect();
        borders.xMin = 0f;
        borders.xMax = HexMetrics.InnerRadius * 2f * mapWidth;
        borders.yMin = 0f;
        borders.yMax = HexMetrics.OuterRadius * 1.5f * mapHeight;
        return borders;
    }

    public void FindPath(HexCell fromCell, HexCell toCell)
    {
        if (fromCell == toCell || !toCell.CanbeDestination())
            return;

        List<HexCell> cellToFind = new List<HexCell>();
        currentRoutes.Clear();

        // Set distance to max value
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].prevCell = null;
        }

        fromCell.Distance = 0;
        cellToFind.Add(fromCell);
        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);
            if (cell == toCell)
                break;

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    int distance = cell.Distance;
                    if (!nextCell.CanbeDestination())
                        continue;
                    else if (nextCell.hexType == HexType.Plain)
                        distance = cell.Distance + 1;
                    else if (nextCell.hexType == HexType.Forest || nextCell.hexType == HexType.Swamp)
                        distance = cell.Distance + 2;


                    nextCell.heuristicDistance = nextCell.DistanceTo(toCell);
                    if (nextCell.Distance == int.MaxValue)
                    {
                        distance += nextCell.heuristicDistance;
                        nextCell.Distance = distance;
                        cellToFind.Add(nextCell);
                        nextCell.prevCell = cell;
                    }
                    else if (nextCell.Distance > distance + nextCell.heuristicDistance)
                    {
                        nextCell.Distance = distance + nextCell.heuristicDistance;
                        nextCell.prevCell = cell;
                    }
                }
            }
            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }
		pathLength=0;
        currentRoutes.Clear();
        if (toCell.prevCell != null)
        {
            HexCell prev = toCell;
            while(prev != fromCell)
            {
                currentRoutes.Insert(0, prev);
				pathLength+=(prev.hexType == HexType.Plain)?1:2;
                prev = prev.prevCell;
            }
        }
    }
	
	public int GetPathLength()
	{
		return pathLength;
	}

    public HexCell GetEmptyNearestCellAround(HexCell startcell)
    {
        if (startcell == null)
            return null;

        List<HexCell> cellToFind = new List<HexCell>();

        // Set distance to max value
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].prevCell = null;
        }

        startcell.Distance = 0;
        cellToFind.Add(startcell);
        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    if (nextCell.Distance == int.MaxValue)
                    {
                        nextCell.Distance = 0;
                        cellToFind.Add(nextCell);
                    }
                }
                if (cell != startcell && cell.CanbeDestination())
                    return cell;
            }
            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }
        return null;
    }

    public List<HexCell> GetRoutes(Pawn pawn, HexCell Target)
    {
        List<HexCell> routes = new List<HexCell>();

        FindPath(pawn.currentCell, GetEmptyNearestCellAround(Target));

        int pathDistance = 0;
        for(int i = 0; i < currentRoutes.Count-1; i++)
        {
            pathDistance += currentRoutes[i].hexType == HexType.Plain ? 1 : 2;
            if (pathDistance <= pawn.GetDexterity())
                routes.Add(currentRoutes[i]);
            else
                break;
        }

        currentRoutes = routes;

        return currentRoutes;
    }

    public void ShowPath(HexCell fromCell, HexCell toCell)
    {
        HideIndicator();
        if (fromCell == null || toCell == null || fromCell == toCell)
            return;
        fromCell.indicator.gameObject.SetActive(true);
        fromCell.indicator.SetColor(Indicator.StartColor);
        toCell.indicator.gameObject.SetActive(true);
        toCell.indicator.SetColor(Indicator.EndColor);

        HexCell prev = toCell.prevCell;
        while(prev != fromCell && prev != null)
        {
            prev.indicator.gameObject.SetActive(true);
            prev.indicator.SetColor(Indicator.RouteColor);
            prev = prev.prevCell;
        }
    }

    public void FindReachableCells(HexCell startCell, int maxDistance)
    {
        List<HexCell> cellToFind = new List<HexCell>();
        
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        startCell.Distance = 0;
        cellToFind.Add(startCell);
        reachableCells.Clear();

        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    int distance = cell.Distance;
                    if (!nextCell.CanbeDestination())
                        continue;
                    else if (nextCell.hexType == HexType.Plain)
                        distance = cell.Distance + 1;
                    else if (nextCell.hexType == HexType.Forest || nextCell.hexType == HexType.Swamp)
                        distance = cell.Distance + 2;

                    if (nextCell.Distance == int.MaxValue)
                    {
                        nextCell.Distance = distance;
                        if(nextCell.Distance < maxDistance+1)
                            cellToFind.Add(nextCell);
                    }
                    else if (nextCell.Distance > distance)
                    {
                        if (nextCell.Distance > distance)
                        {
                            nextCell.Distance = distance;
                        }
                    }
                    if (cell.Distance <= maxDistance && cell != startCell && cell.hexType != HexType.Mountain && cell.hexType!=HexType.Thorns&&cell.hexType!=HexType.Stones)
                        reachableCells.Add(cell);
                }
            }
            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }

        reachableCells.Remove(startCell);
    }

    public void ShowReachableCells()
    {
        HideIndicator();
        for (int i = 0; i < reachableCells.Count; i++)
        {
            reachableCells[i].indicator.gameObject.SetActive(true);
            reachableCells[i].indicator.SetColor(Indicator.StartColor);
        }
    }

	 public void UpdateBuildableCells(HexCell startCell, int maxDistance, bool buildmode)
    {
        List<HexCell> cellToFind = new List<HexCell>();
        HideIndicator();
		if(!buildmode)
		{
			for (int i = 0; i < cells.Length; i++)
				cells[i].buildable=false;
			return;
		}
        for (int i = 0; i < cells.Length; i++)
        {
            if(startCell.DistanceTo(cells[i])<=maxDistance&&cells[i].CanbeCellConstructTarget())
			{
				cells[i].buildable=true;
				cells[i].indicator.gameObject.SetActive(true);
				cells[i].indicator.SetColor(Indicator.BuildColor);
			}
			else
				cells[i].buildable=false;
        }
    }
	
	public List<HexCell> GetTeleporterBuildableCells(HexCell startCell,int maxDistance)
	{
		List<HexCell> cellToFind = new List<HexCell>();
        //HideIndicator();
        for (int i = 0; i < cells.Length; i++)
        {
            if(startCell.DistanceTo(cells[i])<=maxDistance&&cells[i].hexType==HexType.Plain&&cells[i].pawn==null&&cells[i].building==null)
			{
				cellToFind.Add(cells[i]);
			}
        }
		return cellToFind;
	}

    public void ProbeAttackTarget(HexCell startCell)
    {
        if (startCell.pawn == null)
            return;

        List<HexCell> cellToFind = new List<HexCell>();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        //Debug.Log("Attack range: " + startCell.pawn.currentAttackRange);

        startCell.Distance = 0;
        cellToFind.Add(startCell);
        reachableCells.Clear();

        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    int distance = cell.Distance + 1;
                    if(nextCell.Distance == int.MaxValue)
                    {
                        nextCell.Distance = distance;
                        cellToFind.Add(nextCell);
                    }
                }
            }
            if (cell.Distance <= startCell.pawn.currentAttackRange)
                reachableCells.Add(cell);

            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }

        reachableCells.Remove(startCell);

        attackableCells.Clear();
        friendCells.Clear();
        emptyCells.Clear();
        foreach (HexCell cell in reachableCells)
        {
            if (cell.CanbeAttackTargetOf(startCell))
                attackableCells.Add(cell);
            else if (cell.pawn != null)
                friendCells.Add(cell);
            //else if (false) // is buildings
                //;
            else
                emptyCells.Add(cell);
        }
        if(startCell.pawn != null)
            friendCells.Add(startCell);
    }

    public HexCell GetNearestAttackableTarget(HexCell fromCell, int radius = 20)
    {
        if (fromCell.pawn == null)
            return null;

        List<HexCell> cellToFind = new List<HexCell>();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        fromCell.Distance = 0;
        cellToFind.Add(fromCell);

        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    int distance = cell.Distance + 1;
                    if (nextCell.Distance == int.MaxValue)
                    {
                        nextCell.Distance = distance;
                        cellToFind.Add(nextCell);
                    }
                    else if(nextCell.Distance > distance)
                    {
                        nextCell.Distance = distance;
                    }
                }
            }
            if (cell.Distance <= radius && cell.CanbeAttackTargetOf(fromCell))
            {
                return cell;
            }

            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }
        return null;
    }

    public HexCell GetNearestBuilding(HexCell fromCell, int probeDistance = 30)
    {
        if (fromCell == null)
            return null;

        List<HexCell> cellToFind = new List<HexCell>();

        int maxDistance = probeDistance;

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        fromCell.Distance = 0;
        cellToFind.Add(fromCell);
        reachableCells.Clear();

        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    int distance = cell.Distance;
                    if (!nextCell.CanbeDestination())
                        continue;
                    else if (nextCell.hexType == HexType.Plain)
                        distance = cell.Distance + 1;
                    else if (nextCell.hexType == HexType.Forest || nextCell.hexType == HexType.Swamp)
                        distance = cell.Distance + 2;

                    if (nextCell.Distance == int.MaxValue)
                    {
                        nextCell.Distance = distance;
                        if (nextCell.Distance < maxDistance + 1)
                            cellToFind.Add(nextCell);
                    }
                    else if (nextCell.Distance > distance)
                    {
                        if (nextCell.Distance > distance)
                        {
                            nextCell.Distance = distance;
                        }
                    }
                    if (cell.Distance <= maxDistance && cell != fromCell)
                        reachableCells.Add(cell);

                    if (nextCell.building != null)
                        return nextCell;
                }
            }
            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }

        return null;
    }

    public List<HexCell> GetAttackableTargets()
    {
        return attackableCells;
    }

    public List<HexCell> GetFriendTargets()
    {
        return friendCells;
    }

    public List<HexCell> GetEmptyCells()
    {
        return emptyCells;
    }

    public void ShowAttackCandidates()
    {
        HideIndicator();
        
        for (int i = 0; i < attackableCells.Count; i++)
        {
            attackableCells[i].indicator.gameObject.SetActive(true);
            attackableCells[i].indicator.SetColor(Indicator.AttackColor);
        }
    }

    public void ShowFriendCandidates()
    {
        HideIndicator();

        for (int i = 0; i < friendCells.Count; i++)
        {
            friendCells[i].indicator.gameObject.SetActive(true);
            friendCells[i].indicator.SetColor(Indicator.FriendColor);
        }
    }

    public bool IsReachable(HexCell cell)
    {
        return reachableCells.Contains(cell);
    }


    public List<HexCell> GetCurrentRoutes()
    {
        return currentRoutes;
    }

    public void HideIndicator()
    {
        for (int i = 0; i < cells.Length; i++)
            cells[i].indicator.gameObject.SetActive(false);
        UnselectHex();
    }

    public HexCell GetCellFromPosition(Vector3 point)
    {
        HexCoordinate hexCoord = HexCoordinate.FromPosition(transform.InverseTransformPoint(point));
        int index = hexCoord.X + hexCoord.Z * mapWidth + hexCoord.Z / 2;
        return cells[index];
    }

    public HexCell GetRandomCellToSpawn()
    {
        // make it centered
        int ranX = Random.Range(mapWidth / 4, mapWidth / 4 * 3);
        int ranY = Random.Range(mapHeight / 4, mapHeight / 4 * 3);
        HexCell cell = cells[ranX + ranY * mapWidth];
        while (!cell.CanbeDestination())
        {
            ranX = Random.Range(mapWidth / 4, mapWidth / 4 * 3);
            ranY = Random.Range(mapHeight / 4, mapHeight / 4 * 3);
            cell = cells[ranX + ranY * mapWidth];
        }
            return cell;
    }
	
	public HexCell GetRandomCellToSpawnEvent()
    {
        // make it centered
        int ranX = Random.Range(mapWidth / 4, mapWidth / 4 * 3);
        int ranY = Random.Range(mapHeight / 4, mapHeight / 4 * 3);
        HexCell cell = cells[ranX + ranY * mapWidth];
        while (!cell.CanbeEventDestination())
        {
            ranX = Random.Range(mapWidth / 4, mapWidth / 4 * 3);
            ranY = Random.Range(mapHeight / 4, mapHeight / 4 * 3);
            cell = cells[ranX + ranY * mapWidth];
        }
            return cell;
    }

    public bool SetCharacterCell(Pawn pawn, HexCell cell)
    {
        if (pawn == null || cell == null || !cell.CanbeDestination())
            return false;

        HexCell oldCell = pawn.currentCell;
        if (oldCell != null)
            oldCell.pawn = null;
        pawn.currentCell = cell;
        cell.pawn = pawn;

        pawn.transform.position = cell.transform.position;

        return true;
    }

    public bool SwapPawns(Pawn pawn1, Pawn pawn2)
    {
        if (pawn1 == null || pawn2 == null || pawn1 == pawn2)
            return false;

        HexCell cell1 = pawn1.currentCell;
        HexCell cell2 = pawn2.currentCell;

        pawn1.currentCell = pawn2.currentCell = null;
        cell1.pawn = cell2.pawn = null;

        if (SetCharacterCell(pawn1, cell2) && SetCharacterCell(pawn2, cell1))
            return true;

        // Nothing changed
        pawn1.currentCell = cell1;
        pawn2.currentCell = cell2;
        cell1.pawn = pawn1;
        cell2.pawn = pawn2;
        return false;
    }
	
	public bool SetBuildingCell(Building building,HexCell cell)
	{
		 if (building == null ||cell==null||!cell.CanbeDestination())
            return false;
		cell.building=building;
		building.currentCell=cell;
		
		building.transform.position=cell.transform.position;
		return true;
	}

    public bool SetGameEventDisplayerCell(GameEventDisplayer displayer, HexCell cell)
    {
        if (displayer == null || cell == null || !cell.CanbeDestination())
            return false;
        cell.gameEventDisplayer = displayer;
        displayer.currentCell = cell;

        displayer.transform.position = cell.transform.position;
        return true;
    }

}
