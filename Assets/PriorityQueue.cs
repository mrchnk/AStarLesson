using System;
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
		throw new NotImplementedException ();
	}

	/// <summary>
	/// Достать вершину из очереди с наименьшим приоритетом
	/// </summary>
	public Point Dequeue() {
		throw new NotImplementedException ();
	}

	/// <summary>
	/// 
	/// </summary>
	public bool Empty() {
		throw new NotImplementedException ();
	}

}


