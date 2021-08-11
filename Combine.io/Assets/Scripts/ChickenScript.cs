using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    [SerializeField] private float ChickenSpeed;
    [SerializeField] private float RotSpeed;
    [SerializeField] private GameObject Particles;

    // A.I STUFF
    [SerializeField] private float SphereRadius;
    [SerializeField] private LayerMask ObstacleMask;
    [SerializeField] private LayerMask SpawnMask;




    private Vector3 RandomDirection;



    [SerializeField] private Vector3 rayOffset;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeDirection());
        DieIfOutOfZoneI();
    }

    // Update is called once per frame
    void Update()
    {
        ChickenMove();
        AvoidObstacles();
        DieIfOutOfZone();
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
        Destroy(this.gameObject);
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

    void DieIfOutOfZone()
    {
        RaycastHit[] rayhits = Physics.RaycastAll(transform.position + rayOffset, Vector3.down, SpawnMask);

        //13TH LAYER IS SPAWNHERELAYER
        if(rayhits.Length > 0)
        {
            if (rayhits[0].transform.gameObject.layer != 13)
            {
                Destroy(this.gameObject);
            }
        }

    }

    IEnumerator DieIfOutOfZoneI()
    {
        yield return new WaitForSeconds(3f);
        RaycastHit[] rayhits = Physics.RaycastAll(transform.position + rayOffset, Vector3.down, SpawnMask);

        //13TH LAYER IS SPAWNHERELAYER
        if (rayhits.Length > 0)
        {
            if (rayhits[0].transform.gameObject.layer != 13)
            {
                Destroy(this.gameObject);
            }
        }
        StartCoroutine(DieIfOutOfZoneI());
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, SphereRadius);
    //}

}//CLASS
