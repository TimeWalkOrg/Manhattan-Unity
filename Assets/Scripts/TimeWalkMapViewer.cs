using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeWalkMapViewer : MonoBehaviour
{
    public GameObject TerrainObject;
    public GameObject MapsObject;
    public GameObject PaintingObject;
    public GameObject BuildingsObject;
    //public GameObject ARSessionOriginObject;
    public GameObject HUDPanel;
    public GameObject HUDButtons;
    public GameObject HelpTextPanel;
    public GameObject ControlPanel;
    public GameObject DescriptionPanel;
    public GameObject TimeSliderOverlay;
    public GameObject GroundPlane; // not needed for AR
    //public GameObject PlayerAR; // AR player controller objects (disabled if not AR)
    public GameObject PlayerDesktop; // PC or Mac player controller objects
    public TextMeshProUGUI mapTextDisplay;
    public TextMeshProUGUI helpTextDisplay;
    public TextMeshProUGUI MapTerrainButtonText;
    //public Slider scaleSlider; // The slider this script is attached to

    public List<GameObject> _myMaps = new List<GameObject>();
    private int mapCount;
    private int mapIndex;
    private int collidersEntered = 0;
    private bool terrainIsVisible;
    private bool mapsAreVisible;
    private bool buildingsAreVisible;
    private bool paintingIsVisible;
    private bool controlPanelIsVisible;
    private bool helpIsVisible;
    private bool groundPlaneIsVisible;
    private bool buildingDescriptionActive = true;
    private string helpText = "<b>Keypress Controls:</b><color=blue>" +
        "\n  H = Help" +
        "\n  T = Terrain/Map" +
        "\n  P = Painting" +
        "\n  N = Next Map" +
        "\n  Y = Year Slider" //+
        //"\n  B = Building Description</color>"
        ;
    //public float arStartingScale;
    //public float desktopStartingScale;
    public bool runningInEditor = false; //  set to false if running as an app (e.g. iOS build or remote AR testing)

    public void Awake()
    {
        //scaleSlider.onValueChanged.AddListener(delegate { OnSliderWasChanged(); });
        //OnSliderWasChanged();

    }

    void Start()
    {
        //runningInEditor = Application.isEditor; // use this if you're not running AR remote (which needs to think it's running on iOS, not the Editor)
        runningInEditor = true; // temporary: Add back AR functions later
        mapCount = MapsObject.transform.childCount;
        for (int i = 0; i < mapCount; i++)
        {
            Transform nextMap = MapsObject.transform.GetChild(i);
            _myMaps.Add(nextMap.gameObject);
            nextMap.gameObject.SetActive(false);
        }


        if (runningInEditor)
        {
            Debug.Log("Running in Unity Editor");
            terrainIsVisible = true;
            mapsAreVisible = false;
            buildingsAreVisible = true;
            paintingIsVisible = false;
            controlPanelIsVisible = false;
            helpIsVisible = false;
            groundPlaneIsVisible = true;
            //PlayerAR.gameObject.SetActive(false);
            PlayerDesktop.gameObject.SetActive(true);

        }
        else
        {
            Debug.Log("Running on iOS");
            terrainIsVisible = false;
            mapsAreVisible = true;
            buildingsAreVisible = false;
            paintingIsVisible = false;
            controlPanelIsVisible = false;
            helpIsVisible = false;
            HUDButtons.gameObject.SetActive(true);
            groundPlaneIsVisible = false;
            PlayerDesktop.gameObject.SetActive(false);
            //PlayerAR.gameObject.SetActive(true);
        }

        mapIndex = 0; // set to first map in list
        _myMaps[mapIndex].gameObject.SetActive(true);
        ClearMapName();
        TerrainObject.gameObject.SetActive(terrainIsVisible);
        MapsObject.gameObject.SetActive(mapsAreVisible);
        PaintingObject.gameObject.SetActive(paintingIsVisible);
        BuildingsObject.gameObject.SetActive(buildingsAreVisible);
        HelpTextPanel.gameObject.SetActive(helpIsVisible);
        helpTextDisplay.text = helpText;
        HUDPanel.gameObject.SetActive(true);
        ControlPanel.gameObject.SetActive(controlPanelIsVisible);
        GroundPlane.gameObject.SetActive(groundPlaneIsVisible);

        //ARSessionOriginObject.transform.localScale = new Vector3(arStartingScale, arStartingScale, arStartingScale);
    }

    void Update()
    {
        if (runningInEditor)
        {
            if (Input.GetKeyDown("t")) // toggle Terrain
            {
                MapTerrainButtonPressed();
            }
            if (Input.GetKeyDown("p")) // toggle Painting
            {
                paintingIsVisible = !paintingIsVisible;
                PaintingObject.gameObject.SetActive(paintingIsVisible);
            }
            if (Input.GetKeyDown("h")) // toggle Help
            {
                helpIsVisible = !helpIsVisible;
                HelpTextPanel.gameObject.SetActive(helpIsVisible);

                //controlPanelIsVisible = !controlPanelIsVisible;
                //ControlPanel.gameObject.SetActive(controlPanelIsVisible);
            }
            if (Input.GetKeyDown("n"))
            {
                mapsAreVisible = false;
                //ToggleMapVisibility(); // will set map visibility to true
                ShowNextMap(true);
            }
            if (Input.GetKeyDown("b")) // toggle Building Description
            {
                buildingDescriptionActive = !buildingDescriptionActive;
                //DescriptionPanel.SetActive(DescriptionActive);
                var panelAlphaComponent = DescriptionPanel.GetComponent<CanvasRenderer>();
                var text = DescriptionPanel.transform.GetChild(0);
                var textAlphaComponent = text.GetComponent<CanvasRenderer>();
                if (!buildingDescriptionActive) {
                    panelAlphaComponent.SetAlpha(0f);
                    textAlphaComponent.SetAlpha(0f);
                }
                else if(collidersEntered > 0){
                    panelAlphaComponent.SetAlpha(1f);
                    textAlphaComponent.SetAlpha(1f);
                }
            }

        }

    }

    public void SetMapName()
    {
        string newMapName = _myMaps[mapIndex].gameObject.name;
        string newYear = newMapName.Substring(0, 4);
        string mapNameText = newMapName.Substring(7, newMapName.Length - 7);
        mapTextDisplay.text = mapNameText + "\n<color=yellow>" + newYear + "</color>";
    }


    public void ShowNextMap(bool isNext)
    {
        _myMaps[mapIndex].gameObject.SetActive(false);
        if (isNext)
        {
            mapIndex++;
            if (mapIndex == mapCount) mapIndex = 0;
        } else
        {
            mapIndex--;
            if (mapIndex < 0) mapIndex = mapCount-1;
        }
        _myMaps[mapIndex].gameObject.SetActive(true);
        SetMapName();
        mapsAreVisible = true; // show maps if cycling through them!
        MapsObject.gameObject.SetActive(mapsAreVisible);


    }
    public void ToggleControlPanel()
    {
        controlPanelIsVisible = !controlPanelIsVisible;
        ControlPanel.gameObject.SetActive(controlPanelIsVisible);
    }


    public void MapTerrainButtonPressed()
    {
        if (mapsAreVisible) // enable Terrain view, hide Maps
        {
            MapsObject.gameObject.SetActive(false);
            mapTextDisplay.gameObject.SetActive(false);
            mapTextDisplay.text = "";
            TimeSliderOverlay.gameObject.SetActive(true);
            TerrainObject.gameObject.SetActive(true);
            BuildingsObject.gameObject.SetActive(true);
            mapsAreVisible = false;
            MapTerrainButtonText.text = "Terrain";
        }
        else // enable Maps view, hide Terrain
        {
            MapsObject.gameObject.SetActive(true);
            mapTextDisplay.gameObject.SetActive(true);
            SetMapName();
            TimeSliderOverlay.gameObject.SetActive(false);
            TerrainObject.gameObject.SetActive(false);
            BuildingsObject.gameObject.SetActive(false);
            mapsAreVisible = true;
            MapTerrainButtonText.text = "Maps";
        }
    }

    //public void OnSliderWasChanged()
    //{
    //    ScalePlayer();
    //}

    //public void ScalePlayer()
    //{
    //    if (runningInEditor)
    //    {
    //        float tempScaleDesktop = scaleSlider.value * desktopStartingScale;
    //        PlayerDesktop.transform.localScale = new Vector3(tempScaleDesktop, tempScaleDesktop, tempScaleDesktop);

    //    }
    //    else
    //    {
    //        //float tempScale = scaleSlider.value * arStartingScale;
    //        float tempScale = scaleSlider.value; // testing direct setting of scale
    //        ARSessionOriginObject.transform.localScale = new Vector3(arStartingScale, tempScale, arStartingScale); // only scale Y
    //    }
    //}
    public void ClearMapName()
    {
        mapTextDisplay.text = "";

    }

    //Building description functions
    public bool isBDActive() {
        return buildingDescriptionActive;
    }

    //Returns the number of times the player has entered a box collider around a building
    public int GetCollidersEntered() {
        return collidersEntered;
    }

    //Adds one to the number of of colliders entered
    public void EnterCollider() {
        collidersEntered += 1;
    }

    //Subtracts one to the number of of colliders entered
    public void ExitCollider() {
        collidersEntered -= 1;
    }
}
