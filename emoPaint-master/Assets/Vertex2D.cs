using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex2D : MonoBehaviour
{
    public int ReferenceCount { get { return reference; } }
    public Vector2 Coordinate { get { return coordinate; } }

    Vector2 coordinate;
    int reference;

    public Vertex2D(Vector2 coord)
    {
        coordinate = coord;
    }

    public int Increment()
    {
        return ++reference;
    }

    public int Decrement()
    {
        return --reference;
    }
}