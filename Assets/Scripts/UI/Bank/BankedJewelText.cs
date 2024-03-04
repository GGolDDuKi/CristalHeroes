using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankedJewelText : MonoBehaviour
{
    Text knightText;
    Text archerText;
    Text priestText;
    Text knightInterest;
    Text archerInterest;    
    Text priestInterest;

    // Start is called before the first frame update
    void Start()
    {
        knightText = gameObject.transform.GetChild(0).GetComponent<Text>();
        archerText = gameObject.transform.GetChild(1).GetComponent<Text>();
        priestText = gameObject.transform.GetChild(2).GetComponent<Text>();
        knightInterest = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        archerInterest = gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        priestInterest = gameObject.transform.GetChild(2).GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        knightText.text = "����� ��� ���� : " + gameObject.GetComponentInParent<JewelBanking>().bankedKnightJewel + "��";
        archerText.text = "����� �ü� ���� : " + gameObject.GetComponentInParent<JewelBanking>().bankedArcherJewel + "��";
        priestText.text = "����� ���� ���� : " + gameObject.GetComponentInParent<JewelBanking>().bankedPriestJewel + "��";
        knightInterest.text = "���ڷ� ���� ���� : " + gameObject.GetComponentInParent<JewelBanking>().interestKnightJewel + "��";
        archerInterest.text = "���ڷ� ���� ���� : " + gameObject.GetComponentInParent<JewelBanking>().interestArcherJewel + "��";
        priestInterest.text = "���ڷ� ���� ���� : " + gameObject.GetComponentInParent<JewelBanking>().interestPriestJewel + "��";
    }
}
