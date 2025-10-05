using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float Sensibilidade = 100f;

    public bool cutscene = false;

    public Transform player;
    private PlayerMove playScrip;

    float Xrotate = 0f;

    void Start()
    {
        playScrip = player.GetComponent<PlayerMove>();
    }
    
    void Update()
    {
        if(!playScrip.naCam)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }else{
            Cursor.lockState = CursorLockMode.None;
        }
        
        if(!cutscene)
        {
            float mouseX = Input.GetAxis("Mouse X") * Sensibilidade;
            float mouseY = Input.GetAxis("Mouse Y") * Sensibilidade;

            Xrotate -= mouseY;
            Xrotate = Mathf.Clamp(Xrotate, -90f, 90);

            transform.localRotation = Quaternion.Euler(Xrotate, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }
}
