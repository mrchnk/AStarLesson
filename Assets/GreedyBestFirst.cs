using System.Collections.Generic;
using UnityEngine;

public class GreedyBestFirst : PathFinder
{

	public List<Point> FindPath (Labirynth labirynth, Point source, Point destination)
	{
		// Карта "обратных" перемещений (вершина) -> (откуда мы в нее попали)
		Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point> ();
		// Очередь вершин, по которым предстоит пройти
		// Является границей текущей карты "обратных" перемещений
		PriorityQueue frontier = new PriorityQueue ();

		// Добавляем информацию о начале
		cameFrom[source] = null;
		frontier.Enqueue (source, 0f);

		while (!frontier.Empty ()) {
			// Берем первую вершину в очереди
			Point current = frontier.Dequeue ();
			if (current == destination) {
				break;
			}

			// Ищем все соседние вершины
			List<Point> neighbours = labirynth.GetNeighbours (current);
			foreach (Point neighbour in neighbours) {
				if (cameFrom.ContainsKey (neighbour)) {
					continue;
				}
				// Записываем что в `neighbour` мы попали из `current`
				float priority = Heuristic(neighbour, destination);
				cameFrom[neighbour] = current;
				frontier.Enqueue (neighbour, priority);

			}
		}

		return BuildPath (cameFrom, source, destination);
	}

	private float Heuristic(Point source, Point destination) {
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

