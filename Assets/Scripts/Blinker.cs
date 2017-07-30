using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour {
	public GameObject blinkObject;

	float lastChanged = 0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		lastChanged += Time.deltaTime;
		if (lastChanged >= 0.5f) {
			blinkObject.gameObject.SetActive (!blinkObject.gameObject.activeSelf);
			lastChanged = 0f;
		}
	}
}
