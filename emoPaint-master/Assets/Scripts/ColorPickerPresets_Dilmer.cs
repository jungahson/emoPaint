///Credit judah4
///Sourced from - http://forum.unity3d.com/threads/color-picker.267043/
using UnityEngine;
using UnityEngine.UI;

namespace DilmerGames
{
	public class ColorPickerPresets_Dilmer : MonoBehaviour
	{
        public Dropdown dropdown;

        //public DilmerGames.ColorPickerControl picker;

        

        /*[SerializeField]
		protected GameObject presetPrefab;
        */ 

        /*[SerializeField]
        protected GameObject createPrefab;
        */
        /*[SerializeField]
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
        */ 
		//protected virtual 
        void Start()
		{
            // Load color palette values..
            /*TextAsset col_pal = Resources.Load<TextAsset>("myrgb3");

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
            /*presetPrefab.SetActive(false);
            //createPrefab.SetActive(false); 
            Debug.Log(float.Parse(colorpalette[0][0]));
            Debug.Log(float.Parse(colorpalette[0][1]));
            Debug.Log(float.Parse(colorpalette[0][2]));
            

            for (int i = 0; i < 20; i++)
            {
                myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                CreatePreset(myRgbColor);
            }

            //VRStats.Instance.firstText.text = $"Emotion";

            //string text = dropdown.options[index].text;

            

            presets.Clear();
            countPrefab = 20;

            for (int i = 0; i < countPrefab; i++)
            {
                GameObject tempObj = GameObject.Find($"GameObject_{i}");
                GameObject.Destroy(tempObj);
            }

            //dropdown.onValueChanged.AddListener(SelectedEmotion);
            //dropdown.onValueChanged.AddListener(delegate { SelectedEmotion(dropdown); });
            //picker.onHSVChanged.AddListener(HSVChanged);
            //picker.onValueChanged.AddListener(ColorChanged);
            picker.CurrentColor = Color.white;
            //presetPrefab.SetActive(false);

            /*presets.AddRange(predefinedPresets);
            LoadPresets(saveMode);*/
        }

        public void SelectedEmotion(int val)  
        {
            VRStats.Instance.firstText.text = $"Emotion";
            //VRStats.Instance.firstText.text = $"Emotion: {val}";
            /*int index = dropdown.value;

            selectedEmo = index;

            presets.Clear();
            countPrefab = 20; 

            for (int i = 0; i < countPrefab; i++)
            {
                GameObject tempObj = GameObject.Find($"GameObject_{i}");
                GameObject.Destroy(tempObj);
            }

            countPrefab = 0; 

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
                /*for (int i = 120; i < 140; i++)
                {
                    myRgbColor = new Color(float.Parse(colorpalette[i][0]), float.Parse(colorpalette[i][1]), float.Parse(colorpalette[i][2]));
                    CreatePreset(myRgbColor);
                }*/

            //}
        }

		/*public virtual void CreatePresetButton()
		{
			CreatePreset(picker.CurrentColor);
		}

		public virtual void CreatePreset(Color color, bool loading)
		{
            //createButton.gameObject.SetActive(presets.Count < maxPresets);
            //GameObject go = new GameObject($"LineRenderer_{controlHand.ToString()}_{lines.Count}");
            
            GameObject newPresetButton = Instantiate(presetPrefab, presetPrefab.transform.parent);
            newPresetButton.name = $"GameObject_{countPrefab}";

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
		}

		/*protected virtual void HSVChanged(float h, float s, float v)
		{
			createPresetImage.color = HSVUtil.ConvertHsvToRgb(h * 360, s, v, 1);
			//Debug.Log("hsv util color: " + createPresetImage.color);
		}

		protected virtual void ColorChanged(Color color)
		{
			createPresetImage.color = color;
			//Debug.Log("color changed: " + color);
		}*/
	}
}