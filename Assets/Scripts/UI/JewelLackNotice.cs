using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JewelLackNotice : MonoBehaviour
{
    GameObject lackJewel;
    public Button close;

    // Start is called before the first frame update
    void Start()
    {
        close.onClick.AddListener(Close);
        lackJewel = GameObject.Find("LackJewel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Close()
    {
        //�ݱ⸦ ������ LackJewel(������ �����ϴٰ� �˸��� â)�� ��Ȱ��ȭ
        lackJewel.SetActive(false);
    }
}
