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
    [SerializeField] Transform ChickenParent;
    private GameObject ParentInstance;
    private Color CombineColor;


    //EAT STUFF
    [SerializeField] private Transform EatBoxCenter;
    [SerializeField] private Vector3 EatBoxSize;
    [SerializeField] private LayerMask EatBoxMask;

    //DIE STUFF
    [SerializeField] private GameObject BrokenCombine;
    [SerializeField] private GameObject BrokenChickenBox;
    [SerializeField] private GameObject ChickenPrefab;



    //Leveling stuff
    public int Level;
    [SerializeField] private int AmountForNextLevel = 10;
    [SerializeField] private int ChickenCountForLevel;





    private void OnEnable()
    {
        
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
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    Die();
        //}


        //SCALING TEST
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        }
        ManageLevels();
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
                ChickenCountForLevel++;
                collider.transform.GetComponent<ChickenScript>().Die();
            }

            // 16TH LAYER IS EATBOX LAYER
            if(collider.gameObject.layer == 16)
            {
                if(collider.GetComponentInParent<CombineManagerScript>().Level < Level)
                {
                    //KILL
                    collider.GetComponentInParent<CombineManagerScript>().Die();
                }
            }

            // 11TH LAYER IS PLAYER LAYER AND 15TH LAYER IS ENEMYCOMBINE LAYER
            if (collider.gameObject.layer == 15 || collider.gameObject.layer == 11)
            {
                collider.GetComponent<CombineManagerScript>().Die();
            }

        }
    }


    public void Die()
    {


        // თუ მოასწარი შეასწორე ის, რომ იმაზე მეტი ქათამი თავისუფლდება სიკვიდლის შემდეგ ვიდრე იყო

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

                for (int i = 0; i < 5 + ChickenCount; i++)
                {
                    Vector3 _RandomPos = new Vector3(Random.Range(1f, 2f), 0.5f, Random.Range(1f, 2f));
                    GameObject _Chicken = Instantiate(ChickenPrefab, _ChickenBox.transform.position + _RandomPos, Quaternion.identity);
                    _Chicken.transform.SetParent(ChickenParent);

                }

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
        if (ChickenCountForLevel >= AmountForNextLevel )
        {
            Level++;
            AmountForNextLevel += 2;
            transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            ChickenCountForLevel = 0;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(EatBoxCenter.position, transform.rotation, EatBoxSize);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }


}//CLASS
