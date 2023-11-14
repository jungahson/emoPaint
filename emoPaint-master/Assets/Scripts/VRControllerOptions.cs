using System.Collections;
using System.Linq;
using DilmerGames.Core.Singletons;
using DilmerGames.Enums;
using DilmerGames.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Collections.Generic;

[System.Serializable]
public class PrimaryButtonEvent : UnityEvent { }

namespace DilmerGames
{
    public class VRControllerOptions : MonoBehaviour
    {
        //public PrimaryButtonEvent primaryButtonPress;

        private bool lastButtonState = false;
        private bool lastMenuState;
        private Coroutine co_alpha; 

        [SerializeField]
        private XRNode xrNode = XRNode.LeftHand;

        private List<InputDevice> devices = new List<InputDevice>();

        private InputDevice device;

        [SerializeField]
        private TextMeshProUGUI lineWidthSliderLabel;

        [SerializeField]
        private TextMeshProUGUI shapelineWidthSliderLabel;

        [SerializeField]
        private TextMeshProUGUI minLineDistanceSliderLabel;

        [SerializeField]
        private Slider lineWidthSlider;

        [SerializeField]
        private Slider shapelineWidthSlider;

        [SerializeField]
        private Slider minLineDistanceSlider;

        [SerializeField]
        private float lineWidthSliderMultiplier = 0.001f;

        [SerializeField]
        private TextMeshProUGUI colorLabel;

        private Canvas canvas;
        private CanvasGroup canvasGroup;

        [SerializeField]
        private bool isScreenHidden = true;

        [SerializeField]
        private ControlHand controlHand = ControlHand.NoSet;

        [SerializeField]
        private VRControllerSliderChanged OnLineChanged;

        [SerializeField]
        private VRControllerSliderChanged OnShapeLineChanged;

        [SerializeField]
        private VRControllerSliderChanged OnMinDistanceLineChanged;

        [SerializeField, Range(0, 5.0f)]
        private float fadeSpeed = 0.1f; 
        //private float fadeSpeed = 0.3f;

        [SerializeField, Range(0, 5.0f)]
        private float fadeSpeed2 = 0.1f;

        private VRControllerOption[] options;

        private int currentOption = 0;

        public VRControllerOption selectedOption;

        [SerializeField]
        private VRValidColorGradient validColorChanged;

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

        public bool IsScreenHidden
        {
            get 
            {
                return isScreenHidden;
            }
        }
        
        void Start() 
        {
            //primaryButtonPress.AddListener(ToggleScreen);
            canvas = gameObject.GetComponent<Canvas>(); 
            canvasGroup = GetComponent<CanvasGroup>();
            
            lineWidthSlider.onValueChanged.AddListener(LineWidthValueChanged);
            shapelineWidthSlider.onValueChanged.AddListener(ShapeLineWidthValueChanged);
            minLineDistanceSlider.onValueChanged.AddListener(MinDistanceLineWidthValueChanged);
            canvasGroup.alpha = 1.0f;
            options = GetComponentsInChildren<VRControllerOption>()
                .Where(v => v != this).ToArray();
            //selectedOption = options[currentOption];
        }

        void Update()
        {
            bool tempState = false;
            if (!device.isValid)
            {
                GetDevice();
            }

            // left hand
            if (controlHand == ControlHand.Left)
            {
                // toggle screen control
                //if(OVRInput.GetDown(OVRInput.RawButton.X) || Input.GetKeyDown(KeyCode.X))
                /*device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primary);
                if (primary)
                    ToggleScreen();

                // increment slider value
                if((OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight) || Input.GetKey(KeyCode.RightArrow)) && ShoudActivateSlider())
                    selectedOption.GetComponent<Slider>().value += Time.deltaTime * lineWidthSliderMultiplier;

                // drecrement slider value
                if((OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) || Input.GetKey(KeyCode.LeftArrow)) && ShoudActivateSlider())
                    selectedOption.GetComponent<Slider>().value -= Time.deltaTime * lineWidthSliderMultiplier;

                // focus option left direction
                if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) || Input.GetKeyDown(KeyCode.UpArrow))
                    FocusOption(false);

                // focus option right direction
                if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown) || Input.GetKeyDown(KeyCode.DownArrow))
                    FocusOption(true);

                // select option
                if((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.L)) && ShoudActivateColor())
                    SelectOption(true);

                // unselect option
                if((OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyUp(KeyCode.L)))
                    SelectOption(false);
                    */

                
                // toggle screen control
                //if(OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.Z)) 
                /*bool primary; 
                device.TryGetFeatureValue(CommonUsages.primaryButton, out primary);
                tempState = primary;
                if (tempState != lastButtonState) // Button state changed since last frame
                {
                    primaryButtonPress.Invoke();
                    /*if (lastButtonState == true && tempState == false) 
                    {
                        primaryButtonPress.Invoke();
                    }*/

                  //  lastButtonState = tempState;
                    //ToggleScreen();

                    //ScreenHidden = !ScreenHidden; 
                //}

                if (IsActionButtonPressDown())
                {
                    ToggleScreen(); 
                }

                /*if (primary)
                    ToggleScreen();
                */

                // increment slider value
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movementVector);
                /*if ((movementVector.x) >= 0.5f && ShoudActivateSlider())
                {
                    selectedOption.GetComponent<Slider>().value += Time.deltaTime * lineWidthSliderMultiplier;
                }*/ 

                // decrement slider value
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movementVectorL);
                /*if ((movementVector.x) <= -0.5f && ShoudActivateSlider())
                {
                    selectedOption.GetComponent<Slider>().value -= Time.deltaTime * lineWidthSliderMultiplier;
                }*/ 

