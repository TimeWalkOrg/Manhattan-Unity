using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingDescription : MonoBehaviour
{
    private GameObject DescriptionPanel;
    //public GameObject DescriptionPanel;
    private GameObject TimeWalkController;
    private TimeWalkMapViewer MapViewerScript;
    private TextMeshProUGUI DescriptionText;
    private string tempText = "";
    
    private bool DescriptionActive;
    private bool onetime = false;
    // Start is called before the first frame update

    private void Awake() {
        DescriptionPanel = GameObject.FindGameObjectWithTag("BuildingDescriptionPanel");
    }

    void Start()
    {
        var panelAlphaComponent = DescriptionPanel.GetComponent<CanvasRenderer>();
        var text = DescriptionPanel.transform.GetChild(0);
        var textAlphaComponent = text.GetComponent<CanvasRenderer>();
        //DescriptionPanel.SetActive(false);
        panelAlphaComponent.SetAlpha(0f);
        textAlphaComponent.SetAlpha(0f);
        TimeWalkController = GameObject.FindGameObjectWithTag("TimeWalkScript");
        DescriptionText = DescriptionPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        MapViewerScript = TimeWalkController.GetComponent<TimeWalkMapViewer>();
        DescriptionActive = MapViewerScript.isBDActive(); ;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other) {
        //Add one to the number of buildings entered
        MapViewerScript.EnterCollider();

        //Always update the text, even if the description is not active
        var Data = this.gameObject.GetComponent<BuildingData>();
        string YearReplaced = Data.YearReplaced.ToString();
        if (Data.YearReplaced == -1) {
            YearReplaced = "Unknown";
        }
        string GPS = Data.GPSLatitude + ", " + Data.GPSLongitude;
        if (Data.GPSLongitude == 0f && Data.GPSLatitude == 0f) {
            GPS = "Unknown";
        }
        tempText = "Year Built: " + Data.YearBuilt +
            "\nYear Replaced: " + YearReplaced +
            "\nOwner: " + Data.Owner +
            "\nAddress: " + Data.Address +
            "\nGPS: " + GPS +
            "\n" + Data.Description;
        DescriptionText.text = tempText;
        DescriptionActive = TimeWalkController.GetComponent<TimeWalkMapViewer>().isBDActive();

        //If the description is active, then set the alpha to 1f on entering
        if (DescriptionActive && MapViewerScript.GetCollidersEntered() > 0) {
            var panelAlphaComponent = DescriptionPanel.GetComponent<CanvasRenderer>();
            var text = DescriptionPanel.transform.GetChild(0);
            var textAlphaComponent = text.GetComponent<CanvasRenderer>();
            //DescriptionPanel.SetActive(true);
            panelAlphaComponent.SetAlpha(1f);
            textAlphaComponent.SetAlpha(1f);
        }
    }

    private void OnTriggerExit(Collider other) {
        //Subtract one from the number of buildings entered
        MapViewerScript.ExitCollider();

        //Check for the same text, if the desciption is active, and if there are no more entered buildings
        DescriptionActive = TimeWalkController.GetComponent<TimeWalkMapViewer>().isBDActive();
        if (tempText == DescriptionText.text || !DescriptionActive) {
            if(MapViewerScript.GetCollidersEntered() == 0) {
                var panelAlphaComponent = DescriptionPanel.GetComponent<CanvasRenderer>();
                var text = DescriptionPanel.transform.GetChild(0);
                var textAlphaComponent = text.GetComponent<CanvasRenderer>();
                //DescriptionPanel.SetActive(false);
                panelAlphaComponent.SetAlpha(0f);
                textAlphaComponent.SetAlpha(0f);
                DescriptionText.SetText("");
            }
     
        }
            
        //var ObjectColor = this.gameObject.GetComponent<Renderer>().material.color;
        //ObjectColor.g -= 50;
        //this.gameObject.GetComponent<Renderer>().material.color = ObjectColor;
        
    }

}
