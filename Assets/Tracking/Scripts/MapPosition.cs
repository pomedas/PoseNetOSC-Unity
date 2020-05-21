using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPosition : MonoBehaviour
{
    public GameObject TargetObject1;
    public GameObject TargetObject2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = (TargetObject1.transform.position.x+ TargetObject2.transform.position.x) / 2.0f;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
