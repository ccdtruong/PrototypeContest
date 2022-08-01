using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CameraBehavior
{
    Idle, ZoomIn, ZoomOut
}
public class CameraZoom : MonoBehaviour
{
    Vector3 defaultPosition;
    [SerializeField] Transform m_targetTransform;
    private CameraBehavior m_cameraBehavior;
    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 3f;
    private float zoomLerpSpeed = 10;

    public void ZoomIn()
    {
        m_cameraBehavior = CameraBehavior.ZoomIn;
    }

    public void ZoomOut()
    {
        m_cameraBehavior = CameraBehavior.ZoomOut;
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
        defaultPosition = new Vector3(0f, 0f, -10f);
        m_cameraBehavior = CameraBehavior.Idle;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_cameraBehavior == CameraBehavior.Idle) return;
        else if(m_cameraBehavior == CameraBehavior.ZoomIn)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 3f, 0.015f);
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(m_targetTransform.position.x, m_targetTransform.position.y, - 10f), 0.015f);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5f, 0.015f);
            cam.transform.position = Vector3.Lerp(cam.transform.position, defaultPosition, 0.015f);
        }


        //float scrollData = 0.05f;
        //targetZoom -= scrollData * zoomFactor;
        //targetZoom = Mathf.Clamp(targetZoom, 1.5f, 8f);
        //cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);

    }
}