                // focus option left direction
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movementVectorU);
                /*if ((movementVector.y) >= 0.5f)
                {
                    FocusOption(false);
                }*/ 

                // focus option right direction
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movementVectorD);
                /*if ((movementVector.y) <= -0.5f)
                {
                    FocusOption(true);
                }*/ 

                // select option
                device.TryGetFeatureValue(CommonUsages.indexTouch, out float index );
                /*if (index >= 0.2f && ShoudActivateColor())
                {
                    SelectOption(true);
                }*/ 

                // unselect option
                device.TryGetFeatureValue(CommonUsages.indexTouch, out float indexU );
                /*if (indexU < 0.2f && ShoudActivateColor())
                {
                    SelectOption(false);
                }*/ 
            }
            // right hand
            else if(controlHand == ControlHand.Right)
            {
                // toggle screen control
                //if(OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.Z)) 
                device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primary); 
                /*if (primary) 
                    ToggleScreen();
                */

                // increment slider value
                //if ((OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) || Input.GetKey(KeyCode.RightArrow)) && ShoudActivateSlider())
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movementVector);
                /*if ((movementVector.x) >= 0.5f && ShoudActivateSlider()) 
                {
                    selectedOption.GetComponent<Slider>().value += Time.deltaTime * lineWidthSliderMultiplier;
                }*/ 

                // decrement slider value
                /*if((OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKey(KeyCode.LeftArrow)) && ShoudActivateSlider())
                    selectedOption.GetComponent<Slider>().value -= Time.deltaTime * lineWidthSliderMultiplier;*/ 
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movementVectorL);
                /*if ((movementVector.x) <= -0.5f && ShoudActivateSlider())
                {
                    selectedOption.GetComponent<Slider>().value -= Time.deltaTime * lineWidthSliderMultiplier;
                }*/ 

                // focus option left direction
                /*if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp) || Input.GetKeyDown(KeyCode.UpArrow))
                    FocusOption(false);*/
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movementVectorU);
                /*if ((movementVector.y) >= 0.5f)
                {
                    FocusOption(false);
                }*/ 

                // focus option right direction
                /*if(OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKeyDown(KeyCode.DownArrow))
                    FocusOption(true);*/
                device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movementVectorD);
                /*if ((movementVector.y) <= -0.5f)
                {
                    FocusOption(true);
                }*/ 

                // select option
                /*if ((OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.R)) && ShoudActivateColor())
                SelectOption(true);
                */
                device.TryGetFeatureValue(CommonUsages.indexTouch, out float index );
                /*if (index >= 0.2f && ShoudActivateColor())
                {
                    SelectOption(true);
                }*/ 

                // unselect option
                /*if ((OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyUp(KeyCode.R)))
                    SelectOption(false);
                */
                device.TryGetFeatureValue(CommonUsages.indexTouch, out float indexU );
                /*if (indexU < 0.2f && ShoudActivateColor())
                {
                    SelectOption(false);
                }*/ 
            }
        }

        private bool IsActionButtonPressDown(float pressThreshold = -1.0f)
        {
            IsActionButtonPress(out bool isButtonPress);
            bool wasPressed = lastMenuState == false && isButtonPress;

            lastMenuState = isButtonPress;

            return wasPressed;
        }

        private bool IsActionButtonPress(out bool isPressed, float pressThreshold = -1.0f)
        {
                if (device.isValid == false)
                {
                    return isPressed = false;
                }

                //device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primary);
                if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool value))
                {
                    isPressed = value;
                    return true;
                }
            return isPressed = false;
        }

        //bool ShoudActivateSlider() => selectedOption.VRControllerOptionType == VRControllerOptionType.Slider && !IsScreenHidden;

        //bool ShoudActivateColor() => selectedOption.VRControllerOptionType == VRControllerOptionType.Color && !IsScreenHidden;

        /*void SelectOption(bool active)
        {
            selectedOption = options[currentOption];
            if(selectedOption.IsFocused)
            {
                selectedOption.IsSelected = active;
                if(active)
                {
                    VRDrawColor drawColor = (VRDrawColor)selectedOption;
                    drawColor.ColorSelect();
                }
            }
        }*/ 

        /*void FocusOption(bool rightDirection)
        {
            if(rightDirection)
            {
                if(currentOption >= options.Length - 1)
                    currentOption = 0; 
                else 
                    currentOption++;
            }
            else 
            {
                if(currentOption == 0)
                    currentOption = options.Length - 1; 
                else 
                    currentOption--;
            }

            selectedOption = options[currentOption];

            foreach(VRControllerOption option in options)
            {
                CanvasGroup optionCanvasGroup = option.GetComponent<CanvasGroup>();
                if(optionCanvasGroup == null) continue;

                if(option == selectedOption)
                {
                    optionCanvasGroup.alpha = 1.0f;
                    option.IsFocused = true;  
                    if(option is VRDrawColor)
                        colorLabel.text = ((VRDrawColor)option).ColorName;
                }    
                else
                {
                    option.IsFocused = false;
                    option.IsSelected = false;
                    //optionCanvasGroup.alpha = 0.3f;
                }
            }
        }*/ 

        void LineWidthValueChanged(float newValue)
        {
            lineWidthSliderLabel.text = $"LINE WIDTH: {newValue.ToString("0.000")}";
            OnLineChanged?.Invoke(newValue);
        }
        void ShapeLineWidthValueChanged(float newValue)
        {
            shapelineWidthSliderLabel.text = $"SHAPE LINE WIDTH: {newValue.ToString("0.000")}";
            OnShapeLineChanged?.Invoke(newValue);
        }

        void MinDistanceLineWidthValueChanged(float newValue)
        {
            if(!IsScreenHidden) minLineDistanceSliderLabel.text = $"NEW POINT DISTANCE: {newValue.ToString("0.000")}";
            OnMinDistanceLineChanged?.Invoke(newValue);
        }

        //public void ToggleScreen(bool noFade = false)
        public void ToggleScreen()  
        {
            isScreenHidden = !isScreenHidden;
            /*if (co_alpha != null)
                StopCoroutine(co_alpha);
            if (isScreenHidden)
            {
                co_alpha = StartCoroutine(FadeAlpha(1,0, fadeSpeed));
                //FadeAlpha(1, 0, fadeSpeed);
                //validColorChanged?.Invoke(false);
            }
            else
            {
                co_alpha = StartCoroutine(FadeAlpha(0,1, fadeSpeed2));
                //FadeAlpha(0, 1, fadeSpeed2);
                //validColorChanged?.Invoke(true);
            }*/ 

            canvas.enabled = !canvas.enabled;

            /*if (canvas.enabled)
            {
                Vector3 position = trainee.position + (trainee.forward * appearanceDistance);
                Quaternion rotation = new Quaternion(0.0f, trainee.rotation.y, 0.0f, trainee.rotation.w);
                position.y = trainee.position.y;

                transform.SetPositionAndRotation(position, rotation);
            }
            else
            {
                HideDropdowns();
            }*/ 

            /*if (ScreenHidden)
            {
                //StartCoroutine(FadeAlpha(1,0, fadeSpeed));
                FadeAlpha(0, 1, fadeSpeed);
                ScreenHidden = false; 
                //validColorChanged?.Invoke(false);
            }
            else
            {
                //StartCoroutine(FadeAlpha(0,1, fadeSpeed2));
                FadeAlpha(1, 0, fadeSpeed2);
                ScreenHidden = true;
                //validColorChanged?.Invoke(true);
            }*/

            Resolver.Instance.VRPlayer.SetHaltUpdateMovement(!isScreenHidden);
        }

        
        //private void FadeAlpha(float from, float to, float duration)
        /*private IEnumerator FadeAlpha(float from, float to, float duration)
        {
            float elaspedTime = 0f;

            while (elaspedTime <= duration) 
            {
                canvasGroup.alpha = Mathf.Lerp(from, to, elaspedTime / duration);
                elaspedTime += Time.deltaTime;
                yield return null;
            }  
            canvasGroup.alpha = to;
        }*/ 
    }
}