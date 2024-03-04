using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManager : MonoBehaviour
{
    //���� ��ȭ ���� ����
    public int archerLevel;     //�ü� ���� ��ȭ ����
    public int archerCost;
    public int archerBonus;

    void Start()
    {
        archerLevel = 0;    //���߿� 0���� ����
    }

    void Update()
    {
        archerCost = 1 + (archerLevel * 2);     //�������� ���׷��̵� ��� 2�� ����
        archerBonus = 0 + (archerLevel * 2);       //�⺻ 10, �������� ���ʽ� 2�� ����
    }
}
