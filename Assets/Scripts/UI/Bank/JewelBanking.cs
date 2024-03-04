using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JewelBanking : MonoBehaviour
{
    TankDragDrop tankDragDrop;
    DpsDragDrop dpsDragDrop;
    HealDragDrop healDragDrop;

    public int bankedKnightJewel;   //����� ��� ���� ��
    public int bankedArcherJewel;   //����� �ü� ���� ��
    public int bankedPriestJewel;   //����� ���� ���� ��
    public int interestKnightJewel;   //��� ���� ����
    public int interestArcherJewel;   //�ü� ���� ����
    public int interestPriestJewel;   //���� ���� ����

    public Button knightButton;     //��纸�� ��ư
    public Button archerButton;     //�ü����� ��ư
    public Button priestButton;     //�������� ��ư
    public Button getBackButton;       //���� �����ޱ� ��ư
    public GameObject lackNotice;

    public GameObject knightJewelPrefab;
    public GameObject archerJewelPrefab;
    public GameObject priestJewelPrefab;


    // Start is called before the first frame update
    void Start()
    {
        tankDragDrop = GameObject.Find("TankJewely").GetComponent<TankDragDrop>();
        dpsDragDrop = GameObject.Find("DPSJewely").GetComponent<DpsDragDrop>();
        healDragDrop = GameObject.Find("HealJewely").GetComponent<HealDragDrop>();
        knightButton.onClick.AddListener(KnightBanking);
        archerButton.onClick.AddListener(ArcherBanking);
        priestButton.onClick.AddListener(PriestBanking);
        getBackButton.onClick.AddListener(GetBack);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void KnightBanking()        //��� ������ 1�� �̻��̶��, ��纸�� 1 ��ŷ, �������� ������ �̵��ϴ� �ִϸ��̼�
    {
        if(tankDragDrop.count > 0)
        {
            Debug.Log("��� ������ �����ϼ̽��ϴ�.");
            bankedKnightJewel++;
            tankDragDrop.count--;
            GameObject jewel = (GameObject)Instantiate(knightJewelPrefab, GameObject.Find("UI(Camera)").transform);
            jewel.transform.position = knightButton.GetComponent<RectTransform>().position;
        }
        else    //���� ���ڶ�� ���
        {
            Debug.Log("������ ���ڶ��ϴ�.");
            lackNotice.SetActive(true);
        }
    }

    void ArcherBanking()        //�ü� ������ 1�� �̻��̶��, �ü����� 1 ��ŷ, �������� ������ �̵��ϴ� �ִϸ��̼�
    {
        if (dpsDragDrop.count > 0)
        {
            Debug.Log("�ü� ������ �����ϼ̽��ϴ�.");
            bankedArcherJewel++;
            dpsDragDrop.count--;
            GameObject jewel = (GameObject)Instantiate(archerJewelPrefab, GameObject.Find("UI(Camera)").transform);
            jewel.transform.position = archerButton.GetComponent<RectTransform>().position;
        }
        else    //���� ���ڶ�� ���
        {
            Debug.Log("������ ���ڶ��ϴ�.");
            lackNotice.SetActive(true);
        }
    }

    void PriestBanking()        //���� ������ 1�� �̻��̶��, �������� 1 ��ŷ, �������� ������ �̵��ϴ� �ִϸ��̼�
    {
        if (healDragDrop.count > 0)
        {
            Debug.Log("���� ������ �����ϼ̽��ϴ�.");
            bankedPriestJewel++;
            healDragDrop.count--;
            GameObject jewel = (GameObject)Instantiate(priestJewelPrefab, GameObject.Find("UI(Camera)").transform);
            jewel.transform.position = priestButton.GetComponent<RectTransform>().position;
        }
        else    //���� ���ڶ�� ���
        {
            Debug.Log("������ ���ڶ��ϴ�.");
            lackNotice.SetActive(true);
        }
    }

    void GetBack()      //������ ���ܿ� �����صξ��� ��� ������ ���ڿ� �Բ� ��ȯ��
    {
        Debug.Log("��Ƶξ��� ������ �������̽��ϴ�.");

        tankDragDrop.count = tankDragDrop.count + bankedKnightJewel;
        dpsDragDrop.count = dpsDragDrop.count + bankedArcherJewel;
        healDragDrop.count = healDragDrop.count + bankedPriestJewel;

        bankedKnightJewel = 0;
        bankedArcherJewel = 0;
        bankedPriestJewel = 0;
        interestKnightJewel = 0;
        interestArcherJewel = 0;
        interestPriestJewel = 0;
    }
}
