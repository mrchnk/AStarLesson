using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{

	private List<Point> points = new List<Point>();
	private List<float> priorities = new List<float>();

	public PriorityQueue () {
	}

	/// <summary>
	/// Добавить вершину в очередь с приоритетом.
	/// Если вершина уже есть в очереди, нужно задать новый приоритет
	/// </summary>
	public void Enqueue(Point point, float priority) {
		points.Add (point);
		priorities.Add (priority);
	}

	/// <summary>
	/// Достать вершину из очереди с наименьшим приоритетом
	/// </summary>
	public Point Dequeue() {
		float lowestPriority = float.PositiveInfinity;
		int lowestIndex = -1;
		for (int i = 0; i < points.Count; i++) {
			if (priorities [i] < lowestPriority) {
				lowestIndex = i;
				lowestPriority = priorities [i];
			}
		}
		if (lowestIndex == -1) {
			return null;
		}

		Point point = points [lowestIndex];
		points.RemoveAt (lowestIndex);
		priorities.RemoveAt (lowestIndex);
		return point;
	}

	public bool Empty() {
		return points.Count == 0;
	}

}


