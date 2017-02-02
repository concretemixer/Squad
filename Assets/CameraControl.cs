using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    Plane ground = new Plane(Vector3.up, Vector3.zero);


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

        if (Input.GetMouseButton(0))
        {
            gameObject.transform.Rotate(Vector3.up, -40 * Time.deltaTime);
        }
        if (Input.GetMouseButton(1))
        {
            gameObject.transform.Rotate(Vector3.up, 40 * Time.deltaTime);
        }

    }
}
