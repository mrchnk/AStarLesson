using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Алгоритм A*
/// Предстоит реализовать самому
/// </summary>
public class AStar: PathFinder {

	public List<Point> FindPath (Labirynth labirynth, Point source, Point destination)
	{
		// Карта "обратных" перемещений (вершина) -> (откуда мы в нее попали)
		Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point> ();
		Dictionary<Point, float> costSoFar = new Dictionary<Point, float> ();
		// Очередь вершин, по которым предстоит пройти
		// Является границей текущей карты "обратных" перемещений
		PriorityQueue frontier = new PriorityQueue ();

		// Добавляем информацию о начале
		cameFrom[source] = null;
		costSoFar[source] = 0f;
		frontier.Enqueue (source, 0f);

		while (!frontier.Empty ()) {
			// Берем первую вершину в очереди
			Point current = frontier.Dequeue ();
			if (current == destination) {
				break;
			}
			float currentCost = costSoFar [current];

			// Ищем все соседние вершины
			List<Point> neighbours = labirynth.GetNeighbours (current);
			foreach (Point neighbour in neighbours) {
				float walkingCost = labirynth.GetCost (current, neighbour);
				float neigbourCost = currentCost + walkingCost;
				// Мы смогли в нее прийти другим более дешевым способом
				if (cameFrom.ContainsKey (neighbour) && costSoFar[neighbour] <= neigbourCost) {
					continue;
				}
				// Записываем что в `neighbour` мы попали из `current` с новой стоимостью
				float priority = neigbourCost + Heuristic(neighbour, destination);
				cameFrom[neighbour] = current;
				costSoFar[neighbour] = neigbourCost;
				frontier.Enqueue (neighbour, priority);

			}
		}

		return BuildPath (cameFrom, source, destination);
	}

	private int Heuristic(Point source, Point destination) {
		return Mathf.Abs (source.x - destination.x) + Mathf.Abs (source.z - destination.z);
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

}
