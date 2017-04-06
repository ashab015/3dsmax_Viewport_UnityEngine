using UnityEngine;
using System.Collections;

public class HideShow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HideToggle()
    {
        if (transform.gameObject.activeSelf == false)
        {
            transform.gameObject.SetActive(true);
            return;
        }
        if (transform.gameObject.activeSelf == true)
        {
            transform.gameObject.SetActive(false);
            return;
        }

    }

}
