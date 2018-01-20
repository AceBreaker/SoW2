using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    Vector3 defaultCamPosition;
    public float distance = -1.0f;

	// Use this for initialization
	void Start () {
        defaultCamPosition = this.transform.position;
        Input.simulateMouseWithTouches = true;
        //Input.uses
	}
	
	// Update is called once per frame
	void Update () {
        float movementSpeed = 0.1f;
        float mobileMoveSpeed = 0.05f;
	    if(Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(new Vector3(0.0f, movementSpeed));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(new Vector3(0.0f, -movementSpeed));

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(new Vector3(-movementSpeed, 0.0f));

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(new Vector3(movementSpeed, 0.0f));

        }

        if(Input.touchCount == 1 )
        {
            this.transform.Translate(Input.touches[0].deltaPosition * -mobileMoveSpeed);
        }
        if(Input.touchCount > 4)
        {
            this.transform.position = defaultCamPosition;
        }

        float dist = -1.0f;
        if(Input.touchCount == 2 )
        {
            dist = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

            if(distance > 0.0f)
            {
                GetComponent<Camera>().orthographicSize -= (dist - distance) * 0.1f;
                if (GetComponent<Camera>().orthographicSize <= 4)
                    GetComponent<Camera>().orthographicSize = 4;
            }

        }
        distance = dist;

        GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 4.0f;
    }
}
