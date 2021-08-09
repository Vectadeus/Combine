using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class JoyStick : MonoBehaviour
{
    public Transform JoyTransform;
    public Transform JoyBackgroundTransform;
    public float maxDistance;


    public Vector3 MouseCenterPoint;
    public float MouseDistance;
    public float MouseDistanceRatio;







    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JoystickMechanic();
    }




    public void JoystickMechanic()
    {


        if (Input.GetMouseButtonDown(0))
        {
            MouseCenterPoint = Input.mousePosition;
        }

        MouseDistance = Vector2.Distance(MouseCenterPoint, Input.mousePosition);
        MouseDistanceRatio = (MouseDistance / maxDistance);



        if (Input.GetMouseButton(0))
        {

            JoyTransform.localPosition = (Input.mousePosition - MouseCenterPoint);

            if (MouseDistanceRatio >= 1)
            {
                JoyTransform.localPosition = (Input.mousePosition - MouseCenterPoint).normalized * maxDistance;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            JoyTransform.localPosition = Vector2.zero;
        }

    }
}
