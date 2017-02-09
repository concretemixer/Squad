using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateHexGrid : MonoBehaviour {

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
        for (int x = -60; x < 40; x++)
        {
            for (int z = -40; z < 60; z++)
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
                        go.transform.localScale = new Vector3(1.5f, 1, 1.5f);
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }
}
