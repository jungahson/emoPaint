using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Diagnostics; 

namespace DilmerGames
{
    public class ColorPickerPre : MonoBehaviour
    {
        public DilmerGames.ColorPickerControl picker;

        public Dropdown dropdown; 

        [SerializeField]
		protected GameObject presetPrefab;
        
        [SerializeField]
		protected int maxPresets = 16;

		[SerializeField]
		protected Color[] predefinedPresets;

		protected List<Color> presets = new List<Color>();
		//public Image createPresetImage;
		//public Transform createButton;

		public enum SaveType { None, PlayerPrefs, JsonFile }
		[SerializeField]
		public SaveType saveMode = SaveType.None;

		[SerializeField]
		protected string playerPrefsKey;

        string[][] colorpalette;
        //private float[,] colorpalette;
        //int[,] array = new int[4, 2];

        int countPrefab = 0;
        int selectedEmo = 0;
        Color myRgbColor;

        public virtual string JsonFilePath
		{
			get { return Application.persistentDataPath + "/" + playerPrefsKey + ".json"; }
		}

		protected virtual void Reset()
		{
			playerPrefsKey = "colorpicker_" + GetInstanceID().ToString();
		}
        
        // Start is called before the first frame update
        void Start()
        {
            // Load color palette values..
            TextAsset col_pal = Resources.Load<TextAsset>("myrgb3");

            string[] rowpal = col_pal.text.Split(new char[] { '\n' });

            string[][] pal_array;
            pal_array = new string[rowpal.Length][];

            for (int i = 0; i < rowpal.Length; i++)
            {
                pal_array[i] = rowpal[i].Split(new char[] { ' ' });
            }

            
            colorpalette = new string[rowpal.Length][]; 

            //var lines = File.ReadAllLines("myrgb3");
            colorpalette = new string[rowpal.Length][]; 

            for (int i = 0; i < rowpal.Length; i++)
            {
                colorpalette[i] = rowpal[i].Split(' '); 
            }

            /*for (int j = 0; j < rowpal.Length; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorpalette[j][i] = float.Parse(fields[j][i]);
                }
            } */
            presetPrefab.SetActive(false);
            //createPrefab.SetActive(false); 
            //Debug.Log(float.Parse(colorpalette[0][0]));
            //Debug.Log(float.Parse(colorpalette[0][1]));
            //Debug.Log(float.Parse(colorpalette[0][2]));
            

            for (int i = 0; i < 20; i++)
            {
                myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                CreatePreset(myRgbColor);
            }

            //VRStats.Instance.firstText.text = $"Emotion";

            //string text = dropdown.options[index].text;

            //dropdown.onValueChanged.AddListener(SelectedEmotion);
            //dropdown.onValueChanged.AddListener(delegate { SelectedEmotion(dropdown); });
            //picker.onHSVChanged.AddListener(HSVChanged);
            //picker.onValueChanged.AddListener(ColorChanged);
            
            picker.CurrentColor = Color.white;
            

            //presetPrefab.SetActive(false);

            /*presets.AddRange(predefinedPresets);
            LoadPresets(saveMode);*/
        }

