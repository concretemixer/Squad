using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour {

    public HexGrid grid;

    public Character hero;
    Plane ground = new Plane(Vector3.up, Vector3.zero);

    bool to = false;
    Vector3 from;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(2))
        {
            Camera camera = Camera.main;

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            float rayDistance;
            if (ground.Raycast(ray, out rayDistance))
            {
                Vector3 touchGround = ray.origin + ray.direction * rayDistance;

                gameObject.transform.position = touchGround;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            hero.OnClick();

        }
        if (Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.LeftControl))
                gameObject.transform.Rotate(Vector3.up, -40 * Time.deltaTime);
            else
                gameObject.transform.Rotate(Vector3.up, 40 * Time.deltaTime);
        }

    }
}
