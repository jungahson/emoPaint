using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using System.Linq; 
using DilmerGames.Enums;
using DilmerGames.Core.Utilities;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.IO;

namespace DilmerGames
{
    public class VRDraw : MonoBehaviour
    {
        [SerializeField]
        private XRNode xrNode = XRNode.RightHand;

        private List<InputDevice> devices = new List<InputDevice>();

        private InputDevice device;

        [SerializeField]
        private ControlHand controlHand = ControlHand.NoSet;

        [SerializeField]
        private GameObject objectToTrackMovement;

        [SerializeField]
        private GameObject leftTrackMovement;

        [SerializeField]
        private GameObject TrackHeadMovement;

        private float lineSize = 0.01f;

        private Vector3 prevPointDistance = Vector3.zero;

        [SerializeField, Range(0, 1.0f)]
        private float minDistanceBeforeNewPoint = 0.2f;

        [SerializeField, Range(0, 1.0f)]
        private float minDrawingPressure = 0.8f;

        private Vector3 s = Vector3.zero;
        //private Vector3 s;

        [SerializeField, Range(0, 1.0f)]
        //private float lineDefaultWidth = 0.010f;
        private float lineDefaultWidth = 0.050f;
        private float lineDefaultWidth_ep = 0.050f;

        private int positionCount = 0; // 2 by default
        private int positionCount2 = 0; // 2 by default
        private int positionCount3 = 0; // 2 by default
        private int pC = 0;

        private List<LineRenderer> lines = new List<LineRenderer>();
        private List<LineRenderer> shapelines = new List<LineRenderer>();
        private List<Vector3> linePos = new List<Vector3>();

        private List<Vector3> middlePos = new List<Vector3>();

        private List<int> colorIdxes = new List<int>();
        private List<int> IdxofCIdx = new List<int>();

        // Shape
        private List<int> shapeColorIdxes = new List<int>();
        private List<int> shapeIdxofCIdx = new List<int>();

        private Vector3[,] savedPos = new Vector3[50, 300];

        //private LineRenderer shapeLines; 
        private LineRenderer currentLineRender;
        private LineRenderer currentLineRender_ep;
        private LineRenderer comp;

        // Mesh Renderer (for lines) 
        private int lineCount = 0;

        [SerializeField]
        private Material defaultLineMaterial;

        private int defaultTex = 0;
        public int selectedEmo = 0;  

        [SerializeField]
        private Color defaultColor = Color.white;
        [SerializeField]
        private Color defaultColor_ep = Color.white;

        public GameObject Panel;
        public GameObject Panel2; 
        public Dropdown emoDd;
        public Dropdown emoDd2;

        public GameObject lineObj;
        public GameObject shapelineObj;
        public GameObject shapeObj;
        public GameObject temp;

        private bool IsPinchingReleased = false;
        private bool IsThumbsUp2 = false;

        public ColorPickerControl picker;
        
        [SerializeField]
        private GameObject editorObjectToTrackMovement;

        [SerializeField]
        private bool allowEditorControls = true;

        [SerializeField]
        private VRControllerOptions vrControllerOptions;

        public VRControllerOptions VRControllerOptions => vrControllerOptions;

        private Vector3[] saved_quad;
        private bool triggerIsPressed;
        private bool pause = false;

        private bool temp_triggerButton = false;

        //[SerializeField]
        //private Material[] linemat;
        private Material linematerial;

        public Texture m_MainTexture;
        // Texture 
        public Texture[] textures;
        public LineTextureMode textureMode = LineTextureMode.RepeatPerSegment;

        public Slider arousal0;
        public Slider valence0;

        public Slider arousal1;
        public Slider valence1;

        public Slider arousal2;
        public Slider valence2;

        public Slider arousal3;
        public Slider valence3;

        public Slider arousal4;
        public Slider valence4;

        public Slider arousal5;
        public Slider valence5;

        public Slider arousal6;
        public Slider valence6;

        //Song beats per minute
        //This is determined by the song you're trying to sync up to
        public float songBpm;

        //The number of seconds for each song beat
        public float secPerBeat;

        //Current song position, in seconds
        public float songPosition;
        public float lastPosition;

        //Current song position, in beats
        public float songPositionInBeats;

        //How many seconds have passed since the song started
        public float dspSongTime;

        //an AudioSource attached to this GameObject that will play the music.
        public AudioSource musicSource;

        private bool startPaint_flag = false; 
        private bool musicPlay_flag = false;
        private bool device_flag = false;
        private bool saveData_flag = true;
        private bool saveSecond_flag = true; 
        
        private float[] arousal;
        private float[] valence;

        private int modalityIdx = 3;
        //private int emotionIdx = 0;
        private int arousal_l;
        private int valence_l;

        private int[] arousal_arr;
        private int[] valence_arr;

        private Transform s_trans; 
        private Vector3 s_pos;
        private int shapeCount = 0;

        private int colorIdx;
        private int shapeColorIdx;

        private string[][] colorpalette;
        private Color myRgbColor;

        private List<Vector2> total_pointlist_VRDraw = new List<Vector2>();
        private List<Vector2> total_pointlist2_VRDraw = new List<Vector2>();

        private List<c_LinePos> LineList;

        [SerializeField]
        GameObject prefab;

        bool LineFlag = true;

        int iter = 0;

        string filePath;
        StreamWriter writer;
        StreamWriter writer2; 

        string format = "0.####";

        void GetDevice()
        {
            InputDevices.GetDevicesAtXRNode(xrNode, devices);
            device = devices.FirstOrDefault();
        }

        void OnEnable()
        {
            if (!device.isValid)
            {
                GetDevice();
            }
        }

        public class c_LinePos
        {
            public List<Vector3> points = new List<Vector3>();
            public void Load(Vector3 data)
            {
                points.Add(data);
            }
        }
        
