using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayController : MonoBehaviour {

	public GameObject circlePrefab;

	GameObject playArea;
	GameController gameController;

	public int[] sizes;
	public Color[] colors;

	// Settings

	// Internal
	public bool spawned = false;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();

		playArea = GameObject.Find ("PlayArea");
	}
	
	// Update is called once per frame
	void Update () {
		if (!spawned && gameController.running) {
			SpawnCircle ();
		}
	}

	void SpawnCircle () {
		if (spawned)
			return;
		
		GameObject circle = (GameObject)Instantiate (circlePrefab, playArea.transform);
		int size = sizes [Random.Range (0, sizes.Length)];
		circle.GetComponent<Image> ().rectTransform.sizeDelta = new Vector2 (size, size);
		circle.GetComponent<Image> ().color = colors [Random.Range (0, colors.Length)];

		float minX = (playArea.GetComponent<RectTransform> ().rect.size.x / 2) - (size / 2);
		float minY = (playArea.GetComponent<RectTransform> ().rect.size.y / 2) - (size / 2);

		// Get previous click vector
		Vector3 newPos;
		if (gameController.clicks.Count == 0) {
			newPos = new Vector3 (0, 0, 0);
		} else {
			Vector2 prevCoords = gameController.clicks [gameController.clicks.Count - 1].coords;
			Vector3 prevPos = new Vector3 (prevCoords.x, prevCoords.y, 0);

			float distance;
			int count = 0;
			do {
				newPos = new Vector3 (Random.Range (-minX, minX), Random.Range (-minY, minY));
				distance = Vector3.Distance (prevPos, newPos);
				count++;
				Debug.Log ("Pass " + count + ": " + distance);
			} while (distance < 50);
		}


		circle.transform.localPosition = newPos;

		spawned = true;
	}
}
