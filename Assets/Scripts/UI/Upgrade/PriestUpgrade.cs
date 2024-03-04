using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriestUpgrade : MonoBehaviour
{
    //�ڽ� ������Ʈ �ؽ�Ʈ ���� ����
    public Text detailObject;
    Text detail;
    public Text levelObject;
    Text level;
    public Text costObject;
    Text cost;

    public GameObject priestJewerly;    //���� ���� ���� ������Ʈ
    public int priestJewelCnt;      //������ ���� ���� ��

    public Button button;
    public GameObject lackNotice;

    public GameObject managerObject; //������ ��ȭ �ܰ�, ���, ���ʽ��� �ҷ����� ����
    PriestManager priestManager;

    private void Start()
    {
        button.onClick.AddListener(Upgrade);
        detail = detailObject.GetComponent<Text>();
        level = levelObject.GetComponent<Text>();
        cost = costObject.GetComponent<Text>();
        priestManager = managerObject.GetComponent<PriestManager>();
    }

    private void Update()
    {
        priestJewelCnt = priestJewerly.GetComponent<HealDragDrop>().count;
        level.text = priestManager.priestLevel.ToString();
        cost.text = "��ȭ��� : " + priestManager.priestCost;
        detail.text = "��ų ȿ���� " + (int)(priestManager.priestBonus * 100) + "% �� " + (int)((priestManager.priestBonus + 0.1) * 100) + "% �����մϴ�.";
    }

    void Upgrade()
    {
        if (priestJewelCnt >= priestManager.priestCost)      //�ڽ�Ʈ�� ����ϴٸ�
        {
            priestJewerly.GetComponent<HealDragDrop>().count = priestJewerly.GetComponent<HealDragDrop>().count - priestManager.priestCost;
            Debug.Log("���� ������ ���׷��̵� �߽��ϴ�.");
            priestManager.priestLevel += 1;     //������
        }
        else    //����� �����ϴٸ�
        {
            //��� UI������ ���� ���� ��
            lackNotice.SetActive(true);
            Debug.Log("������ �����մϴ�.");
        }
    }
}
