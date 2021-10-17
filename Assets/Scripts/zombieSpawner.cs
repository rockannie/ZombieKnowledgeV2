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

    // Start is called before the first frame update
    void Start()
    {
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
            i++;
        }

        numZombs++;
        yield return new WaitForSeconds(30f);
        StartCoroutine(makeZombs());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
