using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    public float ChickenSpeed;
    public float RotSpeed;
    public Vector3 RandomDirection;

    public GameObject Particles;


    // A.I STUFF
    public float SphereRadius;
    public LayerMask ObstacleMask;




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    // Update is called once per frame
    void Update()
    {
        ChickenMove();
        AvoidObstacles();
    }



    void ChickenMove()
    {
        //Move
        transform.position += RandomDirection * ChickenSpeed * Time.deltaTime;

        //LookTowards

        transform.forward = Vector3.Lerp(transform.forward, RandomDirection, RotSpeed);
    }

    public void Die()
    {
        GameObject _Particles = Instantiate(Particles, transform.position, Quaternion.identity);
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(2, 4));

        RandomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        StartCoroutine(ChangeDirection());
    }

    void AvoidObstacles()
    {
        Collider[] Obstacles = Physics.OverlapSphere(transform.position, SphereRadius, ObstacleMask);

        if(Obstacles.Length > 0)
        {
            Transform ObstacleTrasform;
            ObstacleTrasform = Obstacles[0].transform;

            Vector3 OppositeDirection = (transform.position - ObstacleTrasform.position).normalized;
            RandomDirection = OppositeDirection;

        }

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, SphereRadius);
    //}

}//CLASS
