using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadData : MonoBehaviour
{

    //頭部モデルの中心線
    public Vector3[] HeadMainV = new Vector3[] {
    new Vector3(0.0f, 0.01f, 0.0f),//ゼロ除算しないように
    new Vector3(1.0f, 2.0f, 0.0f),
    new Vector3(2.5f, 1.5f, 0.0f),
    new Vector3(4.0f, 1.0f, 0.0f),
    new Vector3(5.5f, 0.5f, 0.0f)
    };

// Start is called before the first frame update
void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

