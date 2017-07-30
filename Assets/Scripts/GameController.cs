using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Click {
	public Vector2 coords { get; set; }
	public int size { get; set; }
}

public class GameController : MonoBehaviour {

	public Text batteryText;
	public Image batteryLevelImage;
	public Text statsText;
	public Text highScoreText;
	public Text newHighScoreText;
	public GameObject gameOverContainer;
	public Button startGameButton;

	public Text mutedText;

	public float batteryLoss = 0.1f;
	public float decreaseDelay;

	public bool gameOver = false;

	public bool running = false;

	public List<Click> clicks = new List<Click> ();

	bool muted = false;

	float waitBeforeDecrease = 0.0f;

	float fullBatteryLevel;
	float batteryLevel = 100f;

	int points = 0;
	int numClicks = 0;
	float startTime;

	float prevPitch = 0.5f;
	int highscore = 0;

	bool newHighscore = false;
	float newHighscoreTextBlinked = 0f;

	float timeSinceLastClick = 0.0f;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey ("highscore")) {
			highscore = PlayerPrefs.GetInt ("highscore");
		}

		if (PlayerPrefs.HasKey ("muted")) {
			muted = PlayerPrefs.GetInt ("muted") == 1 ? true : false;
		}

		if (!muted) {
			GameObject.Find ("MusicPlayer").GetComponent<AudioSource> ().Play ();
			mutedText.text = "Mute";
		}

		highScoreText.text = "Highscore: " + highscore.ToString();

		fullBatteryLevel = batteryLevelImage.rectTransform.rect.width;
	}
	
	// Update is called once per frame
	void Update () {
		// Blink "New highscore" text
		if (newHighscore) {
			newHighscoreTextBlinked += Time.deltaTime;
			if (newHighscoreTextBlinked >= 0.5f) {
				newHighScoreText.gameObject.SetActive (!newHighScoreText.gameObject.activeSelf);
				newHighscoreTextBlinked = 0f;
			}
		}

		// Do not proceed if game hasn't been started or is over
		if (gameOver || !running)
			return;

		// Decrease battery level
		if (waitBeforeDecrease <= 0.0f) {
			batteryLevel -= batteryLoss * Time.deltaTime;
			if (batteryLevel <= 0.0f) {
				batteryLevel = 0.0f;
				GameOver ();

				return;
			}
		} else {
			waitBeforeDecrease -= Time.deltaTime;
		}

		// Set battery level text
		batteryText.text = (int)batteryLevel + "%";

		// Change width of batter level image
		batteryLevelImage.rectTransform.sizeDelta = new Vector2 ((fullBatteryLevel / 100) * batteryLevel, batteryLevelImage.rectTransform.sizeDelta.y);

		// Change color of image based on battery level
		if (batteryLevel >= 50f) {
			batteryLevelImage.color = Color.green;
		} else if (batteryLevel > 20f) {
			batteryLevelImage.color = new Color(1f, 0.54f, 0f);
		} else {
			batteryLevelImage.color = Color.red;
		}

		System.TimeSpan ts = new System.TimeSpan (0, 0, (int)(Time.time - startTime));

		string txt = "Time: " + GetTimeString (ts) + "\n";
		txt = txt + "Clicks: " + numClicks + "\n";
		txt = txt + "\n";
		txt = txt + "Score: " + points;
		statsText.text = txt;

		timeSinceLastClick += Time.deltaTime;
	}

	public void StartGame() {
		startGameButton.gameObject.SetActive (false);
		running = true;
		startTime = Time.time;
	}
		
	public void ToggleSound() {
		if (muted) {
			muted = false;
			mutedText.text = "Mute";
			GameObject.Find ("MusicPlayer").GetComponent<AudioSource> ().Play ();
			PlayerPrefs.SetInt ("muted", 0);
		} else {
			muted = true;
			mutedText.text = "Unmute";
			GameObject.Find ("MusicPlayer").GetComponent<AudioSource> ().Stop ();
			PlayerPrefs.SetInt ("muted", 1);
		}
	}

	public void ExitGame() {
		Application.Quit ();
	}

	void GameOver() {
		gameOver = true;
		gameOverContainer.SetActive (true);

		if (points > highscore) {
			PlayerPrefs.SetInt ("highscore", points);
			newHighscore = true;
		}
	}

	public int AddClick(Click click) {
		clicks.Add (click);
		int lastClick = clicks.Count - 1;

		int p = 5;
		if (lastClick > 0) {
			float distance = Vector2.Distance (clicks [lastClick - 1].coords, clicks [lastClick].coords);
			p = (int)Mathf.Floor (distance / 20);
			p = p * (100 / clicks [lastClick].size);
			p = (int)(p / timeSinceLastClick);
		}
		IncreasePoints (p);
		timeSinceLastClick = 0.0f;

		return (int)p;
	}

	public void IncreaseBatteryLevel() {
		waitBeforeDecrease += decreaseDelay;
	}

	public void IncreasePoints(int incPoints) {
		points += incPoints;
		numClicks++;

		if (!muted) {
			AudioSource audio = GetComponent<AudioSource> ();
			prevPitch += 0.0125f;
			audio.pitch = prevPitch;
			audio.Play ();
		}
	}

	string GetTimeString(System.TimeSpan ts) {
		return ts.Minutes.ToString ("D2") + ":" + ts.Seconds.ToString ("D2");
	}

	public void NewGame() {
		SceneManager.LoadScene ("Game");
	}
}