        // Update is called once per frame
        public void SelectedEmotion(int val)
        {
            string callingFuncName = new StackFrame(1).GetMethod().Name; 
            GameObject vrDraw = GameObject.Find($"VRDrawRight");
            int selected_Emo = vrDraw.GetComponent<VRDraw>().selectedEmo;

                VRStats.Instance.firstText.text = $"Emotion";
                //int index = dropdown.value;

                //GameObject.Find($"DrawLine").SetActive(true);
                //GameObject.Find($"DrawShape").SetActive(true);

                selectedEmo = val;

                //presets.Clear();

                //countPrefab = 20; 

                if (gameObject.name == "Presets")
                {
                    for (int i = 0; i < countPrefab; i++)
                    {
                        GameObject tempObj = GameObject.Find($"Presets/{i}");
                        if (selectedEmo <= 5)
                        {
                            int coloridx = selectedEmo * 20 + i;
                            tempObj.GetComponent<Image>().color = new Color(float.Parse(colorpalette[coloridx][0]), float.Parse(colorpalette[coloridx][1]), float.Parse(colorpalette[coloridx][2]));
                        }
                        else
                        {
                            int s_Emo = dropdown.value;
                            int coloridx = s_Emo * 20 + i;
                            tempObj.GetComponent<Image>().color = new Color(float.Parse(colorpalette[coloridx][0]), float.Parse(colorpalette[coloridx][1]), float.Parse(colorpalette[coloridx][2]));
                        }
                        //GameObject.Destroy(tempObj);
                        //tempObj.GetComponent<Image>().SetAllDirty();
                    }
                }
                else if (gameObject.name == "SPresets")
                {
                    for (int i = 0; i < countPrefab; i++)
                    {
                        GameObject tempObj = GameObject.Find($"SPresets/{i}");
                        if (selectedEmo <= 5)
                        {
                            int coloridx = selectedEmo * 20 + i;
                            tempObj.GetComponent<Image>().color = new Color(float.Parse(colorpalette[coloridx][0]), float.Parse(colorpalette[coloridx][1]), float.Parse(colorpalette[coloridx][2]));
                        }
                        else
                        {
                            int s_Emo = dropdown.value;
                            int coloridx = s_Emo * 20 + i;
                            tempObj.GetComponent<Image>().color = new Color(float.Parse(colorpalette[coloridx][0]), float.Parse(colorpalette[coloridx][1]), float.Parse(colorpalette[coloridx][2]));
                        }
                        //tempObj.GetComponent<Image>().SetAllDirty();
                        //GameObject.Destroy(tempObj);
                    }
                }
                else if (gameObject.name == "BGPresets")
                {
                    for (int i = 0; i < countPrefab; i++)
                    {
                        GameObject tempObj = GameObject.Find($"BGPresets/{i}");
                        if (selectedEmo <= 5)
                        {
                            int coloridx = selectedEmo * 20 + i;
                            tempObj.GetComponent<Image>().color = new Color(float.Parse(colorpalette[coloridx][0]), float.Parse(colorpalette[coloridx][1]), float.Parse(colorpalette[coloridx][2]));
                        }
                        else
                        {
                            int s_Emo = dropdown.value;
                            int coloridx = s_Emo * 20 + i;
                            tempObj.GetComponent<Image>().color = new Color(float.Parse(colorpalette[coloridx][0]), float.Parse(colorpalette[coloridx][1]), float.Parse(colorpalette[coloridx][2]));
                        }
                        //tempObj.GetComponent<Image>().SetAllDirty();
                        //GameObject.Destroy(tempObj);
                    }
                }

                /*countPrefab = 0; 

                if (selectedEmo == 0)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                        CreatePreset(myRgbColor);
                    }
                }
                else if (selectedEmo == 1)
                {
                    for (int i = 20; i < 40; i++)
                    {
                        myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                        CreatePreset(myRgbColor);
                    }
                }
                else if (selectedEmo == 2)
                {
                    for (int i = 40; i < 60; i++)
                    {
                        myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                        CreatePreset(myRgbColor);
                    }
                }
                else if (selectedEmo == 3)
                {
                    for (int i = 60; i < 80; i++)
                    {
                        myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                        CreatePreset(myRgbColor);
                    }
                }
                else if (selectedEmo == 4)
                {
                    for (int i = 80; i < 100; i++)
                    {
                        myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                        CreatePreset(myRgbColor);
                    }
                }
                else if (selectedEmo == 5)
                {
                    for (int i = 100; i < 120; i++)
                    {
                        myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                        CreatePreset(myRgbColor);
                    }
                }
                else
                {
                    int s_Emo= dropdown.value;
                    int idxcnt = s_Emo * 20; 

                    for (int i = idxcnt; i < (idxcnt + 20); i++)
                    {
                        myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                        CreatePreset(myRgbColor);
                    }

                }*/

                Canvas.ForceUpdateCanvases();
            
        }

        public virtual void CreatePreset(Color color, bool loading)
        {
            //createButton.gameObject.SetActive(presets.Count < maxPresets);
            //GameObject go = new GameObject($"LineRenderer_{controlHand.ToString()}_{lines.Count}");

            GameObject newPresetButton = Instantiate(presetPrefab, presetPrefab.transform.parent);
            //newPresetButton.name = $"GameObject_{countPrefab}";
            newPresetButton.name = $"{countPrefab}";
            countPrefab = countPrefab + 1;

            newPresetButton.transform.SetAsLastSibling();
            newPresetButton.SetActive(true);
            newPresetButton.GetComponent<Image>().color = color;

            //createPresetImage.color = Color.white;

            if (!loading)
            {
                presets.Add(color);
                //SavePresets(saveMode);
            }
        }
        public virtual void CreatePreset(Color color)
        {
            CreatePreset(color, false);
        }
        public virtual void PresetSelect(Image sender)
        { 
            picker.CurrentColor = sender.color;
            //onValueChanged.Invoke(sender.color); 
        }
    }

}