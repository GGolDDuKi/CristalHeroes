using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightManager : MonoBehaviour
{
    //���� ��ȭ ���� ����
    public int knightLevel;     //��� ���� ��ȭ ����
    public int knightCost;
    public int knightBonus;

    void Start()
    {
        knightLevel = 0;    //���߿� 0���� ����
    }

    void Update()
    {
        knightCost = 1 + (knightLevel * 2);     //�������� ���׷��̵� ��� 2�� ����
        knightBonus = 0 + (knightLevel * 2);       //�⺻ 0, �������� ���ʽ� 2�� ����
    }
}
