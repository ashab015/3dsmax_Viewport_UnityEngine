using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = this.transform.position;
        this.transform.position = pos + new Vector3(0.01f, 0.0f, 0.0f);

	}
}
