using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MaoScrip : MonoBehaviour
{
    public Transform cam;
    public Transform maoObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        maoObj.transform.position = Vector3.Lerp(maoObj.transform.position, cam.transform.position, 0.7f);
        maoObj.rotation = Quaternion.Lerp(maoObj.transform.rotation, cam.transform.rotation, 0.3f);
    }
}
