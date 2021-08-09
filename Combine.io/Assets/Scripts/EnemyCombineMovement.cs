using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombineMovement : MonoBehaviour
{

    [SerializeField] private float CombineSpeed;
    [SerializeField] private float CombineRotSpeed;
    [SerializeField] private float RayLength;
    [SerializeField] private LayerMask ObstacleMask;
    [SerializeField] private Transform RayTransform;

    private Vector3 RandomDirection;
    private Rigidbody rb;




    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeDirection());
        
    }

    // Update is called once per frame
    void Update()
    {
        AvoidObstacles();
    }

    private void FixedUpdate()
    {
        CombineMove();
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(2, 4));

        RandomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        RandomDirection.Normalize();
        StartCoroutine(ChangeDirection());
    }

    void AvoidObstacles()
    {

        RaycastHit ray;

        if(Physics.Raycast(RayTransform.position , transform.forward , out ray , RayLength))
        {
            //14TH LAYER IS OBSTACLE LAYER
            if(ray.collider.gameObject.layer == 14)
            {
                int GoRandom;
                GoRandom = Random.Range(0, 1);

                if(GoRandom == 0)
                {
                    Vector3 _NewDir = -transform.right;
                    RandomDirection = _NewDir;
                }
                else
                {
                    Vector3 _NewDir = transform.right;
                    RandomDirection = _NewDir;
                }
            }
        }
        Debug.DrawRay(RayTransform.position, transform.forward);








        //if (Obstacles.Length > 0)
        //{
        //    Transform ObstacleTrasform;
        //    ObstacleTrasform = Obstacles[0].transform;

        //    Vector3 OppositeDirection = (transform.position - ObstacleTrasform.position).normalized;
        //    OppositeDirection.y = 0f;
        //    RandomDirection = OppositeDirection;
        //    Debug.Log(Obstacles[0].name);
        //}

    }

    void CombineMove()
    {
        //Move
        //transform.position += RandomDirection * CombineSpeed * Time.deltaTime;
        rb.velocity = RandomDirection * CombineSpeed * Time.deltaTime;

        //LookTowards

        transform.forward = Vector3.Lerp(transform.forward, RandomDirection, CombineRotSpeed);
    }


    private void OnDrawGizmos()
    {
    }


}//CLASS
