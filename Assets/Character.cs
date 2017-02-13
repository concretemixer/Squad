using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {


    enum Mode { Idle, Walk, Attack }
    Mode mode = Mode.Idle;

    HexGrid.GridCell destination;
    List<HexGrid.GridCell> path;
    float speed = 0.0f;
    float speedMax = 3.0f;
    float acc = 1.0f;


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


        if (mode == Mode.Walk)
        {
            Vector3 dir = path[0].pos - transform.position;
            if (dir.magnitude < 0.1)
            {
                path.RemoveAt(0);
                if (path.Count == 0)
                {
                    mode = Mode.Idle;
                    GetComponent<Animator>().SetFloat("Speed_f", 0);
                }
                
            }
            else 
            {
                speed += acc * Time.deltaTime;
                if (speed > speedMax)
                    speed = speedMax;

                dir.Normalize();
                transform.position += speed * Time.deltaTime * dir;
                Quaternion t = Quaternion.LookRotation(dir);

                dir.y = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, t, 100 * Time.deltaTime);

                GetComponent<Animator>().SetFloat("Speed_f",speed+0.25f);
            }
        }

	}

    public void OnClick() {

        if (mode != Mode.Idle)
            return;

        Plane ground = new Plane(Vector3.up, Vector3.zero);
        Camera camera = Camera.main;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        float rayDistance;
        if (ground.Raycast(ray, out rayDistance))
        {
            Vector3 touchGround = ray.origin + ray.direction * rayDistance;
            HexGrid.GridCell cell = grid.GetNearestCell(touchGround);

            mode = Mode.Walk;
            destination = cell;
            path = grid.GetPath(transform.position, touchGround);
        }
    }
}
