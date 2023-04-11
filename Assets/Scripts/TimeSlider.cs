using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlider : MonoBehaviour
{
    public List<GameObject> ObjectParents;
    public List<GameObject> Buildings;
    public Slider TSlider;
    public TextMeshProUGUI TSliderText;


    //Variables for when the game is paused
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public Camera FPSCamera;
    public GameObject FPSBody;

    private FirstPersonLook CamScript;
    private Image OverlayPanel;
    private RigidbodyConstraints FPSRigidCon;
    private RigidbodyConstraints FPSRigidConOriginal;
    private GameObject OVRPlayerController;



    // Start is called before the first frame update
    void Start()
    {
        //Create a list of all the buildings to enable/disable when the slider changes
        createList();
        //Checks once at the start of the game, and again only when the slider is changed
        ChangeTime();
        TSlider.onValueChanged.AddListener(delegate { ChangeTime(); });

        CamScript = FPSCamera.GetComponentInChildren<FirstPersonLook>();
        var PanelParent = GameObject.FindGameObjectWithTag("Panel");
        OverlayPanel = PanelParent.GetComponentInChildren<Image>();
        FPSRigidCon = FPSBody.GetComponentInChildren<Rigidbody>().constraints;
        FPSRigidConOriginal = FPSRigidCon;
        OVRPlayerController = GameObject.FindGameObjectWithTag("OVRPlayerController");
    }

    // Update is called once per frame
    void Update()
    {
        //Pause the game such that the camera and player do not move when changing the year
        if (Input.GetKeyDown(KeyCode.Y)) {
            if (GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    //Checks through each object in the list of buildings.
    //If they exist in the year of the current slider's value they are set to active,
    //otherwise they are set to inactive
    public void ChangeTime() {
        foreach(GameObject Building in Buildings){
            var Data = Building.GetComponent<BuildingData>();
            //if the YearReplaced is set to '-1' then the building is thought of as "Currently Existing"
            //so we place the YearReplaced value as 1 more than the slider's maximum
            var tempYearReplaced = Data.YearReplaced;
            if(tempYearReplaced == -1) {
                tempYearReplaced = (int)TSlider.maxValue +1;
            }
            if (TSlider.value <= tempYearReplaced && TSlider.value >= Data.YearBuilt) {
                if(Building.tag == "BuildingDataChild") {
                    Building.transform.parent.gameObject.SetActive(true);
                }
                else
                    Building.SetActive(true);
            }
            else {
                if (Building.tag == "BuildingDataChild") {
                    Building.transform.parent.gameObject.SetActive(false);
                }
                else
                    Building.SetActive(false);
            }   
        }
        TSliderText.SetText("The Current Year Is: " + TSlider.value);
    }

    void createList() { //Create a list of all the buildings to enable/disable when the slider changes
        foreach(GameObject parent in ObjectParents){
            foreach(Transform child in parent.transform) {
                //Attempt to find the BuildingData Script on each object
                //If it fails, don't add the object to the list
                var Data = child.GetComponent<BuildingData>();

                //The modern day NY buildings have their building data script on a child,
                //(in order to rotate their box colliders)
                //so I'm checking through their children as well. 
                if (Data == null) {
                    var childData = child.GetComponentInChildren<BuildingData>();
                    if (childData != null) {
                        Buildings.Add(childData.gameObject);
                    }
                    continue;
                }
                Buildings.Add(child.gameObject);
            }
        }
    }

    void Resume() {
        //Turn off the pause menu
        pauseMenuUI.SetActive(false);
        var tempColor = OverlayPanel.color;
        tempColor.a = 0f;
        OverlayPanel.color = tempColor;

        //Set the game to resumed
        GameIsPaused = false;

        //Turn the cursor off and lock it to the game creen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Have the camera follow mouse movement
        CamScript.enabled = true;

        //Resume Y rotation for the player's rigidbody
        FPSRigidCon = FPSRigidConOriginal;
        FPSBody.GetComponent<Rigidbody>().constraints = FPSRigidCon;

        //set the First Person Look script's paused to false
        OVRPlayerController.GetComponent<FirstPersonLook>().paused = false;
    }

    void Pause() {
        //Turn on the pause menu
        pauseMenuUI.SetActive(true);
        var tempColor = OverlayPanel.color;
        tempColor.a = .5f;
        OverlayPanel.color = tempColor;

        //Set the game to paused
        GameIsPaused = true;

        //Set the curso to on and unlock it for menu buttons
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Don't have the camera follow the mouse movement
        CamScript.enabled = false;

        //Disable all roation of the player's rigidbody
        FPSRigidCon = RigidbodyConstraints.FreezeRotation;
        FPSBody.GetComponent<Rigidbody>().constraints = FPSRigidCon;

        //set the First Person Look script's paused to true
        OVRPlayerController.GetComponent<FirstPersonLook>().paused = true;
    }

}
