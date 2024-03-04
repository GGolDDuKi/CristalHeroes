using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestManager : MonoBehaviour
{
    //���� ��ȭ ���� ����
    public int priestLevel;     //���� ���� ��ȭ ����
    public int priestCost;
    public float priestBonus;

    void Start()
    {
        priestLevel = 0;    //���߿� 0���� ����
    }

    void Update()
    {
        priestCost = 1 + (priestLevel * 2);     //�������� ���׷��̵� ��� 2�� ����
        priestBonus = 0.0f + ((float)priestLevel * 0.1f);       //�������� ���ʽ� 10%�� ����
    }
}
