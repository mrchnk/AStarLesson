using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Алгоритм поиска пути в графе с помошью встроенного в движок функционала.
/// Расчитывает на то, что навигационный меш (NavMesh) правильно настроен в редакторе.
/// </summary>
public class UnityPathFinder: PathFinder {

	public UnityPathFinder() {
	}

	public List<Point> FindPath (Labirynth labirynth, Point source, Point destination)
	{
		NavMeshPath navMeshPath = new NavMeshPath ();
		NavMesh.CalculatePath (source.position, destination.position, NavMesh.AllAreas, navMeshPath);
		List<Point> path = new List<Point>();
		foreach (Vector3 corner in navMeshPath.corners) {
			path.Add(labirynth.GetPoint(corner));
		}
		return path;
	}

}
