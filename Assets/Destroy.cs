using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ObjectDestroy", time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ObjectDestroy()
    {
        Destroy(gameObject);
    }
}
