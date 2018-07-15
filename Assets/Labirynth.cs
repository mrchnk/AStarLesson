using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Граф для поиска пути. Представляет из себя массив вершин Point 
/// на двумерной плоскости (x, z) с целочисленными координатами.
/// </summary>
public class Labirynth : MonoBehaviour {

	/// <summary>
	/// Размер графа по оси x (ширина)
	/// </summary>
	public int width = 100;

	/// <summary>
	/// Размер графа по оси z (глубина)
	/// </summary>
	public int depth = 100;

	/// <summary>
	/// Вершина графа в лабиринте.
	/// </summary>
	public class Point
	{
		/// <summary>
		/// Координата x вершины в лабиринте-графе
		/// x принимает значения от 0 до width-1
		/// </summary>
		public int x { get; }

		/// <summary>
		/// Координата z вершины в лабиринте-графе
		/// z принимает значения от 0 до depth-1
		/// </summary>
		public int z { get; }

		/// <summary>
		/// Позиция вершины в пространстве игровых объектов Unity
		/// </summary>
		public Vector3 position { get; }

		public Point(int x, int z, Vector3 position) {
			this.x = x;
			this.z = z;
			this.position = position;
		}

	}

	/// <summary>
	/// Позиция вершины (x=0, z=0) в пространстве игровых объектов Unity
	/// </summary>
	private Vector3 _zeroPosition;

	/// <summary>
	/// Массив точек графа
	/// </summary>
	private Point[,] _grid;

	/// <summary>
	/// Вспомогательный массив для поиска соседей
	/// </summary>
	private readonly Vector3Int[] _neighbours = new Vector3Int[]{
		new Vector3Int(1, 0, 0),
		new Vector3Int(-1, 0, 0),
		new Vector3Int(0, 0, 1),
		new Vector3Int(0, 0, -1)
	};

	/// <summary>
	/// Запись последних обращений к методу GetNeighbours()
	/// </summary>
	private List<Point> _recordings;

	void Start () {
		InitGrid ();
		//DrawGrid ();
	}

	/// <summary>
	/// Инициализирует массив вершин
	/// </summary>
	private void InitGrid() {
		// определяем позицию вершины (x=0, z=0)
		Vector3 gridSize = new Vector3 (width, 0, depth);
		Vector3 pointSize = new Vector3 (1f, 0, 1f);
		Vector3 halfPointSize = pointSize / 2f;
		_zeroPosition = - gridSize / 2f + halfPointSize;

		_grid = new Point [width, depth];
		for (int x = 0; x < width; x++) {
			for (int z = 0; z < depth; z++) {
				// определяем позицию точки переносом от вершины (x=0, y=0) направо и вперед
				Vector3 position = _zeroPosition + Vector3.right * x + Vector3.forward * z;
				_grid [x, z] = new Point (x, z, position);
			}
		}
	}

	/// <summary>
	/// Рисует массив вершин-клеток, раскрашивая в 3 цвета (красный,
	/// </summary>
	public void DrawGrid(float time = float.PositiveInfinity) {
		for (int x = 0; x < width; x++) {
			for (int z = 0; z < depth; z++) {
				Point p = GetPoint (x, z);
				if (ObstacleAt(p)) {
					DrawSquare (p, Color.red, time);
				} else if (SwampAt(p)) {
					DrawSquare (p, Color.cyan, time);
				} else {
					DrawSquare (p, Color.green, time);
				}
			}
		}
	}

	/// <summary>
	/// Включить запись обращений к методу GetNeighbours()
	/// </summary>
	public void Record() {
		_recordings = new List<Point> ();
	}

	/// <summary>
	/// Остановить запись обращений к методу GetNeighbours() и вернуть ее
	/// </summary>
	public List<Point> StopRecord() {
		List<Point> recordings = _recordings;
		_recordings = null;
		return recordings;
	}

