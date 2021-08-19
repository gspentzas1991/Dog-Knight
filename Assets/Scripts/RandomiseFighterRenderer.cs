using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseFighterRenderer : MonoBehaviour
{
    private List<GameObject> headPrefabs = new List<GameObject>();
    private List<GameObject> bodyPrefabs = new List<GameObject>();
    private List<GameObject> shieldPrefabs = new List<GameObject>();
    private List<GameObject> weaponPrefabs = new List<GameObject>();

    private Fighter fighter;

    void Start()
    {
        fighter = GetComponent<Fighter>();
        IterateChildObjects(gameObject);
        var headPrefab = headPrefabs[Random.Range(0, headPrefabs.Count)];
        headPrefab.SetActive(true);
        //The name of the character's species depends on the customizationHead name
        fighter.displayName = headPrefab.name;
        bodyPrefabs[Random.Range(0, bodyPrefabs.Count)].SetActive(true);
        shieldPrefabs[Random.Range(0, shieldPrefabs.Count)].SetActive(true);
        weaponPrefabs[Random.Range(0, weaponPrefabs.Count)].SetActive(true);
    }

    void IterateChildObjects(GameObject obj)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
                continue;
            if (child.CompareTag("CustomizationHead"))
            {
                headPrefabs.Add(child.gameObject);
            }
            else if (child.CompareTag("CustomizationBody"))
            {
                bodyPrefabs.Add(child.gameObject);
            }
            else if (child.CompareTag("CustomizationWeapon"))
            {
                weaponPrefabs.Add(child.gameObject);
            }
            else if (child.CompareTag("CustomizationShield"))
            {
                shieldPrefabs.Add(child.gameObject);
            }
            IterateChildObjects(child.gameObject);
        }
    }
}
