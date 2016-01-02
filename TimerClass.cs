using UnityEngine;
using System.Collections;

public class TimerClass : MonoBehaviour {

	public bool isTimeRunning = false;
	private float timeElapsed = 0.0f;
	private float currentTime = 0.0f;
	private float lastTime = 0.0f;
	private float timeScaleFactor = 1.1f; // cambiar escala de tiempo aca.


	private string timeString;
	private string hour;
	private string minutes;
	private string seconds;
	private string mills;

	private int aHour;
	private int aMinute;
	private int aSecond;
	private int aMillis;
	private int tmp;
	private int aTime;

	private GameObject callback;	

	public void UpdateTimer(){
		//calcula el tiempo transcurrido desde el ultimo Update()
		timeElapsed = Mathf.Abs(Time.realtimeSinceStartup-lastTime);

		//si el timer esta corriendo, añadimos el tiempo transcurrido al tiempo actual (avanzando el timer)
		if(isTimeRunning){
			currentTime += timeElapsed*timeScaleFactor;
		}

		//almacena el tiempo actual asi podemos usarlo en la proxima actualizacion 
		lastTime = Time.realtimeSinceStartup;
	}

	public void StartTimer(){
		// configura las variables iniciales para iniciar el timer
		isTimeRunning = true;
		lastTime = Time.realtimeSinceStartup;
	}

	public void StopTimer(){
		//para el timer
		isTimeRunning = false;
	}

	public void ResetTimer(){
		// pondra el timer en ceros 
		timeElapsed = 0.0f;
		currentTime = 0.0f;
		lastTime = Time.realtimeSinceStartup;
	}

	public string GetFormattedTime(){
		//actualiza el timer 
		UpdateTimer();

		//minutos
		aMinute = (int)currentTime/60;
		aMinute = aMinute%60;

		//segundos
		aSecond = (int)currentTime%60;

		//millisegundos
		aMillis = (int)(currentTime*100)%100;

		//formatea strings 
		tmp = (int)aSecond;
		seconds = tmp.ToString();
		if(seconds.Length < 2)
			seconds = "0"+seconds;

		tmp = (int)aMinute;
		minutes = tmp.ToString();
		if(minutes.Length < 2)
			minutes = "0"+minutes;

		tmp = (int)aMillis;
		mills = tmp.ToString();
		if(mills.Length < 2)
			mills = "0"+mills;

		//poner todo junto 
		timeString = minutes + ":" + seconds + ":" + mills;

		return timeString;
	}

	public int GetTime(){
		//recordar llamar la funcion UpdateTimer() antes de usar esta funcion
		// de otra manera el tiempo no estar actualizado.
		return (int)(currentTime);
	}
}