	/// <summary>
	/// Является ли вершина графа непроходимой. 
	/// Непроходимыми считаются клетки в которых содержатся объекты со слоя "Obstacle"
	/// </summary>
	public bool ObstacleAt(Point point) {
		return LayerAt (point.position, "Obstacle");
	}

	public bool ObstacleAt(Vector3 position) {
		return LayerAt (position, "Obstacle");
	}

	/// <summary>
	/// Лежит ли вершина графа в "болоте".
	/// Болотом считаются клетки в которых содержатся объекты со слоя "Swamp"
	/// </summary>
	public bool SwampAt(Point point) {
		return LayerAt (point.position, "Swamp");
	}

	public bool SwampAt(Vector3 position) {
		return LayerAt (position, "Swamp");
	}
		
	private bool LayerAt(Vector3 position, string layer) {
		Vector3 collider = new Vector3 (0.5f, 0.5f, 0.5f);
		int layers = LayerMask.GetMask(new string[]{layer});
		Collider[] collisions = Physics.OverlapBox (position, collider, Quaternion.identity, layers);
		return collisions.Length > 0;
	}

	/// <summary>
	/// Возвращает ближайшую к позиции вершину графа
	/// </summary>
	public Point GetPoint(Vector3 position) {
		Vector3 fromZero = position - _zeroPosition;
		int x = Mathf.RoundToInt(fromZero.x);
		int z = Mathf.RoundToInt(fromZero.z);
		if (x < 0) {
			x = 0;
		} else if (x >= width) {
			x = width - 1;
		}
		if (z < 0) {
			z = 0;
		} else if (z >= depth) {
			z = depth - 1;
		}
		return GetPoint (x, z);
	}

	/// <summary>
	/// Возвращает вершину графа с указанными координатами
	/// </summary>
	public Point GetPoint(int x, int z) {
		return _grid[x, z];
	}

	/// <summary>
	/// Возвращает всех соседей вершины (в том числе непроходимые и болото)
	/// </summary>
	public List<Point> GetNeighbours(Point point) {
		if (_recordings != null) {
			_recordings.Add (point);
		}
		List<Point> list = new List<Point> ();
		foreach (Vector3Int neighbour in _neighbours) {
			int x = point.x + neighbour.x;
			int z = point.z + neighbour.z;
			if (x < 0 || x >= width) {
				continue;
			}
			if (z < 0 || z >= depth) {
				continue;
			}
			list.Add (GetPoint (x, z));
		}
		return list;
	}

	/// <summary>
	/// Рисует крест в вершине графа заданным цветом (используя Debug.DrawLine)
	/// </summary>
	public void DrawPoint(Point p, Color color, float duration = 1.0f) {
		DrawPoint (p.position, color, duration);
	}

	/// <summary>
	/// Рисует квадрат-клетку в вершине графа заданным цветом (используя Debug.DrawLine)
	/// </summary>
	public void DrawSquare(Point p, Color color, float duration = 1.0f) {
		DrawSquare (p.position, color, duration);
	}

	void DrawPoint(Vector3 point, Color color, float duration = 1.0f) {
		float size = 0.25f;
		Vector3 diag1 = new Vector3 (size, 0, size);
		Vector3 diag2 = new Vector3 (size, 0, -size);
		Debug.DrawLine (point - diag1, point + diag1, color, duration);
		Debug.DrawLine (point - diag2, point + diag2, color, duration);
	}


	void DrawSquare(Vector3 point, Color color, float duration = 1.0f) {
		float size = 0.45f;
		Vector3 diag1 = new Vector3 (size, 0, size);
		Vector3 diag2 = new Vector3 (size, 0, -size);
		Debug.DrawLine (point - diag1, point + diag2, color, duration);
		Debug.DrawLine (point - diag1, point - diag2, color, duration);
		Debug.DrawLine (point + diag1, point + diag2, color, duration);
		Debug.DrawLine (point + diag1, point - diag2, color, duration);
	}

}
