using UnityEngine;
using System.Collections;

public class ViewCube : MonoBehaviour {

    public GameObject Rotater;
    private bool selectionstarted;
    private Vector3 startingmouseposition;
    private Vector3 oldmouseposition;
    private Vector3 currentmouse;
    private bool moving;
    private int raycasthits;
    public float CubeOffset;
    public float Movement;
    public GameObject ZoomPoint;
    public float ZoomSpeed;
    bool mousedrag;
    public float dragamount;
    private Vector3 startingmousedrag;
    private Vector3 lastdragposition;
    private bool scrollstarted;
    private Vector3 scrolllastposition;

    // Use this for initialization
    void Start () {
        Rotater.transform.eulerAngles = new Vector3(0, 0, 0);
        transform.eulerAngles = new Vector3(0, 0, 0);
        selectionstarted = false;
        moving = false;
        raycasthits = 0;
        mousedrag = false;
    }
	
	// Update is called once per frame
	void Update () {


        // Viewcube -------------------------------------------------------------------------------------------------------------------------
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - CubeOffset, Screen.height - CubeOffset, 1.65f + 1f));

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0) == true)
        {
            if (transform.gameObject == transform.gameObject && selectionstarted == false)
            {
                startingmouseposition = Input.mousePosition;
                oldmouseposition = startingmouseposition;
                selectionstarted = true;
                moving = true;
                raycasthits += 1;
            }
        }

        if (moving == true && Input.GetMouseButton(0) == true && Input.mousePosition != oldmouseposition)
        {
            Vector3 direction = (Input.mousePosition - oldmouseposition).normalized;
            direction = new Vector3(direction.y, direction.x, 0) * Movement;

            Vector3 cameraoldpos = Camera.main.transform.eulerAngles;
            Rotater.transform.eulerAngles = cameraoldpos + direction;

            Vector3 cubeoldpos = transform.eulerAngles;
            transform.eulerAngles = cubeoldpos + direction;

            oldmouseposition = Input.mousePosition;
        }

        if (moving == true && Input.GetMouseButtonUp(0) == true)
        {
            moving = false;
            selectionstarted = false;
        }

        currentmouse = Input.mousePosition;

        // Scroll wheel ----------------------------------------------------------------------------------------------------------------------------
        Vector3 mousedirection = (Input.mousePosition - scrolllastposition).normalized;
        if (Input.GetMouseButton(1) == true)
        {         
            Vector3 zoomposition = Camera.main.transform.rotation * (new Vector3(0, 0, mousedirection.y) * ZoomSpeed);
            Vector3 zoompositionr = Rotater.transform.rotation * (new Vector3(0, 0, mousedirection.y) * ZoomSpeed);
            Camera.main.transform.position = Camera.main.transform.position + zoomposition;
            Rotater.transform.position = Rotater.transform.position + zoompositionr;
        }
        scrolllastposition = Input.mousePosition;



        // screen grab
        if (Input.GetMouseButtonDown(2) == true && mousedrag == false)
        {
            mousedrag = true;
            startingmousedrag = Input.mousePosition;
            lastdragposition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2) == true && mousedrag == true)
        {
            Vector3 direction = ((Input.mousePosition - lastdragposition) / 100);
            direction = new Vector3(-direction.x, -direction.y);
            Vector3 drag = Camera.main.transform.rotation * (direction * dragamount);
            Vector3 dragr = Rotater.transform.rotation * (direction * dragamount);
            Camera.main.transform.position = Camera.main.transform.position + drag;
            Rotater.transform.position = Rotater.transform.position + drag;
            lastdragposition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(2) == true && mousedrag == true)
        {
            mousedrag = false;
            lastdragposition = Input.mousePosition;
        }


    }

    bool HasMouseMoved()
    {
        //I feel dirty even doing this 
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

}
