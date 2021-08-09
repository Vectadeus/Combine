using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineManagerScript : MonoBehaviour
{
  
    
    private int ChickenTotalCount;

    
    private int ChickenCount; //for box output
    


    [SerializeField] private GameObject ChickenBox;
    [SerializeField] private GameObject ChickenBoxParent;
    [SerializeField] private bool ParentCreated;
    [SerializeField] Transform ChickenBoxPoint;
    private GameObject ParentInstance;
    private Color CombineColor;


    //EAT STUFF
    [SerializeField] private Transform EatBoxCenter;
    [SerializeField] private Vector3 EatBoxSize;
    [SerializeField] private LayerMask EatBoxMask;

    //DIE STUFF
    [SerializeField] private GameObject BrokenCombine;
    [SerializeField] private GameObject BrokenChickenBox;
    private SpawnerScript spawnerscript;
    //Leveling stuff


    private void OnEnable()
    {
        spawnerscript = FindObjectOfType<SpawnerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CombineEat();
        ChickenBoxManage();

        //Die test
        if (Input.GetKeyDown(KeyCode.T))
        {
            Die();
        }
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

             CombineColor = GetComponentInChildren<MeshRenderer>().materials[0].color;
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
                ChickenTotalCount++;
                collider.transform.GetComponent<ChickenScript>().Die();
            }

            // 11TH LAYER IS PLAYER LAYER AND 15TH LAYER IS ENEMYCOMBINE LAYER
            if (collider.gameObject.layer == 15 || collider.gameObject.layer == 11)
            {
                //collider.GetComponent<CombineManagerScript>().Die();
            }
        }
    }


    public void Die()
    {



        //BROKE CHICKENS
        if(ParentInstance != null)
        {
            foreach (Transform _ChickenBox in ParentInstance.transform)
            {

                GameObject _BrokenChickenBox = Instantiate(BrokenChickenBox, _ChickenBox.transform.position, _ChickenBox.transform.rotation);

                MeshRenderer[] _BrokenChickenRenderers = _BrokenChickenBox.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer renderer in _BrokenChickenRenderers)
                {
                    renderer.materials[0].color = CombineColor;
                }
                Destroy(_BrokenChickenBox, 3f);
                Destroy(_ChickenBox.gameObject);

                spawnerscript.ChickenSpawn(ChickenTotalCount, _ChickenBox.transform.position);
            }
        }






        GameObject _BrokenCombine = Instantiate(BrokenCombine, transform.position, transform.rotation);


        MeshRenderer[] brokenCombineRenderers = _BrokenCombine.GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] thisCombineRenderers = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < brokenCombineRenderers.Length; i++)
        {

            brokenCombineRenderers[i].materials = thisCombineRenderers[i].materials;

        }

        Destroy(_BrokenCombine, 3f);
        Destroy(this.gameObject);



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
