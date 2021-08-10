using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    //CHICKEN STUFF
    [SerializeField] private GameObject Chicken;
    [SerializeField] private GameObject ChickenParent;
    [SerializeField] private Vector3 RandomRangePos;
    [SerializeField] private int AmountOfChicken;
    [SerializeField] private LayerMask SpawnMask;

    private Vector3 randomPos;
    private int ChickenAmount;
    private int ChickenSpawned;





    //COMBINE STUFF
    [SerializeField] private int combineAmount = 5;
    [SerializeField] private GameObject CombinePref;
    [SerializeField] private Transform CombineParentsTransform;


    private bool Spawned;



    void Start()
    {
        ChickenSpawn(AmountOfChicken , transform.position);

    }

    private void Update()
    {
    }

    public void TEST()
    {
        Debug.Log("TESTESTESTESTEST");
    }
    public void ChickenSpawn(int _ChickenAmount , Vector3 centerPos)
    {
        ChickenAmount = _ChickenAmount;

        for (int i = 0; i < ChickenAmount; i++)
        {

            randomPos = centerPos + new Vector3(Random.Range(RandomRangePos.x, -RandomRangePos.x), 5f, Random.Range(RandomRangePos.z, -RandomRangePos.z));

            RaycastHit[] rayHits;

            rayHits = Physics.RaycastAll(randomPos, Vector3.down, SpawnMask);


            if (rayHits.Length > 0)
            {

                if (rayHits[0].transform.gameObject.layer == 13)
                {
                    GameObject _Chicken = Instantiate(Chicken, randomPos, Quaternion.identity);
                    _Chicken.transform.SetParent(ChickenParent.transform);
                    ChickenSpawned++;
                }
                else
                {
                    if (ChickenAmount <= 2000 && ChickenSpawned <= _ChickenAmount)
                    {
                        ChickenAmount++;
                    }
                }
            }

            
        }

        Debug.Log(ChickenSpawned);
    }


    public void CombineSpawn(GameObject oldCombine)
    {

        //CombineSpawn();





    }

    public IEnumerator SpawnCombine(GameObject oldCombine)
    {
        MeshRenderer[] oldCombineRenderers = oldCombine.GetComponentsInChildren<MeshRenderer>();
        Debug.Log(oldCombineRenderers.Length);

        yield return new WaitForSeconds(2f);

        GameObject newCombine = Instantiate(CombinePref, randomPos, Quaternion.identity);


        MeshRenderer[] newCombineRenderers = newCombine.GetComponentsInChildren<MeshRenderer>();

        Debug.Log(oldCombineRenderers.Length);
        Debug.Log(newCombineRenderers.Length);


        for (int i = 0; i < newCombineRenderers.Length; i++)
        {
            
            //newCombineRenderers[i].materials = oldCombineRenderers[i].materials;
            Debug.Log(oldCombineRenderers[i].materials.Length);
        }


    }

}//CLASS
