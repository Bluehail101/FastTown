using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float gold;
    public float wood;
    public float food;

    public List<float> rateList = new List<float>();

    void Start()
    {
        StartCoroutine(clock());
    }

    public IEnumerator clock()
    {
        while (true)
        {
            gold += rateList[0];
            wood += rateList[1];
            food += rateList[2];
            yield return new WaitForSeconds(0.5f);
        }
    }
}
