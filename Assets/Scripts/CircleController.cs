﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircleController : MonoBehaviour, IPointerClickHandler {

	PlayController playController;
	GameController gameController;
	GameObject playArea;

	public GameObject gotPointsText;

	// Use this for initialization
	void Start () {
		playController = GameObject.Find ("PlayController").GetComponent<PlayController> ();
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		playArea = GameObject.Find ("PlayArea");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerClick(PointerEventData eventData) {
		int size = (int)GetComponent<RectTransform> ().rect.size.x;

		Click click = new Click ();
		click.coords = new Vector2 (transform.localPosition.x, transform.localPosition.y);
		click.size = size;

		playController.spawned = false;
		gameController.IncreaseBatteryLevel ();

		int pts = gameController.AddClick (click);

		transform.position = new Vector2 (1000, 1000);
		Destroy (gameObject, 2.0f);

		// Spawn points text
		GameObject gpt = (GameObject)Instantiate (gotPointsText, playArea.transform);
		gpt.transform.localPosition = click.coords;
		gpt.GetComponent<Text> ().text = "+" + pts;
		StartCoroutine(FadeText(2.0f, gpt.GetComponent<Text>()));
		Destroy (gpt, 2.0f);
	}

	public IEnumerator FadeText(float fadeTime, Text txt) {
		while (txt.color.a > 0.0f) {
			txt.color = new Color (txt.color.r, txt.color.g, txt.color.b, txt.color.a - (Time.deltaTime / fadeTime));
			yield return null;
		}
		Destroy (txt);
	}

}