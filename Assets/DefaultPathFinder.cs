using System.Collections.Generic;

/// <summary>
/// Простейшая реализация поиска пути: путь формируется из двух вершин: начало и конец
/// </summary>
public class DefaultPathFinder : PathFinder {

	public List<Labirynth.Point> findPath (Labirynth labirynth, Labirynth.Point source, Labirynth.Point destination) {
		List<Labirynth.Point> path = new List<Labirynth.Point> ();
		path.Add (source);
		path.Add (destination);
		return path;
	}

}

