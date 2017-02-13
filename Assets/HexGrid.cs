using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HexGrid : MonoBehaviour {

    public struct GridCell
    {
        public static GridCell Empty = new GridCell();

        public GameObject go;
        public Vector3 pos;
        public bool ok;

        public int x;
        public int z;
    }

    GridCell[,] cells;
    int[,] pathCells;

    const float radius = 2.0f;
	// Use this for initialization
	void Start () {
       Generate(); 
	}
	
	// Update is called once per frame
	void Update () {
        
    }


    void Generate()
    {
        cells = new GridCell[110,80];
        

        for (int x = -59; x < 39; x++)
        {
            for (int z = -19; z < 49; z++)
            {
                float xx = transform.position.x + x * radius * 0.866f;
                float zz = transform.position.z + z * radius;
                if (Mathf.Abs(x) % 2 == 1)
                    zz += radius * 0.5f;

                NavMeshHit hit;
                bool didHit = NavMesh.SamplePosition(new Vector3(xx, 0, zz), out hit, 2.5f, NavMesh.AllAreas);
                if (didHit)
                {
                    if (Mathf.Abs(hit.position.x-xx) < 0.1 && Mathf.Abs(hit.position.z - zz) < 0.1)
                    {
                        GameObject go = GameObject.Instantiate(this.gameObject);
                        go.transform.position = new Vector3(xx, hit.distance, zz);
                        go.transform.localScale = new Vector3(1.2f, 1, 1.2f);
                        go.transform.SetParent(transform.parent);
                        go.GetComponent<HexGrid>().enabled = false;

                        cells[x + 60, z + 20].ok = true;
                        cells[x + 60, z + 20].pos = new Vector3(xx, hit.distance, zz);
                        cells[x + 60, z + 20].go = go;
                        cells[x + 60, z + 20].go.SetActive(false);
                        cells[x + 60, z + 20].x = x + 60;
                        cells[x + 60, z + 20].z = z + 20;
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }


    public GridCell GetNearestCell(Vector3 pos)
    {
        float minD = float.MaxValue;
        GridCell c = GridCell.Empty;
        foreach (var cell in cells)
        {
            if (cell.ok) {
                float d = Vector3.Distance(cell.pos, pos);
                if (d < minD)
                {
                    c = cell;
                    minD = d;
                }
            }
        }
        return c;
    }


    public List<GridCell> GetPath(Vector3 from, Vector3 to)
    {
        bool found = false;
        List<GridCell> path = new List<GridCell>();

        GridCell cellFrom = GetNearestCell(from);
        GridCell cellTo = GetNearestCell(to);

        if (cellFrom.x == cellTo.x && cellFrom.z == cellTo.z)
            return path;

        pathCells = new int[cells.GetUpperBound(0)+1, cells.GetUpperBound(1)+1];

        int wave = 1;
        pathCells[cellFrom.x, cellFrom.z] = wave;

        while (!found)
        {
            int add = 0;
            foreach (var cell in cells)
            {
                if (cell.ok)
                {
                    if ((pathCells[cell.x, cell.z] & 0xFFFF) == wave)
                    {
                        
                      //  cell.go.SetActive(true);
                        
                        if (pathCells[cell.x + 1, cell.z + 0] == 0)
                        {
                            pathCells[cell.x + 1, cell.z + 0] = (1 << 16) | (wave + 1);
                            add++;
                        }
                        if (pathCells[cell.x - 1, cell.z + 0] == 0)
                        {
                            pathCells[cell.x - 1, cell.z + 0] = (2 << 16) | (wave + 1);
                            add++;
                        }                           
                        if (pathCells[cell.x + 0, cell.z + 1] == 0)
                        {
                            pathCells[cell.x + 0, cell.z + 1] = (3 << 16) | (wave + 1);
                            add++;
                        }
                        if (pathCells[cell.x - 0, cell.z - 1] == 0)
                        {
                            pathCells[cell.x - 0, cell.z - 1] = (4 << 16) | (wave + 1);
                            add++;
                        }
                          

                        if (cell.x % 2 == 0)
                        {
                            if (pathCells[cell.x + 1, cell.z - 1] == 0)
                            {
                                pathCells[cell.x + 1, cell.z - 1] = (5 << 16) | (wave + 1);
                                add++;
                            }
                            if (pathCells[cell.x - 1, cell.z - 1] == 0)
                            {
                                pathCells[cell.x - 1, cell.z - 1] = (6 << 16) | (wave + 1);
                                add++;
                            }
                        }
                        else
                        {
                            if (pathCells[cell.x + 1, cell.z + 1] == 0)
                            {
                                pathCells[cell.x + 1, cell.z + 1] = (7 << 16) | (wave + 1);
                                add++;
                            }
                            if (pathCells[cell.x - 1, cell.z + 1] == 0)
                            {
                                pathCells[cell.x - 1, cell.z + 1] = (8 << 16) | (wave + 1);
                                add++;
                            }
                        }

                    }
                }
            }

            wave++;
          
            if (add == 0)            
                return path;

            if (pathCells[cellTo.x, cellTo.z] > 0)
            {
                found = true;

                int w = pathCells[cellTo.x, cellTo.z] & 0xFFFF;
                int xx = cellTo.x;
                int zz = cellTo.z;

                while (w > 0)
                {
                    path.Add(cells[xx, zz]);
                    int d = (pathCells[xx,zz] & 0x0FFF0000) >> 16;
                    switch (d) {
                        case 1: xx--;  break;
                        case 2: xx++;  break;
                        case 3: zz--; break;
                        case 4: zz++; break;
                        case 5: xx--; zz++; break;
                        case 6: xx++; zz++; break;
                        case 7: xx--; zz--; break;
                        case 8: xx++; zz--; break;
                    }

                    w--;
                }
            }

        }

        path.Reverse();

        return path;
    }

}