        void Start()
        {
            //linematerial = linemat[0];
            AddNewLineRenderer();
            AddNewShapeRenderer();
                
            string m_Path;
            // path to the csv file
            m_Path = Application.dataPath;
            
            TextAsset arousaldata = Resources.Load<TextAsset>("arousal");
            string[] row = arousaldata.text.Split(new char[] { '\n' });
            string[][] array;
            array = new string[row.Length][];

            for (int i = 0; i < row.Length; i++)
            {
                array[i] = row[i].Split(new char[] { ',' });
            }

            arousal = new float[array[340].Length];

            for (int i = 0; i < array[340].Length; i++)
            {
                arousal[i] = float.Parse(array[340][i]);
            }

            TextAsset valencedata = Resources.Load<TextAsset>("valence");
            VRStats.Instance.thirdText.text = $"Song Position: {valencedata}";
            string[] row2 = valencedata.text.Split(new char[] { '\n' });
            string[][] array2;
            array2 = new string[row2.Length][];

            for (int i = 0; i < row2.Length; i++)
            {
                array2[i] = row2[i].Split(new char[] { ',' });
            }

            Debug.Log(array2[340].Length);
            valence = new float[array2[340].Length];
            for (int i = 0; i < array2[340].Length; i++)
            {
                valence[i] = float.Parse(array2[340][i]);
            }

            //Load the AudioSource attached to the Conductor GameObject
            musicSource = GetComponent<AudioSource>();
            lastPosition = 0.0f;

            //UpdateShape();    

            arousal_l = 6;
            valence_l = 9;

            //textures = new Texture[18]; 
            arousal_arr = new int[7];
            valence_arr = new int[7];

            arousal_arr[0] = (int) GameObject.Find("Shape1/Slider").GetComponent<Slider>().value;
            valence_arr[0] = (int) GameObject.Find("Shape1/Slider2").GetComponent<Slider>().value; 

            arousal_arr[1] = (int) GameObject.Find("Shape2/Slider").GetComponent<Slider>().value; 
            valence_arr[1] = (int) GameObject.Find("Shape2/Slider2").GetComponent<Slider>().value; 

            arousal_arr[2] = (int) GameObject.Find("Shape3/Slider").GetComponent<Slider>().value; 
            valence_arr[2] = (int) GameObject.Find("Shape3/Slider2").GetComponent<Slider>().value; 

            arousal_arr[3] = (int) GameObject.Find("Shape4/Slider").GetComponent<Slider>().value; 
            valence_arr[3] = (int) GameObject.Find("Shape4/Slider2").GetComponent<Slider>().value; 

            arousal_arr[4] = (int) GameObject.Find("Shape5/Slider").GetComponent<Slider>().value; 
            valence_arr[4] = (int) GameObject.Find("Shape5/Slider2").GetComponent<Slider>().value; 

            arousal_arr[5] = (int) GameObject.Find("Shape6/Slider").GetComponent<Slider>().value; 
            valence_arr[5] = (int) GameObject.Find("Shape6/Slider2").GetComponent<Slider>().value; 

            arousal_arr[6] = (int) GameObject.Find("Shape7/Slider").GetComponent<Slider>().value; 
            valence_arr[6] = (int) GameObject.Find("Shape7/Slider2").GetComponent<Slider>().value; 

            TextAsset col_pal = Resources.Load<TextAsset>("myrgb3");

            string[] rowpal = col_pal.text.Split(new char[] { '\n' });
            string[][] pal_array;
            pal_array = new string[rowpal.Length][];

            for (int i = 0; i < rowpal.Length; i++)
            {
                pal_array[i] = rowpal[i].Split(new char[] { ' ' });
            }
            
            colorpalette = new string[rowpal.Length][];

            for (int i = 0; i < rowpal.Length; i++)
            {
                colorpalette[i] = rowpal[i].Split(' ');
            }
            
            LineList = new List<c_LinePos>();

            filePath = getPath();
            writer = new StreamWriter(filePath);
            filePath = getPath2();
            writer2 = new StreamWriter(filePath);

            //This is writing the line of the type, name, damage... etc... (I set these)
            //writer.WriteLine("Headpos_x,Headpos_y,Headpos_z,Headrot_x,Headrot_y,Headrot_z,Leftpos_x,Leftpos_y,Leftpos_z,Rightpos_x,Rightpos_y,Rightpos_z");

            writer.WriteLine("Pressed, emotion, texture, Color_r, Color_g, Color_b, Pos_x, Pos_y, Pos_z, ls");
            writer2.WriteLine("Pressed, emotion, texture, Color_r, Color_g, Color_b, Pos_x, Pos_y, Pos_z, ls");

            arousal0.onValueChanged.AddListener(delegate { ArousalChanged_0(); });
            arousal1.onValueChanged.AddListener(delegate { ArousalChanged_1(); });
            arousal2.onValueChanged.AddListener(delegate { ArousalChanged_2(); });
            arousal3.onValueChanged.AddListener(delegate { ArousalChanged_3(); });
            arousal4.onValueChanged.AddListener(delegate { ArousalChanged_4(); });
            arousal5.onValueChanged.AddListener(delegate { ArousalChanged_5(); });
            arousal6.onValueChanged.AddListener(delegate { ArousalChanged_6(); });

            valence0.onValueChanged.AddListener(delegate { ValenceChanged_0(); });
            valence1.onValueChanged.AddListener(delegate { ValenceChanged_1(); });
            valence2.onValueChanged.AddListener(delegate { ValenceChanged_2(); });
            valence3.onValueChanged.AddListener(delegate { ValenceChanged_3(); });
            valence4.onValueChanged.AddListener(delegate { ValenceChanged_4(); });
            valence5.onValueChanged.AddListener(delegate { ValenceChanged_5(); });
            valence6.onValueChanged.AddListener(delegate { ValenceChanged_6(); });
        }

        private string getPath()
        {
            #if UNITY_EDITOR
            return Application.dataPath + "/CSVfiles/" + "Saved_Inventory.csv";
            #elif UNITY_ANDROID
            return Application.persistentDataPath+"Saved_Inventory.csv";
            #elif UNITY_IPHONE
            return Application.persistentDataPath+"/"+"Saved_Inventory.csv";
            #else
            return Application.dataPath +"/"+"Saved_Inventory.csv";
            #endif
        }

        private string getPath2()
        {
            #if UNITY_EDITOR
            return Application.dataPath + "/CSVfiles/" + "Saved_Second.csv";
            #elif UNITY_ANDROID
            return Application.persistentDataPath+"Saved_Second.csv";
            #elif UNITY_IPHONE
            return Application.persistentDataPath+"/"+"Saved_Second.csv";
            #else
            return Application.dataPath +"/"+"Saved_Second.csv";
            #endif
        }

        public void ClosePanel()
        {
            startPaint_flag = true; 
            // Write emotion 
            emoDd = GameObject.Find("Panel/Dropdown").GetComponent<Dropdown>();
            int index = emoDd.value;
            string text = emoDd.options[index].text;

            writer.WriteLine(text);  

            if (Panel != null)
            {
                Panel.SetActive(false); 
            }
        }

        public void ClosePanel2()
        {
            // Write emotion 
            emoDd2 = GameObject.Find("ClosingPanel/Dropdown").GetComponent<Dropdown>();
            int index = emoDd2.value;
            string text = emoDd2.options[index].text;

            writer.WriteLine(text);

            if (Panel2 != null)
            {
                Panel2.SetActive(false);
            }
            saveFirst();
            ClearRecording();
        }

        void AddNewLineRenderer()
        {
            positionCount = 0;
            GameObject go = new GameObject($"LineRenderer_{controlHand.ToString()}_{lines.Count}");
            go.transform.parent = gameObject.transform;
            go.transform.position = objectToTrackMovement.transform.position;

            go.AddComponent<BoxCollider>();
            go.AddComponent<XRGrabInteractable>();

            LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
            goLineRenderer.startWidth = lineDefaultWidth;
            goLineRenderer.endWidth = lineDefaultWidth;
            goLineRenderer.useWorldSpace = true;
            goLineRenderer.material = MaterialUtils.CreateMaterial(defaultColor, $"Material_{controlHand.ToString()}_{lines.Count}");
            
            Shader shader = Shader.Find("Particles/Standard Surface");
            goLineRenderer.material.shader = shader;
            goLineRenderer.material.SetFloat("_Mode", 2); 
            goLineRenderer.material.SetFloat("_Cutoff", 0.5f);

            goLineRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            goLineRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            goLineRenderer.material.SetInt("_ZWrite", 1);
            goLineRenderer.material.EnableKeyword("_ALPHATEST_ON");
            goLineRenderer.material.DisableKeyword("_ALPHABLEND_ON");
            goLineRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            goLineRenderer.material.renderQueue = 4000;

            goLineRenderer.material.SetTexture("_MainTex", textures[defaultTex]);
            goLineRenderer.textureMode = textureMode;
            float tileAmount = 1.0f;
            goLineRenderer.material.SetTextureScale("_MainTex", new Vector2(tileAmount, 1.0f));
            goLineRenderer.positionCount = 1;
            goLineRenderer.numCapVertices = 90;
            goLineRenderer.SetPosition(0, objectToTrackMovement.transform.position);
            
            // send position
            TCPControllerClient.Instance.AddNewLine(objectToTrackMovement.transform.position);
            currentLineRender = goLineRenderer;
            lines.Add(goLineRenderer);
        }
        
        void AddNewShapeRenderer()
        {
            positionCount2 = 0;
            GameObject go = new GameObject($"ShapeLineRenderer_{controlHand.ToString()}_{shapelines.Count}");
            go.transform.parent = gameObject.transform;
            go.transform.position = objectToTrackMovement.transform.position;

            LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
            goLineRenderer.startWidth = lineDefaultWidth_ep;
            goLineRenderer.endWidth = lineDefaultWidth_ep;
            goLineRenderer.useWorldSpace = true;

            goLineRenderer.material = MaterialUtils.CreateMaterial(defaultColor_ep, $"Material_{controlHand.ToString()}_{shapelines.Count}");
            
            goLineRenderer.positionCount = 1;
            goLineRenderer.numCapVertices = 90;
            goLineRenderer.SetPosition(0, objectToTrackMovement.transform.position);
            //goLineRenderer.alignment = LineAlignment.TransformZ;
                
            // send position
            TCPControllerClient.Instance.AddNewLine(objectToTrackMovement.transform.position);

            currentLineRender_ep = goLineRenderer;
            shapelines.Add(goLineRenderer);

        }
        
