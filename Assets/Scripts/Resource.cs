using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Resource : MonoBehaviour
{
    public float gold;
    public float food;
    public float wood;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI woodText;

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
            food += rateList[1];
            wood += rateList[2];
            updateResources();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void updateResources()
    {
        goldText.text = "Gold: " + MathF.Truncate(gold).ToString();
        foodText.text = "Food: " + MathF.Truncate(food).ToString();
        woodText.text = "Wood: " + MathF.Truncate(wood).ToString();
    }
}
