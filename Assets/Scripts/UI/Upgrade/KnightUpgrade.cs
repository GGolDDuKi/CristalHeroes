using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightUpgrade : MonoBehaviour
{
    //�ڽ� ������Ʈ �ؽ�Ʈ ���� ����
    public Text detailObject;
    Text detail;
    public Text levelObject;
    Text level;
    public Text costObject;
    Text cost;

    public GameObject knightJewerly;    //��� ���� ���� ������Ʈ
    public int knightJewelCnt;      //������ ��� ���� ��

    public Button button;
    public GameObject lackNotice;

    public GameObject managerObject; //������ ��ȭ �ܰ�, ���, ���ʽ��� �ҷ����� ����
    KnightManager knightManager;

    private void Start()
    {
        button.onClick.AddListener(Upgrade);
        detail = detailObject.GetComponent<Text>();
        level = levelObject.GetComponent<Text>();
        cost = costObject.GetComponent<Text>();
        knightManager = managerObject.GetComponent<KnightManager>();
    }

    private void Update()
    {
        knightJewelCnt = knightJewerly.GetComponent<TankDragDrop>().count;
        level.text = knightManager.knightLevel.ToString();
        cost.text = "��ȭ��� : " + knightManager.knightCost;
        detail.text = "���� ���� �� " + knightManager.knightBonus + " �� " + (knightManager.knightBonus + 2) + " �� ��ȣ���� �����ϴ�.";
    }

    void Upgrade()
    {
        if(knightJewelCnt >= knightManager.knightCost)      //�ڽ�Ʈ�� ����ϴٸ�
        {
            Debug.Log("��� ������ ���׷��̵� �߽��ϴ�.");
            knightJewerly.GetComponent<TankDragDrop>().count = knightJewerly.GetComponent<TankDragDrop>().count - knightManager.knightCost;
            knightManager.knightLevel += 1;     //������
        }
        else    //����� �����ϴٸ�
        {
            //��� UI������ ���� ���� ��
            lackNotice.SetActive(true);
            Debug.Log("������ �����մϴ�.");
        }
    }
}
