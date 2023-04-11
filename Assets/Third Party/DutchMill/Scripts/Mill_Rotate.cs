using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill_Rotate : MonoBehaviour
{


    [SerializeField] float speed = 30f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate( 0,0,-1f * speed * Time.deltaTime, Space.Self);

            


    }
}
