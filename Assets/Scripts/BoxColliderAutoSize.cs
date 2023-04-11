using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderAutoSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var childWithMesh = findChildWithMesh();
        if (childWithMesh == null) {
            Debug.LogError("BoxColliderAutoSize: This object has no mesh to create a box collider from --> " + this.name);
        }
        else {
            BoxCollider boxCollider = childWithMesh.AddComponent<BoxCollider>();
            var renderer = childWithMesh.GetComponent<MeshRenderer>();
            boxCollider.center = renderer.bounds.center;
            boxCollider.center = renderer.bounds.size * 1.5f;
            boxCollider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject findChildWithMesh() {
        GameObject foundChild = null;
        foreach(Transform child in transform){
            var mesh = child.GetComponent<MeshRenderer>();
            if(mesh != null) {
                foundChild = child.gameObject;
                break;
            }
        }
        return foundChild;
    }
}
