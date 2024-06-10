using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject[] sections;
    public int zValue = 70;

    public bool creatingSection = false;
    public int sectionNumber;

    public int count = 0;
    void Update()
    {
        if (!creatingSection & (count < 10)){ //generates 10 sections in total
            creatingSection = true;
            StartCoroutine(GenerateSection());
        }
        
    }

    IEnumerator GenerateSection(){
        sectionNumber = Random.Range(0, 3);
        Instantiate(sections[sectionNumber], new Vector3(0 ,0 , zValue), Quaternion.identity);
        zValue += 70;
        yield return new WaitForSeconds(2);
        count++;
        creatingSection = false;
        
    }
}
