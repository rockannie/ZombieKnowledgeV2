using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class zombieSpawner : MonoBehaviour
{
    //private variables
    private int numZombs = 1;
    
    //Serialized gameobjects
    [SerializeField] private Tilemap walkieTilemap;
    
    [SerializeField] private GameObject zombie;
    [SerializeField] private Transform knightie;
    
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject knightiepoo;
    
    private knightUIController knightUICont;
    
    void Start()
    {
        knightUICont = knightiepoo.GetComponent<knightUIController>();
        StartCoroutine(makeZombs());
    }

    IEnumerator makeZombs()
    {
        int i = 0;
        while (i < numZombs)
        {
            Vector3 temp = transform.position + Vector3.down;
            GameObject newZomb = Instantiate(zombie, temp, Quaternion.identity, transform);
            newZomb.SetActive(true);
            newZomb.GetComponent<zombieController>().knight = knightie;
            newZomb.GetComponent<zombieController>().walkableTilemap = walkieTilemap;
            newZomb.GetComponent<zombieController>().KUIController = knightUICont;
            i++;
        }

        numZombs++;
        yield return new WaitForSeconds(60f);
        StartCoroutine(makeZombs());
    }

    void Update()
    {
        
    }
}
