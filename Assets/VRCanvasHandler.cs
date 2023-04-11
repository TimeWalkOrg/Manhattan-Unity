using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VRCanvasHandler : MonoBehaviour
{
    public bool Desktop = false;
    public bool VR = false;
    public GameObject Player;
    public float UIDistance;
    public Canvas UICanvas;
    public GameObject Pointer;
    public Camera DesktopCamera;
    public GameObject OculusInputModule;


    private GameObject TimeSlider;
    private Vector3 positionDifference;
    private string DesktopHelpText;
    private string VRHelpText;
    // Start is called before the first frame update
    void Start()
    {
        TimeSlider = GameObject.FindGameObjectWithTag("TimeSlider");
        //set help texts
        DesktopHelpText = "<b>Keypress Controls:</b><color=blue>\n  H = Help\n  T = Terrain/Map\n  P = Painting\n  N = Next Map\n  Y = Year Slider\n </color>";
        VRHelpText = "Use the Left Stick to Move" +
            "\nUse the Right Stick to Snap Turn" +
            "\nUse the Right triggers or buttons to change the year on the slider" +
            "\nUse the Left buttons to hide or show the UI";   

        //check if the user has manually selected any device types. if so, change the UI to that type
        //otherwise, check if the application is running on desktop
        if (Desktop && VR) {
            Debug.LogError("Error: Multiple platforms chosen. Please select only one option on 'VRCanvasHandler'");
        }
        if(Desktop && !VR) {
            SwitchToDesktopUI();
        }
        else if(!Desktop && VR) {
            SwitchToVRUI();
        }
        else {
            if(SystemInfo.deviceType == DeviceType.Desktop) {
                VR = false;
                Desktop = true;
                SwitchToDesktopUI();
            }
            else {
                VR = true;
                Desktop = false;
                SwitchToVRUI();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //OVRInput.Update();

        if (VR) {
            SetUIPosition();
        }
        if(OVRInput.GetDown(OVRInput.RawButton.Y) || OVRInput.GetDown(OVRInput.RawButton.X)) {
            HideCanvas();
        }
    }

    private void SetUIPosition() {
        //if the player is not null, then update the canvas to move with the player for better use in VR
        if (Player != null) {
            positionDifference = UIDistance * Player.transform.forward;
            UICanvas.transform.position = Player.transform.position + positionDifference;
            UICanvas.transform.rotation = Player.transform.rotation;
        }
    }

    private void HideCanvas() {
        if(UICanvas != null) {
            if(UICanvas.GetComponent<CanvasGroup>().alpha == 0) {
                UICanvas.GetComponent<CanvasGroup>().alpha = 1;
            }
            else {
                UICanvas.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    private void SwitchToDesktopUI() {
        //turn on First Person Look 
        Player.GetComponent<FirstPersonLook>().enabled = true;

        //turn off OVRPlayerController
        Player.GetComponent<OVRPlayerController>().enabled = false;

        //turn on TeleportLocomotion
        Player.GetComponent<TeleportLocomotion>().enabled = true;

        //turn off the VR hand pointer
        Pointer.SetActive(false);

        //desktop help text
        var HelpTextPanel = UICanvas.transform.Find("Help Text Panel");
        var HelpTextTMP = HelpTextPanel.transform.Find("Help Text (TMP)");
        HelpTextTMP.GetComponent<TextMeshProUGUI>().SetText(DesktopHelpText);

        //change UI scale and render mode
        UICanvas.transform.localScale = new Vector3(1f, 1f, 1f);
        UICanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        //change the Event Camera
        UICanvas.GetComponent<Canvas>().worldCamera = DesktopCamera;

        //turn off TimeSlider Slider
        TimeSlider.SetActive(false);

        //turn off OculusInputModule
        OculusInputModule.SetActive(false);
    }

    private void SwitchToVRUI() {
        //turn off First Person Look 
        Player.GetComponent<FirstPersonLook>().enabled = false;

        //turn on OVRPlayerController
        Player.GetComponent<OVRPlayerController>().enabled = true;

        //turn of TeleportLocomotion
        Player.GetComponent<TeleportLocomotion>().enabled = false;

        //turn on the VR hand pointer
        Pointer.SetActive(true);

        //VR help text
        var HelpTextPanel = UICanvas.transform.Find("Help Text Panel");
        var HelpTextTMP = HelpTextPanel.transform.Find("Help Text (TMP)");
        HelpTextTMP.GetComponent<TextMeshProUGUI>().SetText(VRHelpText);

        //change UI scale and render mode
        UICanvas.transform.localScale = new Vector3(.005f, .005f, .005f);
        UICanvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        var RectTransform = UICanvas.GetComponent<RectTransform>();
        RectTransform.sizeDelta = new Vector2(900f, RectTransform.sizeDelta.y);

        //change the Event Camera
        UICanvas.GetComponent<Canvas>().worldCamera = Pointer.GetComponent<Camera>();

        //turn on TimeSlider Slider
        TimeSlider.SetActive(true);

        //turn on OculusInputModule
        OculusInputModule.SetActive(true);
    }
}
