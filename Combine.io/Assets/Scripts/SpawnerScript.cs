using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject Chicken;
    [SerializeField] private GameObject ChickenParent;
    [SerializeField] private Vector3 RandomRangePos;
    [SerializeField] private int AmountOfChicken;
    [SerializeField] private LayerMask SpawnMask;

    private Vector3 randomPos;
    private int ChickenAmount;
    private int ChickenSpawned;


    void Start()
    {
        ChickenSpawn(AmountOfChicken , transform.position);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            //chickenamount

            Debug.Log(ChickenParent.transform.childCount);


        }
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

}//CLASS
