using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public string[] levelNames; // todos los nombres de los niveles del juego.
	public int gameLevelNum; // este es el index que nos dara el nombre del nivel que necesitamos.

	public void Start()
	{
		// mantiene este objeto con vida.
		DontDestroyOnLoad(this.gameObject);
	}

	public void LoadLevel(string sceneName)
	{
		//carga el nivel que necesitamos
		Application.LoadLevel(sceneName);
	}

	public void GoNextLevel()
	{
		//aumentamos el index del gameLevel, si llega al maximo lo reiniciamos 
		if(gameLevelNum >= levelNames.Length)
		{
			gameLevelNum = 0;
		}

		// tambien por medio de esta funcion cargamos el siguiente nivel 
		LoadLevel(gameLevelNum);

		//aumentamos el contador 
		gameLevelNum++;

	}

	private void LoadLevel(int indexNum)
	{
		// cargamos el nivel de juego
		Application.LoadLevel(levelNames[gameLevelNum]);
	}

	public void ResetGame()
	{
		// resetea el contador de niveles 
		gameLevelNum = 0;
	}
}
