using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Destroy(this.gameObject, 1.0f);
	}
	
}
