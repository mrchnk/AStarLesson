using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Связующий компонент.
/// При клике мышкой (или нажатии на мобильном устройстве) на пол лабиринта (объекты со слоя "Floor")
/// Ищет путь до указанной точки с помощью выбранного алгоритма pathFinder и передает компоненту PathMover для
/// прохождения по нему
/// </summary>
public class LabirynthActor : MonoBehaviour
{
	/// <summary>
	/// Тип алгоритма поиска пути
	/// </summary>
	public enum PathFinderEnum
	{
		Default,
		DepthFirstSearch,
		BreadthFirstSearch,
		Dijkstra,
		AStar,
		Unity
	}

	/// <summary>
	/// Лабиринт-граф, в котором будет произведен поиск
	/// </summary>
	public Labirynth labirynth;

	/// <summary>
	/// Текущий тип алгоритма поиска пути
	/// </summary>
	public PathFinderEnum pathFinder = PathFinderEnum.Default;

	/// <summary>
	/// Компонент для перемещение по найденному алгоритму
	/// </summary>
	private PathMover _mover;

	public void Start() {
		_mover = GetComponent<PathMover> ();
	}

	public void Update() {
		Ray ray;
		bool visualize = false;
		if (Input.GetMouseButtonDown (0)) {
			// была нажата левая кнопка мыши
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		} else if (Input.GetMouseButtonDown(1)) {
			// была нажата правая кнопка мыши
			visualize = true; // также отобразим как алгоритм работал
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		} else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			// произошло касание по экрану (телефона)
			ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
		} else {
			// досрочно завершаем процедуру во всех остальных случаях
			return;
		}

		// Пытаемся найти точку на слое "Floor" на который пришлось нажатие
		int floor = LayerMask.GetMask (new string[]{ "Floor" });
		RaycastHit hit = new RaycastHit ();
		if (!Physics.Raycast (ray, out hit, float.PositiveInfinity, floor)) {
			// Если нажали не на пол досрочно завершаем процедуру
			return;
		}

		// Инициализируем вершину начала пути = текущему положения объекта
		Point source = labirynth.GetPoint (transform.position);
		// Инициализируем вершину конца пути = точка нажатия
		Point destination = labirynth.GetPoint (hit.point);
		PathFinder finder = GetPathFinder (pathFinder);

		if (visualize) {
			// Поиск пути и визуализация алгоритма поиска
			labirynth.Record ();
			List<Point> path = finder.FindPath (labirynth, source, destination);
			List<Point> algo = labirynth.StopRecord ();
			_mover.VisualizeAndMove (labirynth, algo, path);
		} else {
			// Поиск пути и перемещение
			List<Point> path = finder.FindPath (labirynth, source, destination);
			_mover.Move (labirynth, path);
		}
	}

	/// <summary>
	/// Возвращает имплементацию алгоритма поиска исходя из типа
	/// </summary>
	/// <returns>The path finder.</returns>
	public PathFinder GetPathFinder(PathFinderEnum pathFinder) {
		switch (pathFinder) {
		case PathFinderEnum.DepthFirstSearch:
			return new DepthFirstSearch ();
		case PathFinderEnum.BreadthFirstSearch:
			return new BreadthFirstSearch ();
		case PathFinderEnum.Dijkstra:
			return new Dijkstra ();
		case PathFinderEnum.AStar:
			return new AStar ();
		case PathFinderEnum.Unity:
			return new UnityPathFinder ();
		default:
			return new DefaultPathFinder ();
		}
	}

}

