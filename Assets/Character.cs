using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    bool snapped = false;
    public HexGrid grid;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!snapped)
        {
            HexGrid.GridCell cell = grid.GetNearestCell(transform.position);
            transform.position = cell.pos;
            snapped = true;
        }		
	}
}
