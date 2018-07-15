using System.Collections.Generic;

/// <summary>
/// Простейшая реализация поиска пути: путь формируется из двух вершин: начало и конец
/// </summary>
public class DefaultPathFinder : PathFinder {

	public List<Point> FindPath (Labirynth labirynth, Point source, Point destination) {
		List<Point> path = new List<Point> ();
		path.Add (source);
		path.Add (destination);
		return path;
	}

}

