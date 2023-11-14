using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
public class Polygon2D : MonoBehaviour
{
    public Vertex2D[] Vertices { get { return vertices.ToArray(); } }
    public Segment2D[] Segments { get { return segments.ToArray(); } }

    List<Vertex2D> vertices;
    List<Segment2D> segments;

    /*
     * slow incremental approach
     * O(n * log(n))
     */
    public static Polygon2D ConvexHull(Vector2[] points)
    {
        var ordered = points.ToList().OrderBy(p => p.x).ToList();

        var upper = new List<Vector2>();
        upper.Add(ordered[0]);
        upper.Add(ordered[1]);
        for (int i = 2, n = ordered.Count; i < n; i++)
        {
            upper.Add(ordered[i]);
            int l = upper.Count;
            if (l > 2)
            {
                var p = upper[l - 3];
                var r = upper[l - 2];
                var q = upper[l - 1];
                if (Utils2D.LeftSide(p, q, r))
                { // r is left side of pq
                    upper.RemoveAt(l - 2);
                }
            }
        }

        var lower = new List<Vector2>();
        lower.Add(ordered[ordered.Count - 1]);
        lower.Add(ordered[ordered.Count - 2]);
        for (int i = ordered.Count - 3; i >= 0; i--)
        {
            lower.Add(ordered[i]);
            int l = lower.Count;
            if (l > 2)
            {
                var p = lower[l - 3];
                var r = lower[l - 2];
                var q = lower[l - 1];
                if (Utils2D.LeftSide(p, q, r))
                { // r is left side of pq
                    lower.RemoveAt(l - 2);
                }
            }
        }

        lower.RemoveAt(lower.Count - 1);
        lower.RemoveAt(0);

        upper.AddRange(lower);

        return new Polygon2D(upper.ToArray());
    }

    public static Polygon2D Contour(Vector2[] points)
    {
        var n = points.Length;
        var edges = new List<HalfEdge2D>();
        for (int i = 0; i < n; i++)
        {
            edges.Add(new HalfEdge2D(points[i]));
        }
        for (int i = 0; i < n; i++)
        {
            var e = edges[i];
            e.from = edges[(i == 0) ? (n - 1) : (i - 1)];
            e.to = edges[(i + 1) % n];
        }
        edges = Polygon2D.SplitEdges(edges);

        var result = new List<Vector2>();

        HalfEdge2D start = edges[0];
        result.Add(start.p);

        HalfEdge2D current = start;

        while (true)
        {
            HalfEdge2D from = current, to = current.to;
            HalfEdge2D from2 = to.to, to2 = from2.to;

            bool flag = false;

            while (from2 != start && to2 != from)
            {
                if (flag = Utils2D.Intersect(from.p, to.p, from2.p, to2.p))
                {
                    break;
                }
                from2 = to2;
                to2 = to2.to;
            }

            if (!flag)
            {
                result.Add(to.p);
                current = to; // step to next
            }
            else
            {
                result.Add(from2.p);

                // reconnect
                from.to = from2;
                from2.to = from; // invert this edge later

                to.from = to2;
                to.Invert();
                to2.from = to;

                HalfEdge2D e = from2;
                while (e != to)
                {
                    e.Invert();
                    e = e.to;
                }

                current = from2;
            }

            if (current == start) break;
        }

        result.RemoveAt(result.Count - 1); // remove last

        return new Polygon2D(result.ToArray());
    }

    // Disable to intersect more than two edges
    static List<HalfEdge2D> SplitEdges(List<HalfEdge2D> edges)
    {
        HalfEdge2D start = edges[0];
        HalfEdge2D cur = start;

        while (true)
        {
            HalfEdge2D from = cur, to = from.to;
            HalfEdge2D from2 = to.to, to2 = from2.to;

            int intersections = 0;

            while (to2 != from.from)
            {
                if (Utils2D.Intersect(from.p, to.p, from2.p, to2.p))
                {
                    intersections++;
                    if (intersections >= 2)
                    {
                        break;
                    }
                }
                // next
                from2 = from2.to;
                to2 = to2.to;
            }

            if (intersections >= 2)
            {
                edges.Add(cur.Split());
            }
            else
            {
                // next
                cur = cur.to;
                if (cur == start) break;
            }
        }

        return edges;
    }

    // contour must be inversed clockwise order.
    public Polygon2D(Vector2[] contour)
    {
        vertices = contour.Select(p => new Vertex2D(p)).ToList();
        segments = new List<Segment2D>();
        for (int i = 0, n = vertices.Count; i < n; i++)
        {
            var v0 = vertices[i];
            var v1 = vertices[(i + 1) % n];
            segments.Add(new Segment2D(v0, v1));
        }
    }

    public bool Contains(Vector2 p)
    {
        return Utils2D.Contains(p, vertices);
    }

    public void DrawGizmos()
    {
        segments.ForEach(s => s.DrawGizmos());
    }

    public class Utils2D
    {

        // constrain a distance between two points to "threshold" length
        public static List<Vector2> Constrain(List<Vector2> points, float threshold = 1f)
        {
            var result = new List<Vector2>();

            var n = points.Count;
            for (int i = 0, j = 1; i < n && j < n; j++)
            {
                var from = points[i];
                var to = points[j];
                if (Vector2.Distance(from, to) > threshold)
                {
                    result.Add(from);
                    i = j;
                }
            }

            var p0 = result.Last();
            var p1 = result.First();
            if (Vector2.Distance(p0, p1) > threshold)
            {
                result.Add((p0 + p1) * 0.5f);
            }

            return result;
        }