        void Update()
        {
            Vector3 p = TrackHeadMovement.transform.position; 
            Vector3 r = TrackHeadMovement.transform.eulerAngles;

            Vector3 pl = objectToTrackMovement.transform.position;
           
            Vector3 pr = leftTrackMovement.transform.position;

            if (pause && musicPlay_flag == false)
                    return;

                //if (!vrControllerOptions.IsScreenHidden && musicPlay_flag==false) return;

                if (!device.isValid)
                {
                    device_flag = false;
                    GetDevice();
                }

                float arousal_t;
                float valence_t;

                //determine how many seconds since the song started
                if (musicPlay_flag)
                {
                    songPosition = (float)(AudioSettings.dspTime - dspSongTime);

                    if (iter == 0)
                    {
                        Vector3[] newPos;
                        for (int linecnt = 0; linecnt < lines.Count; linecnt++)
                        {
                            temp = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{linecnt}");
                            comp = temp.GetComponent<LineRenderer>();

                            c_LinePos indline = new c_LinePos();
                            newPos = new Vector3[comp.positionCount];
                            comp.GetPositions(newPos);

                            for (int k = 0; k < comp.positionCount; k++)
                            {
                                indline.Load(newPos[k]);

                            }
                            LineList.Add(indline);
                        }
                    }
                    iter = iter + 1;

                    if (songPosition <= 29.5f)
                    {
                        //if ((songPosition - lastPosition) >= 1.0f) 
                        //{
                        float res = songPosition - Mathf.Floor(songPosition);
                        if (res < 0.5f)
                        {
                            arousal_t = arousal[(int)(songPosition * 2)];
                            valence_t = valence[(int)(songPosition * 2)];
                            //VRStats.Instance.thirdText.text = $"{arousal_t}";
                            //VRStats.Instance.fourthText.text = $"{valence_t}";
                        }
                        else
                        {
                            arousal_t = arousal[(int)(songPosition * 2) + 1];
                            valence_t = valence[(int)(songPosition * 2) + 1];
                            //VRStats.Instance.thirdText.text = $"{arousal_t}";
                            //VRStats.Instance.fourthText.text = $"{valence_t}";
                        }

                        Beziercurve(arousal_t, valence_t, iter);
                        //Beziercurve(arousal_t, valence_t);
                        lastPosition = songPosition;
                        //} 

                    }
                    //Debug.Log(songPosition);

                }

                float _triggerValue = 0.0f;
                bool triggerButtonValue;

                // secondary right controller
            if (musicPlay_flag == false && controlHand == ControlHand.Right)
            {
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerButtonValue))
                {
                    if (triggerButtonValue)
                    {
                        //VRStats.Instance.firstText.text = $"Axis1D.SecondaryIndexTrigger: {triggerButtonValue}"; 
                        if (modalityIdx == 0)
                        {
                            if (temp_triggerButton == false)
                            {
                                UpdateLine(1);
                            }
                            else
                            {
                                UpdateLine(0);
                            } 
                        }
                        else if (modalityIdx == 1)
                        {
                            s_trans = objectToTrackMovement.transform; 
                            //s_pos = objectToTrackMovement.transform.position;
                            middlePos.Add(s_trans.position);

                            if (temp_triggerButton == false)
                            {
                                UpdateLineforShape(s_trans, 1);
                            }
                            else
                            {
                                UpdateLineforShape(s_trans, 0);
                            }
                            LineFlag = false;
                        }
                        temp_triggerButton = true;
                    }
                    else
                    {
                        if (temp_triggerButton)
                        {
                            //VRStats.Instance.secondText.text = $"Button.Secondary: {triggerButtonValue}";
                            if (modalityIdx == 0)
                            {
                                AddNewLineRenderer();
                            }
                            else if (modalityIdx == 1)
                            {
                                AddNewShapeRenderer();
                                //AddNewShapeRenderer2();
                            }
                        }
                        temp_triggerButton = false;
                    }
                }
                    //temp_triggerButton = triggerButtonValue;
                }
                for (int i = 0; i < shapeCount; i++)
                {
                    shapeObj = GameObject.Find($"ShapeRenderer_{controlHand.ToString()}_{i}");

                    Graphics.DrawMesh(shapeObj.GetComponent<MeshFilter>().mesh, Vector3.zero, Quaternion.identity, shapeObj.GetComponent<MeshRenderer>().materials[0], 0);
                }

            }

            private List<Vector3> DrawCubicBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3)
            {
                List<Vector3> drawingPoints = new List<Vector3>();

                float t = 0f;
                Vector3 B = new Vector3(0, 0, 0);

                int localpC = 0;

                while (t <= 1f)
                {
                    B = (1 - t) * (1 - t) * (1 - t) * point0 + 3 * (1 - t) * (1 - t) *
                        t * point1 + 3 * (1 - t) * t * t * point2 + t * t * t * point3;

                    drawingPoints.Add(B);
                    t += 0.1f;
                }

                return drawingPoints;
            }

        void UpdateLine(int start) 
        {
            if (prevPointDistance == null)
            {
                prevPointDistance = objectToTrackMovement.transform.position;
            }

            if (prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, objectToTrackMovement.transform.position)) >= minDistanceBeforeNewPoint)
            {
                Vector3 dir = (objectToTrackMovement.transform.position - Camera.main.transform.position).normalized;
                prevPointDistance = objectToTrackMovement.transform.position;
                AddPoint(prevPointDistance, dir);
                if (saveData_flag && startPaint_flag)
                {
                    writer.WriteLine(start.ToString(format) +
                    "," + selectedEmo.ToString(format) +
                    "," + defaultTex.ToString(format) +
                    "," + defaultColor.r.ToString(format) +
                    "," + defaultColor.g.ToString(format) +
                    "," + defaultColor.b.ToString(format) +
                    "," + prevPointDistance.x.ToString(format) +
                    "," + prevPointDistance.y.ToString(format) +
                    "," + prevPointDistance.z.ToString(format) +
                    "," + lineDefaultWidth.ToString(format) +
                    "," + 0.ToString(format));
                }
                else if (saveSecond_flag && startPaint_flag) 
                {
                    writer2.WriteLine(start.ToString(format) +
                    "," + selectedEmo.ToString(format) +
                    "," + defaultTex.ToString(format) +
                    "," + defaultColor.r.ToString(format) +
                    "," + defaultColor.g.ToString(format) +
                    "," + defaultColor.b.ToString(format) +
                    "," + prevPointDistance.x.ToString(format) +
                    "," + prevPointDistance.y.ToString(format) +
                    "," + prevPointDistance.z.ToString(format) +
                    "," + lineDefaultWidth.ToString(format) +
                    "," + 0.ToString(format));
                }
            }
        }
        
        void UpdateLineforShape(Transform c_Trans, int start)  
        {
            Vector3 cP = c_Trans.position; 
            if (saveData_flag && startPaint_flag)
            {
                writer.WriteLine(start.ToString(format) +
                    "," + selectedEmo.ToString(format) +
                    "," + "NA" +
                    "," + defaultColor_ep.r.ToString(format) +
                    "," + defaultColor_ep.g.ToString(format) +
                    "," + defaultColor_ep.b.ToString(format) +
                    "," + cP.x.ToString(format) +
                    "," + cP.y.ToString(format) +
                    "," + cP.z.ToString(format) +
                    "," + lineDefaultWidth_ep.ToString(format) +
                    "," + 1.ToString(format) +
                    "," + c_Trans.position.x.ToString(format) +
                    "," + c_Trans.position.y.ToString(format) +
                    "," + c_Trans.position.z.ToString(format) +
                    "," + c_Trans.eulerAngles.x.ToString(format) +
                    "," + c_Trans.eulerAngles.y.ToString(format) +
                    "," + c_Trans.eulerAngles.z.ToString(format) +
                    "," + c_Trans.localScale.x.ToString(format) +
                    "," + c_Trans.localScale.y.ToString(format) +
                    "," + c_Trans.localScale.z.ToString(format)); 
                    
            }
            else if (saveSecond_flag && startPaint_flag)
            {
                writer2.WriteLine(start.ToString(format) +
                    "," + selectedEmo.ToString(format) +
                    "," + "NA" +
                    "," + defaultColor_ep.r.ToString(format) +
                    "," + defaultColor_ep.g.ToString(format) +
                    "," + defaultColor_ep.b.ToString(format) +
                    "," + cP.x.ToString(format) +
                    "," + cP.y.ToString(format) +
                    "," + cP.z.ToString(format) +
                    "," + lineDefaultWidth_ep.ToString(format) +
                    "," + 1.ToString(format) +
                    "," + c_Trans.position.x.ToString(format) +
                    "," + c_Trans.position.y.ToString(format) +
                    "," + c_Trans.position.z.ToString(format) +
                    "," + c_Trans.eulerAngles.x.ToString(format) +
                    "," + c_Trans.eulerAngles.y.ToString(format) + 
                    "," + c_Trans.eulerAngles.z.ToString(format) +
                    "," + c_Trans.localScale.x.ToString(format) +
                    "," + c_Trans.localScale.y.ToString(format) +
                    "," + c_Trans.localScale.z.ToString(format));                     
            }

            int bezierSegmentsPerCurve = 25;
            total_pointlist_VRDraw.Clear();

            VRStats.Instance.thirdText.text = $"selectedEmo: {selectedEmo}"; 
            VRStats.Instance.fourthText.text = $"Valence: {valence_l}";
            
            arousal_l = arousal_arr[selectedEmo];
            float arousal_i = arousal_arr[selectedEmo] / 9;
            float valence_i = valence_arr[selectedEmo] / 9;

            int divis = arousal_l;

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

                var p2 = new Vector2() { x = scaling * Mathf.Cos(e_angle / 2) + arousal_l, y = scaling * Mathf.Sin(-e_angle / 2) + valence_prop };
                var p3 = new Vector2() { x = scaling * Mathf.Cos(-e_angle / 2) + arousal_l, y = scaling * Mathf.Sin(e_angle / 2) - valence_prop };
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
                    Vector2 rotatedV = RotateVector(drawingPoints[k], -angle);
                    drawingPoints[k] = ScaleVector(rotatedV, 0.01f);
                    total_pointlist_VRDraw.Add(drawingPoints[k]);
                }
            }
            
            Vector2[] pointlist = total_pointlist_VRDraw.ToArray();
            var pointVec3 = new Vector3[pointlist.Length];

            for (int i = 0; i < pointlist.Length; i++)
            {  
                pointVec3[i].x = pointlist[i].x;
                pointVec3[i].y = pointlist[i].y;
                pointVec3[i].z = 0f; 
                 
                pointVec3[i] = c_Trans.TransformPoint(pointVec3[i]);
                //pointVec3[i] = pointVec3[i] + cP;
            }
            //Vector3 targetDirection = c_Trans.forward;
            //c_Trans.rotation = Quaternion.LookRotation(targetDirection); 

            shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{shapelines.Count - 1}");

            float rot_angle = Vector3.Angle(c_Trans.forward, new Vector3(0, 0, 1));
            //shapelineObj.transform.Rotate(0.0f, 0.0f, rot_angle);
            shapelineObj.GetComponent<LineRenderer>().positionCount = pointlist.Length;
            shapelineObj.GetComponent<LineRenderer>().SetPositions(pointVec3);

            positionCount2 = positionCount2 + pointlist.Length;

        }

            public Vector2 RotateVector(Vector2 v, float angle)
            {
                float _x = v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle);
                float _y = v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle);
                return new Vector2(_x, _y);
            }
            public Vector2 ScaleVector(Vector2 v, float scale)
            {
                float _x = v.x * scale;
                float _y = v.y * scale;
                return new Vector2(_x, _y);
            }

            Vector3[] MakeQuad(Vector3 s, Vector3 e)
            {
                Vector3 dir = (objectToTrackMovement.transform.position - Camera.main.transform.position).normalized;

                float w = 1.0f;
                //w = w / 2;
                Vector3[] q = new Vector3[4];

                Vector3 n = Vector3.Cross(s, e);
                Vector3 l = Vector3.Cross(n, e - s);
                l.Normalize();

                //if (IsPinchingReleased )

                float dist = Mathf.Sqrt((e.x - s.x) * (e.x - s.x) + (e.z - s.z) * (e.z - s.z));

                w = dist / 2;

                float y_l = (s.y + e.y) / 2;

                Vector3 s_y0 = s; s_y0.y = y_l;
                Vector3 e_y0 = e; e_y0.y = y_l;

                Vector3 diff = e_y0 - s_y0;
                diff.Normalize();

                Vector3 normal = new Vector3(1f, 0, -diff.x / diff.z);
                normal.Normalize();
                //normal = w * normal;  
                {

                    q[0] = s_y0 + normal * w;
                    q[1] = s_y0 + normal * -w;
                    q[2] = e_y0 + normal * w;
                    q[3] = e_y0 + normal * -w;
                }

                return q;
            }

            void AddLine(Mesh m, Vector3[] quad, bool tmp, int normal, Vector3 s, Vector3 e)
            {
                int vl = m.vertices.Length;

                Vector3[] vs = m.vertices;

                //if (!tmp || vl == 0)
                if (vl == 0)
                {
                    vs = resizeVertices(vs, 4);
                }
                else
                {
                    vl = 0;
                }

                vs[0] = quad[0];
                vs[1] = quad[1];
                vs[2] = quad[2];
                vs[3] = quad[3];

                int tl = m.triangles.Length;

                int[] ts = m.triangles;
                //if (!tmp || tl == 0)
                if (tl == 0)
                {
                    ts = resizeTriangles(ts, 6);
                }
                else tl = 0;

                ts[tl] = vl;
                ts[tl + 1] = vl + 1;
                ts[tl + 2] = vl + 2;
                ts[tl + 3] = vl + 1;
                ts[tl + 4] = vl + 3;
                ts[tl + 5] = vl + 2;

                float y_l = (s.y + e.y) / 2;

                Vector3 s_y0 = s; s_y0.y = y_l;
                Vector3 e_y0 = e; e_y0.y = y_l;

                Vector3 n = Vector3.Cross(s_y0, e_y0);
                n = new Vector3(0, 1, 0);
                Vector3[] normals = new Vector3[4]
                {n, n, n, n};
                Vector3[] n_normals = new Vector3[4]
                {-n, -n, -n, -n};

                if (normal == 1)
                {
                    m.normals = normals;
                }
                else
                {
                    m.normals = n_normals;
                }

                m.Clear();
                m.vertices = vs;
                m.triangles = ts;

                //m.RecalculateBounds();
                m.RecalculateNormals();

            }

            Vector3[] resizeVertices(Vector3[] ovs, int ns)
            {
                Vector3[] nvs = new Vector3[ovs.Length + ns];
                for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
                return nvs;
            }
            int[] resizeTriangles(int[] ovs, int ns)
            {
                int[] nvs = new int[ovs.Length + ns];
                for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
                return nvs;
            }
            void AddPoint(Vector3 position, Vector3 direction)
            {
                //lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{lines.Count - 1}"); 
                //lineObj.GetComponent<LineRenderer>().SetPosition(positionCount, position);

                currentLineRender.SetPosition(positionCount, position);
                positionCount++;
                //lineObj.GetComponent<LineRenderer>().positionCount = positionCount + 1; 
                //lineObj.GetComponent<LineRenderer>().SetPosition(positionCount, position);
                
                currentLineRender.positionCount = positionCount + 1;
                currentLineRender.SetPosition(positionCount, position);

                // send position
                TCPControllerClient.Instance.UpdateLine(position);
            }

            public void UpdateLineWidth(float newValue)
            {
                currentLineRender.startWidth = newValue;
                currentLineRender.endWidth = newValue;
                lineDefaultWidth = newValue;
            }

            public void UpdateShapeLineWidth(float newValue)
            {
                currentLineRender_ep.startWidth = newValue;
                currentLineRender_ep.endWidth = newValue;
                lineDefaultWidth_ep = newValue;
            }

            public void UpdateLineColor(Color color)
            {
                // in case we haven't drawn anything
                if (currentLineRender.positionCount == 1)
                {
                    currentLineRender.material.color = color;
                    currentLineRender.material.EnableKeyword("_EMISSION");
                    currentLineRender.material.SetColor("_EmissionColor", color);
                } 
                defaultColor = color;
            }

            public void UpdateLineMinDistance(float newValue)
            {
                minDistanceBeforeNewPoint = newValue;
            }

            public void UpdatePauseHover(bool hover)
            {
                if (hover)
                {
                    pause = true;
                    
                }
                else
                {
                    pause = false;
                    
                }
            }


        public void ColorChanged(Image sender)
        {
            Color color = sender.color;

            int i = lines.Count - 1;
            lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{i}");

            // Change line/shape colors 

            // in case we haven't drawn anything
            //if (currentLineRender.positionCount == 1)
            //{
            colorIdx = System.Convert.ToInt32(sender.gameObject.name);

            currentLineRender.material.color = color;
            currentLineRender.material.EnableKeyword("_EMISSION");
            currentLineRender.material.SetColor("_EmissionColor", color);
            
            lineObj.GetComponent<LineRenderer>().material.SetColor("_Color", color);
            lineObj.GetComponent<LineRenderer>().material.EnableKeyword("_EMISSION");
            lineObj.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", color);
            
            colorIdxes.Add(colorIdx);
            IdxofCIdx.Add(i);    
            //}
            defaultColor = color;
        }

        public void ShapeColorChanged(Image sender)
        {
            Color color = sender.color;

            shapeColorIdx = System.Convert.ToInt32(sender.gameObject.name);
            
            currentLineRender_ep.material.color = color;
            currentLineRender_ep.material.EnableKeyword("_EMISSION");
            currentLineRender_ep.material.SetColor("_EmissionColor", color);

            int si = shapelines.Count - 1;
            shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{si}");
            shapelineObj.GetComponent<LineRenderer>().material.SetColor("_Color", color);
            shapelineObj.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", color);

            shapeColorIdxes.Add(shapeColorIdx);
            shapeIdxofCIdx.Add(si);
            
            defaultColor_ep = color;
        }

        public void LineMaterialChanged(int idx)
        {
            int i = lines.Count - 1;
            lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{i}");

            lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[idx]);
            currentLineRender.material.SetTexture("_MainTex", textures[idx]);
            //lineObj.GetComponent<LineRenderer>().material = linemat[15 + index];
            defaultTex = idx;
        }

        public void CurrentEmotionChanged(int idx)
        {
            selectedEmo = idx;
            LineMaterialBasedonEmotion(selectedEmo); 
        }
        public void LineMaterialBasedonEmotion(int idx)
        {
            int i = lines.Count - 1;
            lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{i}");

            Dropdown dropdown;
            
            if (idx == 0)
            {
                dropdown = GameObject.Find("Line1/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[index]);
                currentLineRender.material.SetTexture("_MainTex", textures[index]);
                defaultTex = index;
                //lineObj.GetComponent<LineRenderer>().material = linemat[index];  
            }
            else if (idx == 1)
            {
                dropdown = GameObject.Find("Line2/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[3 + index]);
                currentLineRender.material.SetTexture("_MainTex", textures[3 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[3 + index];
                defaultTex = 3 + index;
            }
            else if (idx == 2)
            {
                dropdown = GameObject.Find("Line3/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[6 + index]);
                currentLineRender.material.SetTexture("_MainTex", textures[6 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[6 + index];
                defaultTex = 6 + index;
            }
            else if (idx == 3)
            {
                dropdown = GameObject.Find("Line4/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[9 + index]);
                currentLineRender.material.SetTexture("_MainTex", textures[9 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[9 + index];
                defaultTex = 9 + index;
            }
            else if (idx == 4)
            {
                dropdown = GameObject.Find("Line5/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[12 + index]);
                currentLineRender.material.SetTexture("_MainTex", textures[12 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[12 + index];
                defaultTex = 12 + index;
            }
            else if (idx == 5)
            {
                dropdown = GameObject.Find("Line6/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[15 + index]);
                currentLineRender.material.SetTexture("_MainTex", textures[15 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[15 + index];
                defaultTex = 15 + index;
            }
            else
            {
                dropdown = GameObject.Find("Line7/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[index]);
                currentLineRender.material.SetTexture("_MainTex", textures[index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[15 + index];
                defaultTex = index;
            }
            //linematerial = linemat[idx];
            // }
        }

        public void LineMaterialBasedonEmotion_idx(int idx)
        {
            int i = lines.Count - 1;
            lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{i}");

            lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[idx]);
            currentLineRender.material.SetTexture("_MainTex", textures[idx]);
            //lineObj.GetComponent<LineRenderer>().material = linemat[15 + index];
            defaultTex = idx;
        }


        public void LineMaterialBasedonEmotion(int idx, int i) 
        {
            //for (int i = 0; i < lines.Count; i++)
            //{
            //int i = lines.Count - 1;
            lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{i}");

            Dropdown dropdown;
            // dropdown.options.Clear(); 

            if (idx == 0)
            {
                dropdown = GameObject.Find("Line1/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[index]);
                defaultTex = index;
                //lineObj.GetComponent<LineRenderer>().material = linemat[index];  
            }
            else if (idx == 1)
            {
                dropdown = GameObject.Find("Line2/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[3 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[3 + index];
                defaultTex = 3 + index;
            }
            else if (idx == 2)
            {
                dropdown = GameObject.Find("Line3/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[6 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[6 + index];
                defaultTex = 6 + index;
            }
            else if (idx == 3)
            {
                dropdown = GameObject.Find("Line4/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[9 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[9 + index];
                defaultTex = 9 + index;
            }
            else if (idx == 4)
            {
                dropdown = GameObject.Find("Line5/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[12 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[12 + index];
                defaultTex = 12 + index;
            }
            else if (idx == 5)
            {
                dropdown = GameObject.Find("Line6/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[15 + index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[15 + index];
                defaultTex = 15 + index;
            }
            else
            {
                dropdown = GameObject.Find("Line7/Dropdown").GetComponent<Dropdown>();
                int index = dropdown.value;
                string text = dropdown.options[index].text;

                lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[index]);
                //lineObj.GetComponent<LineRenderer>().material = linemat[15 + index];
                defaultTex = index;
            }
        }

        public void ChangeLineBasedonEmotion(int idx)
            {
                Dropdown dropdown;
                Dropdown dropdown2;

                for (int i = 0; i < lines.Count; i++)
                {
                    //int i = lines.Count - 1;
                    lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{i}");

                    // dropdown.options.Clear(); 
                    if (idx == 0)
                    {
                        dropdown = GameObject.Find("Line1/Dropdown").GetComponent<Dropdown>();
                        int index = dropdown.value;
                        string text = dropdown.options[index].text;

                        lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[index]);
                        defaultTex = index;
                        //lineObj.GetComponent<LineRenderer>().material = linemat[index];  
                    }
                    else if (idx == 1)
                    {
                        dropdown = GameObject.Find("Line2/Dropdown").GetComponent<Dropdown>();
                        int index = dropdown.value;
                        string text = dropdown.options[index].text;

                        lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[3 + index]);
                        //lineObj.GetComponent<LineRenderer>().material = linemat[3 + index];
                        defaultTex = 3 + index;
                    }
                    else if (idx == 2)
                    {
                        dropdown = GameObject.Find("Line3/Dropdown").GetComponent<Dropdown>();
                        int index = dropdown.value;
                        string text = dropdown.options[index].text;

                        lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[6 + index]);
                        //lineObj.GetComponent<LineRenderer>().material = linemat[6 + index];
                        defaultTex = 6 + index;
                    }
                    else if (idx == 3)
                    {
                        dropdown = GameObject.Find("Line4/Dropdown").GetComponent<Dropdown>();
                        int index = dropdown.value;
                        string text = dropdown.options[index].text;

                        lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[9 + index]);
                        //lineObj.GetComponent<LineRenderer>().material = linemat[9 + index];
                        defaultTex = 9 + index;
                    }
                    else if (idx == 4)
                    {
                        dropdown = GameObject.Find("Line5/Dropdown").GetComponent<Dropdown>();
                        int index = dropdown.value;
                        string text = dropdown.options[index].text;

                        lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[12 + index]);
                        //lineObj.GetComponent<LineRenderer>().material = linemat[12 + index];
                        defaultTex = 12 + index;
                    }
                    else if (idx == 5)
                    {
                        dropdown = GameObject.Find("Line6/Dropdown").GetComponent<Dropdown>();
                        int index = dropdown.value;
                        string text = dropdown.options[index].text;

                        lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[15 + index]);
                        //lineObj.GetComponent<LineRenderer>().material = linemat[15 + index];
                        defaultTex = 15 + index;
                    }
                    else
                    {
                        dropdown = GameObject.Find("Line7/Dropdown").GetComponent<Dropdown>();
                        int index = dropdown.value;
                        string text = dropdown.options[index].text;

                        lineObj.GetComponent<LineRenderer>().material.SetTexture("_MainTex", textures[index]);
                        //lineObj.GetComponent<LineRenderer>().material = linemat[15 + index];
                        defaultTex = index;
                    }
                    //linematerial = linemat[idx];
                }

                colorIdxes.Add(colorIdx);
                IdxofCIdx.Add(lines.Count - 1);

                // Lines Color Change 
                for (int i = 0; i < (colorIdxes.Count - 1); i++)
                {
                    for (int j = IdxofCIdx[i]; j < IdxofCIdx[i + 1]; j++)
                    {
                        int idxcnt;
                        if (idx <= 5)
                        {
                            idxcnt = idx * 20 + colorIdxes[i];
                        }
                        else
                        {
                            dropdown = GameObject.Find("Color7/Dropdown").GetComponent<Dropdown>();
                            int index = dropdown.value;
                            idxcnt = index * 20 + colorIdxes[i];
                        }

                        myRgbColor = new Color(float.Parse(colorpalette[idxcnt][0]), float.Parse(colorpalette[idxcnt][1]), float.Parse(colorpalette[idxcnt][2]));
                        lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{j}");
                        lineObj.GetComponent<LineRenderer>().material.SetColor("_Color", myRgbColor);
                        lineObj.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", myRgbColor);
                    }
                }
            }

            void ChangeShape(int a, int v, int shapeidx)
            {
                int bezierSegmentsPerCurve = 10;
                total_pointlist_VRDraw.Clear();

                //LineRenderer.Points = total_pointlist.ToArray();

                //total_pointlist_VRDraw = GameObject.Find("Shape1/SelectedColor").GetComponent<TestAddingPoints>().total_pointlist;

                VRStats.Instance.thirdText.text = $"Arousal: {a}";
                VRStats.Instance.fourthText.text = $"Valence: {v}";

                float arousal_i = a / 9;
                float valence_i = v / 9;

                int divis = a;

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

                    var p2 = new Vector2() { x = scaling * Mathf.Cos(e_angle / 2) + a, y = scaling * Mathf.Sin(-e_angle / 2) + valence_prop };

                    var p3 = new Vector2() { x = scaling * Mathf.Cos(-e_angle / 2) + a, y = scaling * Mathf.Sin(e_angle / 2) - valence_prop };

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
                        Vector2 rotatedV = RotateVector(drawingPoints[k], -angle);
                        drawingPoints[k] = ScaleVector(rotatedV, 0.01f);
                        total_pointlist_VRDraw.Add(drawingPoints[k]);
                    }
                }

                Vector2[] pointlist = total_pointlist_VRDraw.ToArray();
                var pointVec3 = new Vector3[pointlist.Length];

                for (int i = 0; i < pointlist.Length; i++)
                {
                    /*pointVec3[i].x = pointlist[i].x + middlePos[shapeidx].x;
                    pointVec3[i].y = pointlist[i].y + middlePos[shapeidx].y;
                    pointVec3[i].z = middlePos[shapeidx].z;
                    */
                    pointVec3[i].x = pointlist[i].x;
                    pointVec3[i].y = pointlist[i].y;
                    pointVec3[i].z = 0.0f;
                    pointVec3[i] = shapelineObj.transform.TransformPoint(pointVec3[i]);
                }

                shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{shapeidx}");
                GameObject.Destroy(shapelineObj.GetComponent<MeshRenderer>());
                shapelineObj.GetComponent<LineRenderer>().positionCount = pointlist.Length;
                shapelineObj.GetComponent<LineRenderer>().SetPositions(pointVec3);

            }

        public void ArousalChanged_0()
        {
            arousal_arr[0] = (int)arousal0.value; 
        }
        public void ArousalChanged_1()
        {
            arousal_arr[1] = (int)arousal1.value;
        }

        public void ArousalChanged_2()
        {
            arousal_arr[2] = (int)arousal2.value;
        }

        public void ArousalChanged_3()
        {
            arousal_arr[3] = (int)arousal3.value;
        }

        public void ArousalChanged_4()
        {
            arousal_arr[4] = (int)arousal4.value;
        }
        public void ArousalChanged_5()
        {
            arousal_arr[5] = (int)arousal5.value;
        }
        public void ArousalChanged_6()
        {
            arousal_arr[6] = (int)arousal6.value;
        }
        public void ValenceChanged_0()
        {
            valence_arr[0] = (int)valence0.value; 
        }
        public void ValenceChanged_1()
        {
            valence_arr[1] = (int)valence1.value;
        }
        public void ValenceChanged_2()
        {
            valence_arr[2] = (int)valence2.value;
        }
        public void ValenceChanged_3()
        {
            valence_arr[3] = (int)valence3.value;
        }
        public void ValenceChanged_4()
        {
            valence_arr[4] = (int)valence4.value;
        }
        public void ValenceChanged_5()
        {
            valence_arr[5] = (int)valence5.value;
        }
        public void ValenceChanged_6()
        {
            valence_arr[6] = (int)valence6.value;
        }

        public void ShapeParameterBasedonEmotion(int idx)
        {
            //shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{shapelines.Count - 1}");

            //for (int i = 0; i < shapelines.Count; i++)
            int i = shapelines.Count - 1;
            if (idx == 0)
            {
                //arousal_l = GameObject.Find("Shape1/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                //valence_l = GameObject.Find("Shape1/SelectedColor").GetComponent<TestAddingPoints>().valence;
                arousal_l = arousal_arr[0];
                valence_l = valence_arr[0]; 
                ChangeShape(arousal_l, valence_l, i);
            }
            else if (idx == 1)
            {
                //arousal_l = GameObject.Find("Shape2/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                //valence_l = GameObject.Find("Shape2/SelectedColor").GetComponent<TestAddingPoints>().valence;
                arousal_l = arousal_arr[1];
                valence_l = valence_arr[1];
                ChangeShape(arousal_l, valence_l, i);
            }
            else if (idx == 2)
            {
                //arousal_l = GameObject.Find("Shape3/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                //valence_l = GameObject.Find("Shape3/SelectedColor").GetComponent<TestAddingPoints>().valence;
                arousal_l = arousal_arr[2];
                valence_l = valence_arr[2];
                ChangeShape(arousal_l, valence_l, i);
            }
            else if (idx == 3)
            {
                //arousal_l = GameObject.Find("Shape4/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                //valence_l = GameObject.Find("Shape4/SelectedColor").GetComponent<TestAddingPoints>().valence;
                arousal_l = arousal_arr[3];
                valence_l = valence_arr[3];
                ChangeShape(arousal_l, valence_l, i);
            }
            else if (idx == 4)
            {
                //arousal_l = GameObject.Find("Shape5/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                //valence_l = GameObject.Find("Shape5/SelectedColor").GetComponent<TestAddingPoints>().valence;
                arousal_l = arousal_arr[4];
                valence_l = valence_arr[4];
                ChangeShape(arousal_l, valence_l, i);
            }
            else if (idx == 5)
            {
                //arousal_l = GameObject.Find("Shape6/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                //valence_l = GameObject.Find("Shape6/SelectedColor").GetComponent<TestAddingPoints>().valence;
                arousal_l = arousal_arr[5];
                valence_l = valence_arr[5];
                ChangeShape(arousal_l, valence_l, i);
            }
            else
            {
                //arousal_l = GameObject.Find("Shape7/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                //valence_l = GameObject.Find("Shape7/SelectedColor").GetComponent<TestAddingPoints>().valence;
                arousal_l = arousal_arr[6];
                valence_l = valence_arr[6];
                ChangeShape(arousal_l, valence_l, i);
            }
        }

            void LineRenderShape(int a, int v, int shapeidx)
            {
                int bezierSegmentsPerCurve = 10;
                total_pointlist2_VRDraw.Clear();

                //LineRenderer.Points = total_pointlist.ToArray();

                //total_pointlist_VRDraw = GameObject.Find("Shape1/SelectedColor").GetComponent<TestAddingPoints>().total_pointlist;

                VRStats.Instance.thirdText.text = $"Arousal: {a}";
                VRStats.Instance.fourthText.text = $"Valence: {v}";

                float arousal_i = a / 9;
                float valence_i = v / 9;

                int divis = a;

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

                    var p2 = new Vector2() { x = scaling * Mathf.Cos(e_angle / 2) + a, y = scaling * Mathf.Sin(-e_angle / 2) + valence_prop };

                    var p3 = new Vector2() { x = scaling * Mathf.Cos(-e_angle / 2) + a, y = scaling * Mathf.Sin(e_angle / 2) - valence_prop };

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
                        Vector2 rotatedV = RotateVector(drawingPoints[k], -angle);
                        drawingPoints[k] = ScaleVector(rotatedV, 0.01f);
                        total_pointlist2_VRDraw.Add(drawingPoints[k]);
                    }
                }

                Vector2[] pointlist = total_pointlist2_VRDraw.ToArray();
                var pointVec3 = new Vector3[pointlist.Length];

                for (int i = 0; i < pointlist.Length; i++)
                {
                    /*pointVec3[i].x = pointlist[i].x + middlePos[shapeidx].x;
                    pointVec3[i].y = pointlist[i].y + middlePos[shapeidx].y;
                    pointVec3[i].z = middlePos[shapeidx].z;
                    */
                    pointVec3[i].x = pointlist[i].x;
                    pointVec3[i].y = pointlist[i].y;
                    pointVec3[i].z = 0.0f;
                    pointVec3[i] = shapelineObj.transform.TransformPoint(pointVec3[i]);
                }

                shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{shapeidx}");
                LineRenderer goLineRenderer;
                if (shapelineObj.TryGetComponent(out MeshRenderer render))
                {
                    GameObject.Destroy(render);

                    goLineRenderer = shapelineObj.AddComponent<LineRenderer>();
                    goLineRenderer.startWidth = lineDefaultWidth_ep;
                    goLineRenderer.endWidth = lineDefaultWidth_ep;
                    goLineRenderer.useWorldSpace = true;

                    Color c_shape;
                    c_shape = shapelineObj.GetComponent<MeshRenderer>().sharedMaterial.color;
                    goLineRenderer.material = MaterialUtils.CreateMaterial(c_shape, $"ShapeMaterial_{controlHand.ToString()}_{shapeidx}");
                
                    Shader shader = Shader.Find("Particles/Standard Unlit");
                    //Material particleMat = new Material(shader);
                    //goLineRenderer.material = particleMat; 
                    
                    goLineRenderer.material.shader = shader;
                    goLineRenderer.material.SetFloat("_Mode", 2);
                    goLineRenderer.material.SetFloat("_Cutoff", 0.5f);

                    goLineRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    goLineRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    goLineRenderer.material.SetInt("_ZWrite", 1);
                    goLineRenderer.material.EnableKeyword("_ALPHATEST_ON");
                    goLineRenderer.material.DisableKeyword("_ALPHABLEND_ON");
                    goLineRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    goLineRenderer.material.renderQueue = 4000;

                    goLineRenderer.textureMode = textureMode;
                    float tileAmount = 1.0f;
                    goLineRenderer.material.SetTextureScale("_MainTex", new Vector2(tileAmount, 1.0f));
                    goLineRenderer.positionCount = 1;
                    goLineRenderer.numCapVertices = 90;

                    goLineRenderer.positionCount = pointlist.Length;
                    goLineRenderer.SetPositions(pointVec3);
                }
                else // when only line renderer left
                {
                    goLineRenderer = shapelineObj.GetComponent<LineRenderer>();
                    goLineRenderer.positionCount = pointlist.Length;
                    goLineRenderer.SetPositions(pointVec3);
                }
            }

            public void ChangeShapeBasedonEmotion(int idx)
            {
                Dropdown dropdown;

                //shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{shapelines.Count - 1}");
                int arousal;
                int valence;

                for (int i = 0; i < (shapelines.Count - 1); i++)
                {
                    //int i = 0;
                    //int i = shapelines.Count - 1;
                    if (idx == 0)
                    {
                        arousal = GameObject.Find("Shape1/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                        valence = GameObject.Find("Shape1/SelectedColor").GetComponent<TestAddingPoints>().valence;
                        LineRenderShape(arousal, valence, i);
                    }
                    else if (idx == 1)
                    {
                        arousal = GameObject.Find("Shape2/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                        valence = GameObject.Find("Shape2/SelectedColor").GetComponent<TestAddingPoints>().valence;
                        LineRenderShape(arousal, valence, i);
                    }
                    else if (idx == 2)
                    {
                        arousal = GameObject.Find("Shape3/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                        valence = GameObject.Find("Shape3/SelectedColor").GetComponent<TestAddingPoints>().valence;
                        LineRenderShape(arousal, valence, i);
                    }
                    else if (idx == 3)
                    {
                        arousal = GameObject.Find("Shape4/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                        valence = GameObject.Find("Shape4/SelectedColor").GetComponent<TestAddingPoints>().valence;
                        LineRenderShape(arousal, valence, i);
                    }
                    else if (idx == 4)
                    {
                        arousal = GameObject.Find("Shape5/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                        valence = GameObject.Find("Shape5/SelectedColor").GetComponent<TestAddingPoints>().valence;
                        LineRenderShape(arousal, valence, i);
                    }
                    else if (idx == 5)
                    {
                        arousal = GameObject.Find("Shape6/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                        valence = GameObject.Find("Shape6/SelectedColor").GetComponent<TestAddingPoints>().valence;
                        LineRenderShape(arousal, valence, i);
                    }
                    else
                    {
                        arousal = GameObject.Find("Shape7/SelectedColor").GetComponent<TestAddingPoints>().arousal;
                        valence = GameObject.Find("Shape7/SelectedColor").GetComponent<TestAddingPoints>().valence;
                        LineRenderShape(arousal, valence, i);
                    }
                    //linematerial = linemat[idx];
                }

                shapeColorIdxes.Add(shapeColorIdx);
                shapeIdxofCIdx.Add(shapelines.Count - 1);

                // Shape Color Change 
                for (int i = 0; i < (shapeColorIdxes.Count - 1); i++)
                {
                    for (int j = shapeIdxofCIdx[i]; j < shapeIdxofCIdx[i + 1]; j++)
                    {
                        int idxcnt;
                        if (idx <= 5)
                        {
                            idxcnt = idx * 20 + shapeColorIdxes[i];
                        }
                        else
                        {
                            dropdown = GameObject.Find("Color7/Dropdown").GetComponent<Dropdown>();
                            int index = dropdown.value;
                            idxcnt = index * 20 + shapeColorIdxes[i];
                        }

                        myRgbColor = new Color(float.Parse(colorpalette[idxcnt][0]), float.Parse(colorpalette[idxcnt][1]), float.Parse(colorpalette[idxcnt][2]));

                        shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{j}");
                        shapelineObj.GetComponent<LineRenderer>().material.SetColor("_Color", myRgbColor);
                        shapelineObj.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", myRgbColor);
                    }
                }
            }

        public void Clear()
        {
            if (saveData_flag)
            {
                writer.WriteLine(2.ToString(format));
            }
            else if (saveSecond_flag)
            {
                writer2.WriteLine(2.ToString(format));
            }

            int cnt = lines.Count;
            lines.Clear();
            for (int i = 0; i < cnt; i++)
            {
                //int i = lines.Count - 1;
                lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{i}");
                GameObject.Destroy(lineObj); 
            }

            int shapecnt = shapelines.Count;
            shapelines.Clear();
            for (int i = 0; i < shapecnt; i++)
            {
                //int i = lines.Count - 1;
                shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{i}");
                GameObject.Destroy(shapelineObj);
            } 

            lines = new List<LineRenderer>();
            shapelines = new List<LineRenderer>(); 

            AddNewLineRenderer();
            AddNewShapeRenderer();

            Dropdown dropdown = GameObject.Find("VRSelectedPalette/Selected").GetComponent<Dropdown>();
            int index = dropdown.value;

            LineMaterialBasedonEmotion(index);
            ShapeParameterBasedonEmotion(index);
        }

        public void ClearRecording()
        {
            int cnt = lines.Count;
            lines.Clear();
            for (int i = 0; i < cnt; i++)
            {
                //int i = lines.Count - 1;
                lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{i}");
                GameObject.Destroy(lineObj);
            }

            int shapecnt = shapelines.Count;
            shapelines.Clear();
            for (int i = 0; i < shapecnt; i++)
            {
                //int i = lines.Count - 1;
                shapelineObj = GameObject.Find($"ShapeLineRenderer_{controlHand.ToString()}_{i}");
                GameObject.Destroy(shapelineObj);
            }

            lines = new List<LineRenderer>();
            shapelines = new List<LineRenderer>();

            AddNewLineRenderer();
            AddNewShapeRenderer();

            Dropdown dropdown = GameObject.Find("VRSelectedPalette/Selected").GetComponent<Dropdown>();
            int index = dropdown.value;

            LineMaterialBasedonEmotion(index);
            ShapeParameterBasedonEmotion(index);
        }

        public void StartPlayingSong()
            {
                //Record the time when the music starts
                dspSongTime = (float)AudioSettings.dspTime;

                //musicSource.time = 13.21f;
                musicSource.time = 15f;
                //Start the music
                musicSource.Play();

                musicPlay_flag = true;
            }

        public void StopRecording() 
        {
            Color BGColor = GameObject.Find("BGColor/colorPanel").GetComponent<bgColorHandler>().currentBGColor;
            writer.WriteLine(BGColor.r.ToString(format) +
                "," + BGColor.g.ToString(format) +
                "," + BGColor.b.ToString(format));
            saveData_flag = false;

            if (Panel2 != null)
            {
                Panel2.SetActive(true);
            }
        }

        public void saveFirst()
        {
            writer.Flush();
            writer.Close();
        }

        public void SecondRecording()
        {
            Color BGColor = GameObject.Find("BGColor/colorPanel").GetComponent<bgColorHandler>().currentBGColor;
            writer2.WriteLine(BGColor.r.ToString(format) +
                "," + BGColor.g.ToString(format) +
                "," + BGColor.b.ToString(format));
            saveSecond_flag = false; 
            saveSecond();
            ClearRecording(); 
        }

        public void saveSecond() 
        {
            writer2.Flush();
            writer2.Close();
        }

        public void modalityLine()
        {
            modalityIdx = 0;
        }
        public void modalityShape()
        {
            modalityIdx = 1;
        }
        public void modalityColor()
        {
            modalityIdx = 2;
        }

            void Beziercurve(float speed, float val_p, int iter)
            {
                //lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{lines.Count - 1}");

                float arou = 5.0f * (speed + 1.000001f);
                float val = (val_p + 1.000001f);

                //float arou = speed * 5.0f + 5.0f;
                //float val = val_p * 5.0f + 5.0f;

                //float arou = speed * 5;
                for (int j = 0; j < (lines.Count - 1); j += 1)
                {
                    temp = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{j}");
                    comp = temp.GetComponent<LineRenderer>();

                    linePos.Clear();
                    for (int k = 0; k < comp.positionCount; k++)
                    {
                        linePos.Add(LineList[j].points[k]);
                    }

                    /*comp.positionCount = 0;
                    //lineRenderer.positionCount = 0;
                    pC = 0;
                    //lineRenderer.positionCount = 1;
                    comp.positionCount = 1;
                    */
                    float curveWidth;
                    float curveHeight;

                    List<Vector3> total_drawingPoints = new List<Vector3>();
                    int totalCount = 0;

                    int intv = 1;
                    int oe = 1;

                    for (int i = 0; i < linePos.Count - intv; i += intv)
                    {
                        Vector3 p1 = new Vector3(linePos[i].x, linePos[i].y, linePos[i].z);
                        Vector3 p2, p3;

                        curveHeight = Mathf.Abs(linePos[i + intv].y - linePos[i].y);
                        //VRStats.Instance.thirdText.text = $"Song Position: {curveHeight}";
                        //curveHeight = curveHeight * (arou) / 5.0f;
                        curveHeight = curveHeight * (arou);

                        if (oe == 1)
                        {
                            p2 = new Vector3(linePos[i].x + (1.0f - val / 2) * (linePos[i + intv].x - linePos[i].x), linePos[i].y + curveHeight / 5, linePos[i].z);
                            p3 = new Vector3(linePos[i + intv].x - (1.0f - val / 2) * (linePos[i + intv].x - linePos[i].x), linePos[i + intv].y + curveHeight / 5, linePos[i + intv].z);
                            oe = 2;
                        }
                        else
                        {
                            p2 = new Vector3(linePos[i].x + (1.0f - val / 2) * (linePos[i + intv].x - linePos[i].x), linePos[i].y - curveHeight / 5, linePos[i].z);
                            p3 = new Vector3(linePos[i + intv].x - (1.0f - val / 2) * (linePos[i + intv].x - linePos[i].x), linePos[i + intv].y - curveHeight / 5, linePos[i + intv].z);
                            oe = 1;
                        }

                        Vector3 p4 = new Vector3(linePos[i + intv].x, linePos[i + intv].y, linePos[i + intv].z);

                        List<Vector3> drawingPoints;
                        drawingPoints = DrawCubicBezierCurve(p1, p2, p3, p4);

                        for (int k = 0; k < drawingPoints.Count; k++)
                        {
                            total_drawingPoints.Add(drawingPoints[k]);
                        }
                        totalCount = totalCount + drawingPoints.Count;
                    }

                    Vector3[] pointlist = total_drawingPoints.ToArray();
                    //Vector3[] pointlist = linePos.ToArray(); 

                    /*comp.positionCount = pointlist.Length;
                    comp.SetPositions(pointlist);*/
                    //positionCount2 = positionCount2 + pointlist.Length;

                    temp.GetComponent<LineRenderer>().positionCount = 0;
                    temp.GetComponent<LineRenderer>().positionCount = pointlist.Length;
                    temp.GetComponent<LineRenderer>().SetPositions(pointlist);

                    //positionCount = totalCount; 

                }
            }

            void Beziercurve(float speed, float val_p)
            {
                //lineObj = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{lines.Count - 1}");
                Vector3[] newPos;
                float arou = speed * 5.0f + 5.0f;
                float val = val_p * 5.0f + 5.0f;
                //float arou = speed * 5;
                for (int j = 0; j < (lines.Count - 1); j += 1)
                {
                    temp = GameObject.Find($"LineRenderer_{controlHand.ToString()}_{j}");
                    comp = temp.GetComponent<LineRenderer>();

                    linePos.Clear();
                    newPos = new Vector3[comp.positionCount];
                    comp.GetPositions(newPos);

                    for (int k = 0; k < comp.positionCount; k++)
                    {
                        linePos.Add(newPos[k]);
                    }

                    comp.positionCount = 0;
                    //lineRenderer.positionCount = 0;
                    pC = 0;
                    //lineRenderer.positionCount = 1;
                    comp.positionCount = 1;

                    float curveWidth;
                    float curveHeight;
                    Vector3 pos;

                    List<Vector3> total_drawingPoints = new List<Vector3>();
                    int totalCount = 0;

                    for (int i = 0; i < linePos.Count - 1; i += 1)
                    {

                        Vector3 p1 = new Vector3(linePos[i].x, linePos[i].y, linePos[i].z);
                        Vector3 p2, p3;


                        curveHeight = Mathf.Abs(linePos[i + 1].y - linePos[i].y);
                        curveHeight = curveHeight * (arou) / 5.0f;
                        VRStats.Instance.thirdText.text = $"Song Position: {curveHeight}";

                        if (i % 2 == 0)
                        {
                            p2 = new Vector3(linePos[i].x + (1.0f - val / 10.0f) * (linePos[i + 1].x - linePos[i].x), linePos[i].y + curveHeight / 2.0f, linePos[i].z);
                            p3 = new Vector3(linePos[i + 1].x - (1.0f - val / 10.0f) * (linePos[i + 1].x - linePos[i].x), linePos[i + 1].y + curveHeight / 2.0f, linePos[i + 1].z);
                        }
                        else
                        {
                            p2 = new Vector3(linePos[i].x + (1 - val / 10.0f) * (linePos[i + 1].x - linePos[i].x), linePos[i].y - curveHeight / 2.0f, linePos[i].z);
                            p3 = new Vector3(linePos[i + 1].x - (1 - val / 10.0f) * (linePos[i + 1].x - linePos[i].x), linePos[i + 1].y - curveHeight / 2.0f, linePos[i + 1].z);
                        }

                        Vector3 p4 = new Vector3(linePos[i + 1].x, linePos[i + 1].y, linePos[i + 1].z);
                        DrawCubicBezierCurve(p1, p2, p3, p4);
                    }

                    Vector3[] pointlist = total_drawingPoints.ToArray();

                    /*comp.positionCount = pointlist.Length;
                    comp.SetPositions(pointlist);*/
                    //positionCount2 = positionCount2 + pointlist.Length;

                }
            }
        }
}