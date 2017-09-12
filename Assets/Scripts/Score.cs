﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{

	// player
	public Text scoreText;
	public Text highScoreText;
	private Player player;
	private float timer = 0.00f;
	private string scene;
	private bool dataSaved = false;

	// singleton
	public static Score Instance;
	public GameSerialization localPlayerData = new GameSerialization ();


	/// <summary>
	/// Awake this instance.
	/// </summary>
	public void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}

		if (Instance != this) {
			Destroy (gameObject);
		}

		scene = SceneManager.GetActiveScene ().name;
		LoadHighScore ();
	}


	/// <summary>
	/// Start this instance.
	/// </summary>
	public void Start ()
	{
		player = GetComponent<Player> ();
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	public void Update ()
	{
		if (player.hasVictory) {
			if (!dataSaved) {
				StopTimer ();
			}
		} else {
			UpdateTimer ();
		}
	}


	/// <summary>
	/// Updates the timer.
	/// </summary>
	/// <returns>The timer.</returns>
	private void UpdateTimer ()
	{
		timer += Time.deltaTime;
		scoreText.text = FormatScore (timer);
	}

	/// <summary>
	/// Stops the timer.
	/// </summary>
	/// <returns>The timer.</returns>
	private void StopTimer ()
	{
		dataSaved = true;
		localPlayerData.Add ("Luc", float.Parse (scoreText.text));

		SaveLoadController.Save (scene);
	}

	/// <summary>
	/// Loads the high score.
	/// </summary>
	/// <returns>The high score.</returns>
	private void LoadHighScore ()
	{
		SaveLoadController.Load (scene);
		localPlayerData.set (SaveLoadController.savedGames);

		if (localPlayerData.score.Count > 0) {
			highScoreText.text = localPlayerData.name [0] + ":" + FormatScore (localPlayerData.score [0]);
		}
	}

	/// <summary>
	/// Formats float to string.
	/// </summary>
	/// <returns>The score.</returns>
	/// <param name="value">Value.</param>
	private string FormatScore (float value)
	{
		return string.Format ("{00:N2}", value);
	}

}
