using System.Collections.Generic;

/// <summary>
/// Алгоритм поиска в глубину (для прикола)
/// </summary>
public class DepthFirstSearch : PathFinder {

	/// <summary>
	/// Ищет путь в графе-лабиринте
	/// </summary>
	/// <returns>Путь по вершинам в графе</returns>
	/// <param name="labirynth">Лабиринт-граф</param>
	/// <param name="source">Вершина начала пути</param>
	/// <param name="destination">Вершина конца пути</param>
	public List<Point> FindPath (Labirynth labirynth, Point source, Point destination) {
		// Карта "обратных" перемещений (вершина) -> (откуда мы в нее попали)
		Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point> ();
		// Стек вершин, по которым предстоит пройти
		// Является границей текущей карты "обратных" перемещений
		Stack<Point> frontier = new Stack<Point> ();

		// Добавляем информацию о начале
		cameFrom.Add (source, null);
		frontier.Push (source);

		while (frontier.Count > 0) {
			// Берем первую вершину в стеке
			Point current = frontier.Pop ();
			if (current == destination) {
				break;
			}
			// Ищем все соседние вершины
			List<Point> neighbours = labirynth.GetNeighbours (current);

			// Перемешаем соседей для драмматизма
			Shuffle (neighbours);

			foreach (Point neighbour in neighbours) {
				// Эту вершину мы включили в нашу карту перемещений,
				// значит мы смогли в нее прийти другим способом
				if (cameFrom.ContainsKey (neighbour)) {
					continue;
				}
				// Записываем что в `neighbour` мы попали из `current`
				cameFrom.Add (neighbour, current);
				frontier.Push (neighbour);
			}
		}

		return BuildPath (cameFrom, source, destination);
	}

	/// <summary>
	/// Строит путь используя карту "обратных" перемещений
	/// </summary>
	/// <returns>Путь из `source` в `destination` или пустой список, если путь не найден</returns>
	private List<Point> BuildPath(Dictionary<Point, Point> cameFrom, Point source, Point destination) {
		List<Point> path = new List<Point> ();
		if (!cameFrom.ContainsKey (destination)) {
			// Путь не найден, если цели нет на карте обратных перемещений
			return path;
		}
		// Строим обратный путь
		Point current = destination;
		while (current != source) {
			path.Add (current);
			current = cameFrom [current];
		}
		// Не забываем добавить начало
		path.Add (source);
		// "Переворачиваем" путь
		path.Reverse ();
		return path;
	}

	/// <summary>
	/// Перемешать список вершин
	/// </summary>
	private void Shuffle(List<Point> list) {
		int count = list.Count;
		for (var i = 0; i < count - 1; i++) {
			var r = UnityEngine.Random.Range(i, count);
			Point tmp = list[i];
			list[i] = list[r];
			list[r] = tmp;
		}
	}

}
