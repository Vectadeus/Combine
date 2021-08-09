using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //MOVING STUFF


    [SerializeField] private float speed;
    [SerializeField] private float RotSpeed;
    [SerializeField] private JoyStick joyStick;
    public bool IsMoving;
    private Rigidbody rb;

    //WHEEL ROTATION STUFF
    public Transform[] Wheels = new Transform[3];
    public float WheelRotSpeed;


    //EAT STUFF
    public Transform EatBoxCenter;
    public Vector3 EatBoxSize;
    public LayerMask EatBoxMask;

    private void OnEnable()
    {
        joyStick = FindObjectOfType<JoyStick>();
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        StartCoroutine(CheckMovement());
    }


    void Update()
    {
        CheckMovement();
        LookDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }


    void Move()
    {
        //Moving
        float MovementX = joyStick.JoyTransform.localPosition.x;
        float MovementZ = joyStick.JoyTransform.localPosition.y;

        rb.velocity = new Vector3(MovementX, rb.velocity.y, MovementZ).normalized * speed * Time.deltaTime;

    
    }
    
    void LookDirection()
    {
        float MovementX = joyStick.JoyTransform.localPosition.x;
        float MovementZ = joyStick.JoyTransform.localPosition.y;
        Vector3 _MoveDirection = new Vector3(MovementX, 0f, MovementZ);
        _MoveDirection.Normalize();

        if (_MoveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, _MoveDirection, RotSpeed);


        }
        //Wheel rotation

        if (IsMoving)
        {
            foreach (Transform wheel in Wheels)
            {
                wheel.Rotate(Vector3.right, WheelRotSpeed * Time.deltaTime);
            }
        }
    }

    IEnumerator CheckMovement()
    {
        Vector3 pos1 = transform.position;
        yield return new WaitForSeconds(0.05f);
        Vector3 pos2 = transform.position;

        if(pos1.x == pos2.x && pos1.z == pos2.z)
        {
            IsMoving = false;
        }
        else
        {
            IsMoving = true;
        }
        StartCoroutine(CheckMovement());
    }

}//CLASS
