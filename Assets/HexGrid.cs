using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HexGrid : MonoBehaviour {

    public struct GridCell
    {
        public GameObject go;
        public Vector3 pos;
        public bool ok;
    }

    GridCell[,] cells;

    const float radius = 1.5f;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Generate();
    }


    void Generate()
    {
        cells = new GridCell[110,80];

        for (int x = -60; x < 40; x++)
        {
            for (int z = -20; z < 50; z++)
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
                        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        go.transform.position = new Vector3(xx, hit.distance, zz);
                        go.transform.localScale = new Vector3(1.4f, 1, 1.4f);
                        go.transform.SetParent(transform.parent);

                        cells[x + 60, z + 20].ok = true;
                        cells[x + 60, z + 20].pos = new Vector3(xx, hit.distance, zz);
                        cells[x + 60, z + 20].go = go;
                        cells[x + 60, z + 20].go.SetActive(false);
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }

    public List<GridCell> GetPath(Vector3 from, Vector3 to)
    {
        return new List<GridCell>();
    }
}
