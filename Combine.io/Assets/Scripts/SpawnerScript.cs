using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject Chicken;
    public GameObject ChickenParent;
    public Vector3 RandomRangePos;
    private Vector3 randomPos;

    public int _ChickenAmount;
    private int ChickenAmount;
    public LayerMask SpawnMask;
    private int ChickenSpawned;


    void Start()
    {
        ChickenAmount = _ChickenAmount;

        for (int i = 0; i < ChickenAmount; i++)
        {

            randomPos = new Vector3(Random.Range(RandomRangePos.x, -RandomRangePos.x), 0.5f, Random.Range(RandomRangePos.z, -RandomRangePos.z));

            RaycastHit[] rayHits;

            rayHits = Physics.RaycastAll(randomPos, Vector3.down, SpawnMask);

            if(rayHits[0].transform.gameObject.layer == 13)
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

        Debug.Log(ChickenSpawned);
    }


}//CLASS
