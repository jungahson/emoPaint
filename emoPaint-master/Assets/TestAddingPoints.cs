using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEngine.UI.Extensions
{
    public class TestAddingPoints : MonoBehaviour
    {
        public UnityEngine.UI.Extensions.UILineRenderer LineRenderer;
        
        public UnityEngine.UI.Text XValue;
        public UnityEngine.UI.Text YValue;

        [SerializeField, Tooltip("Resolution of the Bezier curve, different to line Resolution")]
        internal int bezierSegmentsPerCurve = 10;

        public List<Vector2> total_pointlist = new List<Vector2>();

        

        [SerializeField]
        private Slider arousalSlider;

        [SerializeField]
        private Slider valenceSlider;

        public int arousal;
        public int valence;

        [SerializeField, Range(10f, 30f)] 
        float angle_t = 20f;

        [SerializeField, Range(0.2f, 2f)] float threshold = 1.5f;

        [SerializeField] 
        GameObject prefab;

        public void AddNewPoint(float x_i, float y_i)
        {
            //var point = new Vector2() { x = float.Parse(XValue.text), y = float.Parse(YValue.text) };
            var point = new Vector2() { x = x_i, y = y_i };

            var pointlist = new List<Vector2>(LineRenderer.Points);
            pointlist.Add(point);
            LineRenderer.Points = pointlist.ToArray();
        }

        void Start()
        {
            arousalSlider.onValueChanged.AddListener(ArousalValueChanged);
            valenceSlider.onValueChanged.AddListener(ValenceValueChanged);


            /*AddNewPoint(1f, 1f);
            AddNewPoint(2f, 2f);
            AddNewPoint(3f, 3f);*/
            
            arousal = (int) arousalSlider.value;
            valence = (int) valenceSlider.value;

            float arousal_i = arousal / 9;
            float valence_i = valence / 9;

            int divis = arousal;

            float radius = 1f;


            float e_angle = 2 * Mathf.PI / divis;

            float scaling_factor = 10f;

            for (int j = 0; j < divis; j++)
            {
                float angle = e_angle * j;

                //float new_x = radius * Mathf.Cos(angle);
                //float new_y = radius * Mathf.Sin(angle);

                float scaling = scaling_factor * radius;

                var p1 = new Vector2() { x = scaling * Mathf.Cos(e_angle / 2), y = scaling * Mathf.Sin(e_angle / 2) };

                float diff = scaling * (Mathf.Sin(e_angle / 2) - Mathf.Sin(-e_angle / 2));

                float valence_prop = diff * valence_i;

                var p2 = new Vector2() { x = scaling * Mathf.Cos(e_angle / 2) + arousal, y = scaling * Mathf.Sin(-e_angle / 2) + valence_prop };

                var p3 = new Vector2() { x = scaling * Mathf.Cos(-e_angle / 2) + arousal, y = scaling * Mathf.Sin(e_angle / 2) - valence_prop };

                var p4 = new Vector2() { x = scaling * Mathf.Cos(-e_angle / 2), y = scaling * Mathf.Sin(-e_angle / 2) };

                var pointsToDraw = new List<Vector2>();
                pointsToDraw.Add(p1);
                pointsToDraw.Add(p2);
                pointsToDraw.Add(p3);
                pointsToDraw.Add(p4);


                BezierPath bezierPath = new BezierPath();
                bezierPath.SetControlPoints(pointsToDraw);
                bezierPath.SegmentsPerCurve = bezierSegmentsPerCurve;

                List<Vector2> drawingPoints;

                drawingPoints = bezierPath.GetDrawingPoints0();

                for (int k = 0; k < drawingPoints.Count; k++)
                {
                    drawingPoints[k] = RotateVector(drawingPoints[k], -angle);
                    total_pointlist.Add(drawingPoints[k]);
                }
                
                
            }

            LineRenderer.Points = total_pointlist.ToArray();

            /*List<Vector2> points = Utils2D.Constrain(total_pointlist, threshold);
            var polygon = Polygon2D.Contour(points.ToArray());
            var vertices = polygon.Vertices;
            
            if (vertices.Length < 3) return; // error
            
            var triangulation = new Triangulation2D(polygon, angle_t);
            
            var go = Instantiate(prefab);
            go.transform.SetParent(transform, false);  
            
            go.GetComponent<DemoMesh>().SetTriangulation(triangulation);*/
            
            LineRenderer.SetAllDirty();

        }

        public Vector2 RotateVector(Vector2 v, float angle)
        {
            float _x = v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle);
            float _y = v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle);
            return new Vector2(_x, _y);
        }

        void Update()
        {

            LineRenderer.canvasRenderer.Clear();
            //LineRenderer.Points = null;
            //LineRenderer.Segments.Clear();
            total_pointlist.Clear(); 
            //LineRenderer.Points = total_pointlist.ToArray();



            float arousal_i = arousal / 9;
            float valence_i = valence / 9;

            int divis = arousal;

            float radius = 1f;


            float e_angle = 2 * Mathf.PI / divis;

            float scaling_factor = 10f;


            for (int j = 0; j < divis; j++)
            {
                float angle = e_angle * j;

                //float new_x = radius * Mathf.Cos(angle);
                //float new_y = radius * Mathf.Sin(angle);

                float scaling = scaling_factor * radius;

                var p1 = new Vector2() { x = scaling * Mathf.Cos(e_angle / 2), y = scaling * Mathf.Sin(e_angle / 2) };

                float diff = scaling * (Mathf.Sin(e_angle / 2) - Mathf.Sin(-e_angle / 2));

                float valence_prop = diff * valence_i;

                var p2 = new Vector2() { x = scaling * Mathf.Cos(e_angle / 2) + arousal, y = scaling * Mathf.Sin(-e_angle / 2) + valence_prop };

                var p3 = new Vector2() { x = scaling * Mathf.Cos(-e_angle / 2) + arousal, y = scaling * Mathf.Sin(e_angle / 2) - valence_prop };

                var p4 = new Vector2() { x = scaling * Mathf.Cos(-e_angle / 2), y = scaling * Mathf.Sin(-e_angle / 2) };

                var pointsToDraw = new List<Vector2>();
                pointsToDraw.Add(p1);
                pointsToDraw.Add(p2);
                pointsToDraw.Add(p3);
                pointsToDraw.Add(p4);


                BezierPath bezierPath = new BezierPath();
                bezierPath.SetControlPoints(pointsToDraw);
                bezierPath.SegmentsPerCurve = bezierSegmentsPerCurve;

                List<Vector2> drawingPoints;

                drawingPoints = bezierPath.GetDrawingPoints0();

                for (int k = 0; k < drawingPoints.Count; k++)
                {
                    drawingPoints[k] = RotateVector(drawingPoints[k], -angle);
                    total_pointlist.Add(drawingPoints[k]);
                }


            }
            LineRenderer.Points = total_pointlist.ToArray();

            /*List<Vector2> points = Utils2D.Constrain(total_pointlist, threshold);
            var polygon = Polygon2D.Contour(points.ToArray());

            //var polygon = Polygon2D.Contour(LineRenderer.Points);

            var vertices = polygon.Vertices;
            if (vertices.Length < 3) return; // error

            var triangulation = new Triangulation2D(polygon, angle_t);
            var go = Instantiate(prefab);
            go.transform.SetParent(transform, false);
            go.GetComponent<DemoMesh>().SetTriangulation(triangulation);*/ 

            LineRenderer.SetAllDirty();
        }  

        public void ArousalValueChanged(float a) 
        {
            arousal = (int) a; 
        }

        public void ValenceValueChanged(float v)
        {
            valence = (int) v; 
        }
    }
}
                