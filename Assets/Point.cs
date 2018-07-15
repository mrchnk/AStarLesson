using UnityEngine;

/// <summary>
/// Вершина графа в лабиринте.
/// </summary>
public class Point {
	
	/// <summary>
	/// Координата x вершины в лабиринте-графе
	/// x принимает значения от 0 до width-1
	/// </summary>
	public int x { get; }

	/// <summary>
	/// Координата z вершины в лабиринте-графе
	/// z принимает значения от 0 до depth-1
	/// </summary>
	public int z { get; }

	/// <summary>
	/// Позиция вершины в пространстве игровых объектов Unity
	/// </summary>
	public Vector3 position { get; }

	public Point(int x, int z, Vector3 position) {
		this.x = x;
		this.z = z;
		this.position = position;
	}

}