using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Компонент для перемещения по заданному пути
/// </summary>
public class PathMover : MonoBehaviour
{

	/// <summary>
	/// Скорость перемещения (единицы в секунду)
	/// </summary>
	public float speed = 9.0f;

	/// <summary>
	/// Скорость перемещения по болоту (объектам со слоя "Swamp")
	/// </summary>
	public float swampSpeed = 3.0f;

	private LineRenderer _line;

	private List<Labirynth.Point> _path;
	private Labirynth _labirynth;

	/// <summary>
	/// Пройти по заданному пути
	/// </summary>
	/// <param name="labirynth">Лабиринт.</param>
	/// <param name="path">Путь.</param>
	public void Move(Labirynth labirynth, List<Labirynth.Point> path) {
		if (path == null) {
			return;
		}
		StopAllCoroutines ();
		_labirynth = labirynth;
		_path = path;
		DrawPath (path);
	} 

	/// <summary>
	/// Отобразить работу алгоритма, после этого вызвать метод Move()
	/// </summary>
	/// <param name="labirynth">Лабиринт.</param>
	/// <param name="algo">Работа алгоритма.</param>
	/// <param name="path">Путь.</param>
	public void VisualizeAndMove(Labirynth labirynth, List<Labirynth.Point> algo, List<Labirynth.Point> path) {
		if (algo.Count <= 1) {
			Move (labirynth, path);
		} else {
			Move (null, new List<Labirynth.Point> ());
			StartCoroutine (VisualizeAndMoveCoro (labirynth, algo, path));
		}

	}

	/// <summary>
	/// Вспомогательная функция для запуска со-программы визуализации
	/// </summary>
	private IEnumerator VisualizeAndMoveCoro(Labirynth labirynth, List<Labirynth.Point> algo, List<Labirynth.Point> path) {
		float visualizeTime = 3f;
		float displayTime = 3f;

		int frames = Mathf.RoundToInt (visualizeTime / Time.fixedDeltaTime); 
		int pointPerFrame = Mathf.CeilToInt(1.0f * algo.Count / frames);

		int pos = 0;
		float time = visualizeTime + displayTime;
		while (pos < algo.Count) {
			for (int i = 0; i < pointPerFrame && pos < algo.Count; i++) {
				Color color = Color.HSVToRGB (1.0f * pos / (algo.Count - 1), 1, 1);
				labirynth.DrawSquare (algo [pos], color, time);
				pos++;
			}
			time -= Time.fixedDeltaTime;
			yield return 0;
		}

		yield return new WaitForSeconds (displayTime);

		Move (labirynth, path);
		yield return null;
	}

	void Start () {
		_line = GetComponent<LineRenderer> ();
		_path = new List<Labirynth.Point> ();
	}

	void Update () {
		if (_path.Count == 0) {
			// если текущий путь пуст, досрочно завершаем процедуру
			return;
		}

		// высчитываем текущую скорость
		float currentSpeed = GetCurrentSpeed ();

		// высчитываем пройденное за кадр расстояние
		float travelDistance = currentSpeed * Time.deltaTime;

		while (_path.Count > 0) {
			// следующая цель
			Labirynth.Point next = _path [0];
			// расстояние до следующей цели
			float distanceToNext = Vector3.Distance (transform.position, next.position);
			if (distanceToNext < travelDistance) {
				// пройденное расстояние больше чем расстояние до цели,
				// значит за этот кадр мы дошли до этоц цели - удаляем ее из очереди
				_path.RemoveAt (0);
				// сокращаем пройденный путь на расстояние до этой цели
				travelDistance -= distanceToNext;
				// перемещаемся в эту цель
				transform.position = next.position;
			} else {
				// до цели за этот кадр мы не добрались
				// сколько за этот кадр мы прошли по направлению к цели
				Vector3 moveVector = travelDistance * (next.position - transform.position).normalized;
				// перемещаемся
				transform.position += moveVector;
				// досрочно выходим из процедуры
				return;
			}
		}
	}

	/// <summary>
	/// Возвращает текущую скорость исходя из местности
	/// </summary>
	private float GetCurrentSpeed () {
		if (_labirynth != null) {
			if (_labirynth.SwampAt (transform.position)) {
				return swampSpeed;
			}
		}
		return speed;
	}
		
	/// <summary>
	/// Рисует путь с помощью LineRenderer
	/// </summary>
	private void DrawPath(List<Labirynth.Point> path) {
		if (_line != null) {
			_line.positionCount = path.Count;
			for (int i = 0; i < path.Count; i++)  {
				// Слегка приподнимем нашу линию, чтобы она не погрязла в болоте
				_line.SetPosition (i, path[i].position + Vector3.up);
			}
		} else {
			Debug.LogWarning ("Failed to draw path: no LineRendere found");
		}
	}

}

