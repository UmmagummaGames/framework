using UnityEngine;
using System.Collections;

public class BaseGameController : MonoBehaviour {

	/*las funciones del game controller son las siguientes

	Rastrear el estado del juego:
		- Juego corriendo.
		- Juego Pausado.
		- Cargando.
		- Juego terminado.

	*/

	bool paused;
	public GameObject explosionPrefab;

	public virtual void PlayerLostLife()
	{
		//manejar la perdida de vidas del jugador 
	}

	public virtual void SpawnPlayer()
	{
		//genera al jugador 
	}

	public virtual void Respawn()
	{
		// el jugador es regenerado.
	}

	public virtual void StartGame()
	{
		// iniciar las funciones de juego.
	}

	public void Explode(Vector3 aPosition)
	{
		//instanciar una explocion en la posicion del parametro.
		Instantiate(explosionPrefab,aPosition,Quaternion.identity);
	}

	public virtual void EnemyDestroyed(Vector3 aPosition,int pointsValue,int hitByID)
	{
		//manejar el evento.
	}

	public virtual void BossDestroyed()
	{
		//manejar evento.
	}

	public virtual void RestartGameButtonPressed()
	{
		// recarga la escena actualmente cargada.
		Application.LoadLevel(Application.loadedLevelName);
	}

	public bool Paused
	{
		get
		{
			return paused;
		}

		set
		{
			paused = value;

			if(paused)
			{
				Time.timeScale = 0f;
			}else{
				Time.timeScale = 1f;
			}
		}
	}

}
