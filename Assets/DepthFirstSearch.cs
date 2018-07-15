using System.Collections.Generic;

/// <summary>
/// Алгоритм поиска в глубину
/// </summary>
public class DepthFirstSearch : PathFinder {

	public List<Labirynth.Point> findPath (Labirynth labirynth, Labirynth.Point source, Labirynth.Point destination) {
		HashSet<Labirynth.Point> visited = new HashSet<Labirynth.Point> ();
		List<Labirynth.Point> path = new List<Labirynth.Point> ();
		path.Add (source);
		visited.Add (source);
		findPath (labirynth, destination, path, visited);
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
	private bool findPath(Labirynth labirynth, Labirynth.Point destination, 
		List<Labirynth.Point> path, HashSet<Labirynth.Point> visited) {

		// последний компонент пути
		Labirynth.Point last = path [path.Count - 1];
		if (last == destination) { // путь заканчивается на цели - значит это искомый путь
			return true;
		}

		// текущие соседи
		List<Labirynth.Point> neighbours = labirynth.GetNeighbours(last);
		foreach (Labirynth.Point neighbour in neighbours) {
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
			if (findPath (labirynth, destination, path, visited)) {
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
