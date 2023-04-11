using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPointer : MonoBehaviour{

    public int MaxLength = 100;
    public GameObject player;

    private LineRenderer pointer;

    // Start is called before the first frame update
    void Start()
    {
        pointer = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var StartPoint = UpdateStart();
        var EndPoint = UpdateEnd(StartPoint);

        pointer.SetPosition(0, StartPoint);
        pointer.SetPosition(1, EndPoint);
    }

    private Vector3 UpdateStart() {
        Vector3 StartPoint = this.transform.position;
        return StartPoint;
    }

    private Vector3 UpdateEnd(Vector3 Start) {
        Vector3 EndPoint = (Start + (MaxLength * this.transform.forward));
        return EndPoint;
    }
}
