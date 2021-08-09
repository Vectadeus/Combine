using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineManagerScript : MonoBehaviour
{
    public int ChickenCount;
    public GameObject ChickenBox;
    public GameObject ChickenBoxParent;
    public bool ParentCreated;
    public Transform ChickenBoxPoint;


    //EAT STUFF
    public Transform EatBoxCenter;
    public Vector3 EatBoxSize;
    public LayerMask EatBoxMask;


    //Leveling stuff


    //TEST
    private GameObject ParentInstance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CombineEat();
        ChickenBoxManage();
    }



    void ChickenBoxManage()
    {
        if (ChickenCount >= 5)
        {
            if (!ParentCreated)
            {
                ParentInstance = Instantiate(ChickenBoxParent, transform.position, Quaternion.identity);
                ParentInstance.name = transform.name + "Chickenboxes";
                ParentCreated = true;
            }

            GameObject _ChickenBox = Instantiate(ChickenBox, ChickenBoxPoint.position, transform.rotation);
            _ChickenBox.transform.SetParent(ParentInstance.transform);

            Color CombineColor = GetComponentInChildren<MeshRenderer>().materials[0].color;
            _ChickenBox.GetComponentInChildren<MeshRenderer>().materials[0].color = CombineColor;
            _ChickenBox.GetComponentInChildren<MeshRenderer>().materials[1].color = CombineColor;


            ChickenCount = 0;
        }

    }

    void CombineEat()
    {
        Collider[] boxcasts;
        boxcasts = Physics.OverlapBox(EatBoxCenter.position, EatBoxSize, transform.rotation);

        foreach (Collider collider in boxcasts)
        {
            //9TH LAYER IS CHICKEN LAYER
            if (collider.gameObject.layer == 9)
            {
                ChickenCount++;
                collider.transform.GetComponent<ChickenScript>().Die();
                Destroy(collider.gameObject);
            }
        }
    }

    void ManageLevels()
    {

    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.matrix = Matrix4x4.TRS(EatBoxCenter.position, transform.rotation, EatBoxSize);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(Vector3.zero, Vector3.one);
    //}


}//CLASS
