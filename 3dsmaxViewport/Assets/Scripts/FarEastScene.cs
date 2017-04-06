using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class FarEastScene : MonoBehaviour {

    public GameObject fareast;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	     

        if (fareast.activeSelf == true)
        {
            Camera.main.GetComponent<BloomAndFlares>().enabled = true;
            Camera.main.gameObject.GetComponent<SunShafts>().enabled = true;
            Camera.main.gameObject.GetComponent<ColorCorrectionCurves>().enabled = true;
        }
        if (fareast.activeSelf == false)
        {
            Camera.main.gameObject.GetComponent<BloomAndFlares>().enabled = false;
            Camera.main.gameObject.GetComponent<SunShafts>().enabled = false;
            Camera.main.gameObject.GetComponent<ColorCorrectionCurves>().enabled = false;
        }


    }
}
