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
    private bool isAlive;
    private PlayerMovement playerMovement;
    private EnemyCombineMovement enemyCombineMovement;
    private Rigidbody rb;

    //EAT STUFF
    [SerializeField] private Transform EatBoxCenter;
    [SerializeField] private Vector3 EatBoxSize;
    [SerializeField] private LayerMask EatBoxMask;

    //DIE STUFF
    [SerializeField] private GameObject BrokenCombine;
    [SerializeField] private GameObject BrokenChickenBox;
    [SerializeField] private GameObject ChickenPrefab;
    [SerializeField] private SpawnerScript spawnerScript;
    

    //RESPAWNING STUFF
    [SerializeField] private Vector3 RandomRespawnPos;
    [SerializeField] private LayerMask SpawnMask;



    //Leveling stuff
    public int Level;
    [SerializeField] private int MaxLevel;
    [SerializeField] private int AmountForNextLevel = 10;
    [SerializeField] private int ChickenCountForLevel;





    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        spawnerScript = FindObjectOfType<SpawnerScript>();
        playerMovement = GetComponent<PlayerMovement>();
        enemyCombineMovement = GetComponent<EnemyCombineMovement>();
        isAlive = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnerScript = FindObjectOfType<SpawnerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            CombineEat();
            ChickenBoxManage();
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
            if(collider.gameObject.layer == 16 && collider.GetComponentInParent<CombineManagerScript>() != this)
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




    private void ChickenFree()
    {
        if (ParentInstance != null)
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

    }

    private void BrakeCombine()
    {
        GameObject _BrokenCombine = Instantiate(BrokenCombine, transform.position, transform.rotation);



        Rigidbody[] _Rigidbodies = _BrokenCombine.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rig in _Rigidbodies)
        {
            rig.velocity = Vector3.zero;
        }


        MeshRenderer[] brokenCombineRenderers = _BrokenCombine.GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] thisCombineRenderers = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < brokenCombineRenderers.Length; i++)
        {

            brokenCombineRenderers[i].materials = thisCombineRenderers[i].materials;

        }

        Destroy(_BrokenCombine, 3f);
    }
    public void Die()
    {
        GetComponent<BoxCollider>().enabled = false;
        ChickenFree();
        BrakeCombine();



        //RESPAWNING STUFF
        GameObject _combineChild = GetComponentInChildren<Animator>().gameObject;
        _combineChild.SetActive(false);
        StartCoroutine(Respawn(_combineChild));

        findRandomPos();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        else
        {
            enemyCombineMovement.enabled = false;
        }

        rb.isKinematic = true;
        isAlive = false;

    }











    void findRandomPos()
    {
        Vector3 _RespawnPos = transform.position + new Vector3(Random.Range(RandomRespawnPos.x, -RandomRespawnPos.x), 5f, Random.Range(RandomRespawnPos.z, -RandomRespawnPos.z));
        RaycastHit[] rayHits;

        rayHits = Physics.RaycastAll(_RespawnPos, Vector3.down, SpawnMask);

        if (rayHits.Length > 0)
        {

            if (rayHits[0].transform.gameObject.layer == 13)
            {
                transform.position = new Vector3(_RespawnPos.x , 0f, _RespawnPos.z);

            }
            else
            {
                findRandomPos();
            }
        }
    }

    IEnumerator Respawn(GameObject _object)
    {
        yield return new WaitForSeconds(4f);
        _object.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
        isAlive = true;
        AmountForNextLevel = default;
        ChickenCountForLevel = default;
        Level = default;
        transform.localScale = new Vector3(1,1,1);
        ChickenCount = default;


        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        else
        {
            enemyCombineMovement.enabled = true;
        }
        rb.isKinematic = false;

    }




    void ManageLevels()
    {
        if (ChickenCountForLevel >= AmountForNextLevel && Level < MaxLevel)
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
