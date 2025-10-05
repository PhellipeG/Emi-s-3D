using UnityEngine;

public class MaoScrip : MonoBehaviour
{
    public Transform cam;
    public Transform maoObj;

    void Update()
    {
        maoObj.transform.position = Vector3.Lerp(maoObj.transform.position, cam.transform.position, 0.7f);
        maoObj.rotation = Quaternion.Lerp(maoObj.transform.rotation, cam.transform.rotation, 0.3f);
    }
}
