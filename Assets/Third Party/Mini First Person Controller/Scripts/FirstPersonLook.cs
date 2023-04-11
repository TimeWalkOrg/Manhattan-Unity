using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public bool Desktop = false;
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    public bool paused = false;

    Vector2 velocity;
    Vector2 frameVelocity;

    private float lookSpeed = 3f;
    

    void Reset()
    {
        
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
        // Get the character from the FirstPersonMovement in parents.
        if (Desktop) {
            character = GetComponentInParent<FirstPersonMovement>().transform;
        }
        else {
            character = GameObject.FindGameObjectWithTag("OVRPlayerController").transform;
        }
    }

    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);
        var desiredRotation = Quaternion.Euler(-velocity.y, velocity.x, 0);


        // Rotate camera up-down and controller left-right from velocity.
        //transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        //character.transform.rotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        if (!paused) {
            transform.rotation = desiredRotation;
        }
        
    }
}
