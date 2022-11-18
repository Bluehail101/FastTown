using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Resource : MonoBehaviour
{
    public float gold;
    public float food;
    public float wood;

    public float maxGold;
    public float maxFood;
    public float maxWood;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI woodText;

    public int maxFailTime;
    private int currentFailTime;
    public bool failClockRunning;


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
            
            if(checkAllValid() == false)
            {
                if (failClockRunning == false) { currentFailTime = 0; StartCoroutine(failClock()); }
            }

        }
    }

    public void updateResources()
    {
        goldText.text = "Gold: " + MathF.Truncate(gold).ToString() + " / " + maxGold;
        foodText.text = "Food: " + MathF.Truncate(food).ToString() + " / " + maxFood;
        woodText.text = "Wood: " + MathF.Truncate(wood).ToString() + " / " + maxWood;
    }

    
    public bool checkAllValid()
    {
        if(checkValid(gold, maxGold) == false) { return false; }
        if(checkValid(food, maxFood) == false) { return false; }
        if(checkValid(wood, maxWood) == false) { return false; }
        return true;
    }
    public bool checkValid(float value, float maxValue)
    {
        if(value < 0 || value > maxValue)
        {
            return false;
        }
        return true;
    }

    public IEnumerator failClock()
    {
        while (true)
        {
            failClockRunning = true;
            if (checkAllValid() == true) { failClockRunning = false; StopCoroutine(failClock()); }
            currentFailTime += 1;
            if (currentFailTime >= maxFailTime)
            {
                SceneManager.LoadScene(1);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
