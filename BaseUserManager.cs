using UnityEngine;
using System.Collections;

public class BaseUserManager : MonoBehaviour {

	/* Este script sera un objeto creado por el PlayerMangar para alamcenar propiedades 
	 * tales como:
	 * 
	 * player name
	 * Current score
	 * Highest score
	 * Level
	 * Health
	 * Cuando o no el jugador a terminado el juego.
	 */

	private int score;
	private int highScore;
	private int level;
	private int health;
	private bool isFinished;

	// este es el nombre del jugador que se mostrara
	public string playername = "Anon";

	public virtual void GetDefaultData()
	{
		playername = "Anon";
		score = 0;
		level = 1;
		health = 3;
		highScore = 0;
		isFinished = false;
	}

	public string GetName()
	{
		return playername;
	}

	public void SetName(string aName)
	{
		playername = aName;
	}

	public int GetLevel()
	{
		return level;
	}

	public void SetLevel(int num)
	{
		level = num;
	}

	public int GetHighScore()
	{
		return highScore;
	}

	public int GetScore()
	{
		return score;
	}

	public virtual void AddScore(int anAmount)
	{
		score += anAmount;
	}

	public void LostScore(int num)
	{
		score -= num;
	}

	public void SetScore(int num)
	{
		score = num;
	}

	public int GetHealth()
	{
		return health;
	}

	public void AddHealth(int num)
	{
		health += num;
	}

	public void ReduceHealth(int num)
	{
		health -= num;
	}

	public void SetHealth(int num)
	{
		health = num;
	}

	public bool GetIsFinished()
	{
		return isFinished;
	}

	public void SetIsFinished(bool aVal)
	{
		isFinished = aVal;
	}
}
