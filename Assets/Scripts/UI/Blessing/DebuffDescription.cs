using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DebuffDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionUI; // ����â UI

    void Start()
    {
        descriptionUI.SetActive(false); // ����â UI�� ��Ȱ��ȭ
    }

    // ���콺�� �÷��� �� ȣ��Ǵ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionUI.SetActive(true); // ����â UI�� Ȱ��ȭ
    }

    // ���콺�� ������ �� ȣ��Ǵ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionUI.SetActive(false); // ����â UI�� ��Ȱ��ȭ
    }
}
