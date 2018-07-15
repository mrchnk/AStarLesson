using System.Collections.Generic;

/// <summary>
/// Интерфейс для алгоритма поиска пути
/// </summary>
public interface PathFinder
{

	/// <summary>
	/// Ищет путь в графе-лабиринте
	/// </summary>
	/// <returns>Путь по вершинам в графе</returns>
	/// <param name="labirynth">Лабиринт-граф</param>
	/// <param name="source">Вершина начала пути</param>
	/// <param name="destination">Вершина конца пути</param>
	List<Point> FindPath (Labirynth labirynth, Point source, Point destination);

}

