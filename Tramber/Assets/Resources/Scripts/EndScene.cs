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



    public void ShowScoreAnimation()
    {
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
