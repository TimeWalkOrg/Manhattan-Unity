using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;
    
    public GameObject pauseMenuUI;
    public Camera FPSCamera;
    public GameObject FPSBody;

    private FirstPersonLook CamScript;
    private Image OverlayPanel;
    private RigidbodyConstraints FPSRigidCon;
    private RigidbodyConstraints FPSRigidConOriginal;

    // Start is called before the first frame update
    void Start()
    {
        CamScript = FPSCamera.GetComponentInChildren<FirstPersonLook>();
        OverlayPanel = this.GetComponentInChildren<Image>();
        FPSRigidCon = FPSBody.GetComponentInChildren<Rigidbody>().constraints;
        FPSRigidConOriginal = FPSRigidCon;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y)){
            if(GameIsPaused){
                Resume();
            }
            else{
                Pause();
            }

        }
    }

    void Resume(){
        //Turn off the pause menu
        pauseMenuUI.SetActive(false);
        var tempColor = OverlayPanel.color;
        tempColor.a = 0f;
        OverlayPanel.color = tempColor;

        //Set the Game to resumed
        GameIsPaused = false;

        //Turn the cursor off and lock it to the game creen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        //Have the camera follow mouse movement
        CamScript.enabled = true;    

        //Resume Y rotation for the player's rigidbody
        FPSRigidCon = FPSRigidConOriginal;
        FPSBody.GetComponent<Rigidbody>().constraints = FPSRigidCon;
    }

    void Pause(){
        //Turn on the pause menu
        pauseMenuUI.SetActive(true);
        var tempColor = OverlayPanel.color;
        tempColor.a = .5f;
        OverlayPanel.color = tempColor;

        //Set the Game to paused
        GameIsPaused = true;

        //Set the curson on and unlock it for menu buttons
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        //Don't have the camera follow the mouse movement
        CamScript.enabled = false;
        
        //Disable all roation of the player's rigidbody
        FPSRigidCon = RigidbodyConstraints.FreezeRotation;
        FPSBody.GetComponent<Rigidbody>().constraints = FPSRigidCon;
    }

}
