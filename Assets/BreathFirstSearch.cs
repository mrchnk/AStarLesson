using System.Collections.Generic;

/// <summary>
/// Поиск в ширину
/// </summary>
public class BreathFirstSearch: PathFinder {

	public List<Point> FindPath (Labirynth labirynth, Point source, Point destination)
	{
		// карта перемещений (вершина -> откуда в нее попали)
		// по совместительству является множеством вершин которые мы обошли
		Dictionary<Point, Point> from = new Dictionary<Point, Point> ();
		// граница множества вершин которые мы обошли
		HashSet<Point> fringe = new HashSet<Point> ();
		// начало содержится в карте и ссылается на null
		from.Add (source, null);
		// кольцо начинается с начальной вершины
		fringe.Add (source);

		if (FindPath (labirynth, destination, from, fringe)) {
			// если путь найден, то сосставляем его используя нашу карту перемещений
			return BuildPath (from, source, destination);		
		} else {
			// иначе возвращаем пустой путь
			return new List<Point> ();
		}
	}

	private bool FindPath(Labirynth labirynth, Point destination, 
		Dictionary<Point, Point> from,
		HashSet<Point> fringe) {

		if (fringe.Contains (destination)) {
			// цель попала на границу
			// заканчиваем поиск
			return true;
		} else if (fringe.Count == 0) {
			// граница множества пуста
			return false;
		}

		// расширяем множество вершин до соседних
		// составляем новую границу множества
		HashSet<Point> nextFringe = new HashSet<Point> ();
		foreach (Point f in fringe) {
			List<Point> neighbours = labirynth.GetNeighbours (f);
			foreach (Point neighbour in neighbours) {
				if (from.ContainsKey (neighbour)) {
					continue;
				}
				if (labirynth.ObstacleAt (neighbour)) {
					continue;
				}
				from.Add (neighbour, f);
				nextFringe.Add (neighbour);
			}
		}
		// проводим следующую итерацию алгоритма
		return FindPath (labirynth, destination, from, nextFringe);
	}

	private List<Point> BuildPath(
		Dictionary<Point, Point> from, 
		Point source,
		Point destination) {

		List<Point> path = new List<Point> ();

		// составляем обратный путь из цели в начало
		Point p = destination;
		path.Add (p);
		while (p != source) {
			p = from [p];
			path.Add (p);
		}
		// обращаем путь
		path.Reverse ();
		return path;
	}
		

}
