using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRock : MonoBehaviour
{

    private Quaternion startRotation;
    public int loopDuration;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float f = Mathf.PingPong(Time.time, loopDuration) - loopDuration/2;
        transform.rotation = startRotation * Quaternion.AngleAxis(f, Vector3.forward);
    }
}

