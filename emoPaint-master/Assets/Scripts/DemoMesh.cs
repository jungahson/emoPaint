using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI.Extensions
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(Rigidbody))]
    public class DemoMesh : Graphic
    {

        //[SerializeField] Material lineMat;

        Triangle2D[] triangles;
        Triangle2D[] triangles_VRDraw; 

        void Update() { }

        public void SetTriangulation(Triangulation2D triangulation)
        {
            var mesh = triangulation.Build();
            GetComponent<MeshFilter>().sharedMesh = mesh;
            this.triangles = triangulation.Triangles;
        }

        public void SetTriangulationForVRDraw(Triangulation2D triangulation)
        {
            var mesh = triangulation.Build();
            GetComponent<MeshFilter>().sharedMesh = mesh;
            this.triangles_VRDraw = triangulation.Triangles;
        }

        protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
        {
            UIVertex[] vbo = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }
            return vbo;
        }

        /*protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Vector2 uv0 = new Vector2(0, 0);
            Vector2 uv1 = new Vector2(0, 1);
            Vector2 uv2 = new Vector2(1, 1);
            Vector2 uv3 = new Vector2(1, 0);

            for (int i = 0, n = triangles.Length; i < n; i++)
            {
                var t = triangles[i];

                vh.AddUIVertexQuad(SetVbo(new[] { t.a.Coordinate, t.b.Coordinate, t.c.Coordinate, t.c.Coordinate }, new[] { uv0, uv1, uv2, uv3 }));
            }
        }*/

        void OnRenderObject()
        {
            if (triangles == null) return;

            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            //lineMat.SetColor("_Color", Color.black);
            //lineMat.SetPass(0);
            GL.Begin(GL.LINES);
            for (int i = 0, n = triangles.Length; i < n; i++)
            {
                var t = triangles[i];
                GL.Vertex(t.s0.a.Coordinate); GL.Vertex(t.s0.b.Coordinate);
                GL.Vertex(t.s1.a.Coordinate); GL.Vertex(t.s1.b.Coordinate);
                GL.Vertex(t.s2.a.Coordinate); GL.Vertex(t.s2.b.Coordinate);
            }
            GL.End();
            GL.PopMatrix();
        }

    }
}