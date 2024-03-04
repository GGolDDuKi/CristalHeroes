using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherUpgrade : MonoBehaviour
{
    //�ڽ� ������Ʈ �ؽ�Ʈ ���� ����
    public Text detailObject;
    Text detail;
    public Text levelObject;
    Text level;
    public Text costObject;
    Text cost;

    public GameObject archerJewerly;    //�ü� ���� ���� ������Ʈ
    public int archerJewelCnt;      //������ �ü� ���� ��

    public Button button;
    public GameObject lackNotice;

    public GameObject managerObject; //������ ��ȭ �ܰ�, ���, ���ʽ��� �ҷ����� ����
    ArcherManager archerManager;

    private void Start()
    {
        button.onClick.AddListener(Upgrade);
        detail = detailObject.GetComponent<Text>();
        level = levelObject.GetComponent<Text>();
        cost = costObject.GetComponent<Text>();
        archerManager = managerObject.GetComponent<ArcherManager>();
    }

    private void Update()
    {
        archerJewelCnt = archerJewerly.GetComponent<DpsDragDrop>().count;
        level.text = archerManager.archerLevel.ToString();
        cost.text = "��ȭ��� : " + archerManager.archerCost;
        detail.text = "������ �� �� �⺻ ������ ������ �� ����\n" + archerManager.archerBonus + " �� " + (archerManager.archerBonus + 2) + " �� �߰� ���ظ� �����ϴ�.";
    }

    void Upgrade()
    {
        if (archerJewelCnt >= archerManager.archerCost)      //�ڽ�Ʈ�� ����ϴٸ�
        {
            archerJewerly.GetComponent<DpsDragDrop>().count = archerJewerly.GetComponent<DpsDragDrop>().count - archerManager.archerCost;
            Debug.Log("�ü� ������ ���׷��̵� �߽��ϴ�.");
            archerManager.archerLevel += 1;     //������
        }
        else    //����� �����ϴٸ�
        {
            //��� UI������ ���� ���� ��
            lackNotice.SetActive(true);
            Debug.Log("������ �����մϴ�.");
        }
    }
}
