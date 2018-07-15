using System.Collections.Generic;

/// <summary>
/// Алгоритм поиска в глубину
/// </summary>
public class DepthFirstSearch : PathFinder {

	public List<Point> FindPath (Labirynth labirynth, Point source, Point destination) {
		HashSet<Point> visited = new HashSet<Point> ();
		List<Point> path = new List<Point> ();
		path.Add (source);
		visited.Add (source);
		FindPath (labirynth, destination, path, visited);
		return path;
	}

	/// <summary>
	/// Вспомогательная функция с другой сигнатурой
	/// </summary>
	/// <returns><c>true</c>, если путь был найден, <c>false</c> иначе.</returns>
	/// <param name="labirynth">граф-лабиринт</param>
	/// <param name="destination">цель</param>
	/// <param name="path">текущий путь</param>
	/// <param name="visited">вершины которые мы обошли алгоритмом</param>
	private bool FindPath(Labirynth labirynth, Point destination, 
		List<Point> path, HashSet<Point> visited) {

		// последний компонент пути
		Point last = path [path.Count - 1];
		if (last == destination) { // путь заканчивается на цели - значит это искомый путь
			return true;
		}

		// текущие соседи
		List<Point> neighbours = labirynth.GetNeighbours(last);
		foreach (Point neighbour in neighbours) {
			if (visited.Contains (neighbour)) {
				// эту вершину мы уже посетили
				continue;
			} else if (labirynth.ObstacleAt( neighbour )) {
				// через эту вершину пройти нельзя
				continue;
			}
			path.Add (neighbour); // дополняем путь одной вершиной
			visited.Add (neighbour); // отмечаем вершину посещенной
			// проводим поиск с использованием этой вершины
			if (FindPath (labirynth, destination, path, visited)) {
				// если поиск с этой вершиной привел к положительному результату
				// досрочно заканчиваем выполнение функции с положительным результатом
				return true;
			}
			// иначе убираем этого соседа из текущего пути
			path.Remove (neighbour);
		}
		return false;
	}

}