        // http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
        // check intersection segment (p0, p1) to segment (p2, p3)
        public static bool Intersect(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var s1 = p1 - p0;
            var s2 = p3 - p2;
            var s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / (-s2.x * s1.y + s1.x * s2.y);
            var t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / (-s2.x * s1.y + s1.x * s2.y);
            return (s >= 0 && s <= 1 && t >= 0 && t <= 1);
        }

        // http://stackoverflow.com/questions/217578/how-can-i-determine-whether-a-2d-point-is-within-a-polygon
        public static bool Contains(Vector2 p, List<Vertex2D> vertices)
        {
            var n = vertices.Count;
            bool c = false;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (vertices[i].Coordinate == p) return true;
                if (
                    ((vertices[i].Coordinate.y > p.y) != (vertices[j].Coordinate.y > p.y)) &&
                    (p.x < (vertices[j].Coordinate.x - vertices[i].Coordinate.x) * (p.y - vertices[i].Coordinate.y) / (vertices[j].Coordinate.y - vertices[i].Coordinate.y) + vertices[i].Coordinate.x)
                )
                {
                    c = !c;
                }
            }
            // c == true means odd, c == false means even
            return c;
        }

        // check p is left side of segment (from, to)
        public static bool LeftSide(Vector2 from, Vector2 to, Vector2 p)
        {
            float cross = ((to.x - from.x) * (p.y - from.y) - (to.y - from.y) * (p.x - from.x));
            return (cross > 0f);
        }

        public static bool CheckEqual(Vertex2D v0, Vertex2D v1)
        {
            return (v0.Coordinate == v1.Coordinate);
        }

        public static bool CheckEqual(Segment2D s0, Segment2D s1)
        {
            return
                (CheckEqual(s0.a, s1.a) && CheckEqual(s0.b, s1.b)) ||
                (CheckEqual(s0.a, s1.b) && CheckEqual(s0.b, s1.a));
        }

        public static bool CheckEqual(Triangle2D t0, Triangle2D t1)
        {
            // 0,1,2 == 0,1,2
            // 0,1,2 == 0,2,1
            // 0,1,2 == 1,0,2
            // 0,1,2 == 1,2,0
            // 0,1,2 == 2,0,1
            // 0,1,2 == 2,1,0
            return
                (CheckEqual(t0.s0, t1.s0) && CheckEqual(t0.s1, t1.s1) && CheckEqual(t0.s2, t1.s2)) ||
                (CheckEqual(t0.s0, t1.s0) && CheckEqual(t0.s1, t1.s2) && CheckEqual(t0.s2, t1.s1)) ||

                (CheckEqual(t0.s0, t1.s1) && CheckEqual(t0.s1, t1.s0) && CheckEqual(t0.s2, t1.s2)) ||
                (CheckEqual(t0.s0, t1.s1) && CheckEqual(t0.s1, t1.s2) && CheckEqual(t0.s2, t1.s0)) ||

                (CheckEqual(t0.s0, t1.s2) && CheckEqual(t0.s1, t1.s0) && CheckEqual(t0.s2, t1.s1)) ||
                (CheckEqual(t0.s0, t1.s2) && CheckEqual(t0.s1, t1.s1) && CheckEqual(t0.s2, t1.s0));
        }

    }

    public class HalfEdge2D
    {
        public Vector2 p;
        public HalfEdge2D from, to;
        public HalfEdge2D(Vector2 p)
        {
            this.p = p;
        }
        public void Invert()
        {
            var tmp = from; from = to; to = tmp;
        }
        public HalfEdge2D Split()
        {
            var m = (to.p + p) * 0.5f;
            var e = new HalfEdge2D(m);
            to.from = e; e.to = to;
            this.to = e; e.from = this;
            return e;
        }
    }

    public class Circle2D
    {
        public Vector2 center;
        public float radius;

        public Circle2D(Vector2 c, float r)
        {
            this.center = c;
            this.radius = r;
        }

        public bool Contains(Vector2 p)
        {
            return (p - center).magnitude < radius;
        }

        public static Circle2D GetCircumscribedCircle(Triangle2D triangle)
        {
            var x1 = triangle.a.Coordinate.x;
            var y1 = triangle.a.Coordinate.y;
            var x2 = triangle.b.Coordinate.x;
            var y2 = triangle.b.Coordinate.y;
            var x3 = triangle.c.Coordinate.x;
            var y3 = triangle.c.Coordinate.y;

            float x1_2 = x1 * x1;
            float x2_2 = x2 * x2;
            float x3_2 = x3 * x3;
            float y1_2 = y1 * y1;
            float y2_2 = y2 * y2;
            float y3_2 = y3 * y3;

            // 外接円の中心座標を計算
            float c = 2f * ((x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1));
            float x = ((y3 - y1) * (x2_2 - x1_2 + y2_2 - y1_2) + (y1 - y2) * (x3_2 - x1_2 + y3_2 - y1_2)) / c;
            float y = ((x1 - x3) * (x2_2 - x1_2 + y2_2 - y1_2) + (x2 - x1) * (x3_2 - x1_2 + y3_2 - y1_2)) / c;
            float _x = (x1 - x);
            float _y = (y1 - y);

            float r = Mathf.Sqrt((_x * _x) + (_y * _y));
            return new Circle2D(new Vector2(x, y), r);
        }

        public void DrawGizmos(float step = 0.02f)
        {

            var points = new List<Vector2>();
            for (float t = 0f; t <= 1f; t += step)
            {
                var r = t * Mathf.PI * 2f;
                float x = Mathf.Cos(r) * radius;
                float y = Mathf.Sin(r) * radius;
                points.Add(center + new Vector2(x, y));
            }

            for (int i = 0, n = points.Count; i < n; i++)
            {
                var p0 = points[i];
                var p1 = points[(i + 1) % n];
                Gizmos.DrawLine(p0, p1);
            }

        }

    }
}


