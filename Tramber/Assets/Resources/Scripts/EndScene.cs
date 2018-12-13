using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour {

    public GameObject[] scoreItemTexts;
    int[] scores = new int[7]{2,2,2,2,2,2,2};
    public Sprite[] gradeSpriteMap;

    string[] scoreStringMap = { "D", "C", "B", "A", "S" };

    public GameObject bigGrade;

    public GameObject happyFace;
    public GameObject sadFace;

    public GameObject flowerHappyMark;
    public GameObject flowerAngryMark;

    public GameObject backButton;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CalcTime()
    {
        var tronTime = 130;
        var t = LevelManager.Instance.playTime;

        if( t < 150)
        {
            scores[0] = 4;
        }
        else if (t < 180)
        {
            scores[0] = 3;
        }
        else if (t < 230)
        {
            scores[0] = 2;
        }
        else if (t < 300)
        {
            scores[0] = 1;
        }
        else
        {
            scores[0] = 0;
        }
    }

    void CalcHandsome()
    {
        var sc = Random.Range(0, 5);

        sc = sc.Clamp(0, 4);
        scores[3] = sc;
    }

    void CalcAcc()
    {
        var sc = Random.Range(3, 5);

        sc = sc.Clamp(0, 4);
        scores[1] = sc;
    }

    void CalcProtection()
    {
        int count = LevelManager.Instance.flowerDamagedCount;
        if(count == 0)
        {
            scores[2] = 4;
        }
        else if (count <= 2)
        {
            scores[2] = 3;
        }
        else if (count <= 5)
        {
            scores[2] = 2;
        }
        else if (count <= 7)
        {
            scores[2] = 1;
        }
        else
            scores[2] = 0;
    }

    void CalcCrash()
    {
        int count = LevelManager.Instance.shipCrashCount;
        if (count <= 4)
        {
            scores[4] = 4;
        }
        else if (count <= 8)
        {
            scores[4] = 3;
        }
        else if (count <= 12)
        {
            scores[4] = 2;
        }
        else if (count <= 16)
        {
            scores[4] = 1;
        }
        else
            scores[4] = 0;
    }

    void CalcAsteroid()
    {
        int count = LevelManager.Instance.asteroidDestroyedCount;
        if (count >= 2)
        {
            scores[5] = 4;
        }
        else if (count >= 1)
        {
            scores[5] = 3;
        }        
        else
            scores[5] = 2;
    }

    void CalcMission()
    {
        int count = LevelManager.Instance.feededCount;
        if(count >= 6)
        {
            scores[6] = 4;
        }
        else if (count >= 5)
        {
            scores[6] = 3;
        }
        else if (count >= 3)
        {
            scores[6] = 2;
        }
        else if (count >= 2)
        {
            scores[6] = 1;
        }
        else
        {
            scores[6] = 0;
        }
    }


    void CalcAll()
    {
        CalcAcc();
        CalcAsteroid();
        CalcCrash();
        CalcHandsome();
        CalcMission();
        CalcProtection();
        CalcTime();
    }

    public void ShowScoreAnimation()
    {
        CalcAll();

        var sum = 0;
        foreach(var s in scores)
        {
            sum += s;
        }
        float averF = (float)sum / (float)scores.Length;
        int averI = Mathf.RoundToInt(averF);
        averI = averI.Clamp(0, gradeSpriteMap.Length - 1);
        bigGrade.GetComponent<Image>().sprite = gradeSpriteMap[averI];

        bool happy = false;
        if (averI >= 2)
            happy = true;

        var interval = 0.3f;
        var intervalWithGrade = 0.3f; 
        var seq = DOTween.Sequence();
      
        for(int i = 0; i < scoreItemTexts.Length; i++)
        {
            var item = scoreItemTexts[i];
            var itemScore = scoreStringMap[scores[i]];

            seq.AppendInterval(interval);

            
            seq.AppendCallback(() =>
            {
                item.SetActive(true);
            });
            seq.AppendInterval(intervalWithGrade);
            

            seq.AppendCallback(() =>
            {
                item.GetComponent<Text>().text += itemScore;
            });
        }        

        seq.AppendInterval(1.0f);
        seq.AppendCallback(()=> {
            bigGrade.SetActive(true);
            bigGrade.SendFSMEvent("SHOW");

            happyFace.SetActive(happy);
            sadFace.SetActive(!happy);

            flowerHappyMark.SetActive(happy);
            flowerAngryMark.SetActive(!happy);

            backButton.SetActive(true);
        });
    }
}
