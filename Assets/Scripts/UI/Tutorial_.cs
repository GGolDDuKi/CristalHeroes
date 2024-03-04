using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_ : MonoBehaviour
{
    public int start = 0;
    public GameObject[] pop_up = new GameObject[18];
    public Control control;
    public TankDragDrop tdd;
    public DpsDragDrop ddd;
    public HealDragDrop hdd;
    public UnitSelections unitselections;
    public GameManager gm;
    public TargetList targetlist;
    public GameObject monster;
    public GameObject battlebutton;
    public GameObject shop;
    public GameObject blessing;
    public GameObject stage1;
    public GameObject overlay;
    void Start()
    {
        start = PlayerPrefs.GetInt("0");
    }

    // Update is called once per frame
    void Update()
    {
        if (start == 0) //�˾�1
        {
            pop_up[0].SetActive(true);
            battlebutton.SetActive(false);
            tdd.count = 1; ddd.count = 1; hdd.count = 1;
        }
        else if (start == 1) //�˾�2
        {
            if (control.playerlist.Count > 2)
            {
                NextPopup();
            }
        }
        else if (start == 2) //�˾�3
        {
            if (unitselections.unitsSelected.Count > 0)
            {
                NextPopup();
            }
        }
        else if (start == 3)//�˾�4
        {
            if (unitselections.unitsSelected.Count > 0)
            {
                if (Input.GetMouseButton(1))
                {
                    NextPopup();
                }
            }
        }
        else if (start == 4)//�˾�5
        {
            if (gm.isBattle)
            {
                NextPopup();
            }
        }
        else if (start == 5)//�˾�6
        {

        }
        else if (start == 6)//�˾�7 ���� ����
        {
            if (targetlist.targetAttack.Count > 0)
            {
                NextPopup();
            }
        }
        else if (start == 7)//�˾�8  �� ���
        {
            if (control.enemylist.Count == 0)
            {
                NextPopup();
            }
        }
        else if (start == 8)//�˾�9-- ��������
        {
        }
        else if (start == 9)//�˾�10
        {
        }
        else if (start == 10)//�˾�11
        {
        }
        else if (start == 11)//�˾�12
        {
        }
        else if (start == 12)//�˾�13
        {
        }
        else if (start == 13)//�˾�14
        {
        }
        else if (start == 14)//�˾�15
        {
        }
        else if (start == 15)//�˾�16
        {
        }
        else if (start == 16)//�˾�17
        {
        }
        else if (start == 17)//�˾�18
        {
        }
        else if (start == 18)
        {
            this.gameObject.SetActive(false); stage1.SetActive(true);
        }


    }

    public void rightClick()
    {
        start++;
        PlayerPrefs.SetInt("123", start);
        PopupManager();
    }

    public void Click()
    {
        pop_up[start].SetActive(false);
        Time.timeScale = 1;
    }
    void NextPopup()
    {
        rightClick();
    }

    void PopupManager()
    {
        if (start == 0) //�˾�1
        {
            pop_up[0].SetActive(true);
            Time.timeScale = 0;
        }
        else if (start == 1) //�˾�2
        {
            pop_up[1].SetActive(true); pop_up[0].SetActive(false);
            Time.timeScale = 0;
        }
        else if (start == 2) //�˾�3 ����������
        {
            pop_up[2].SetActive(true); pop_up[1].SetActive(false);
            Time.timeScale = 0;
        }
        else if (start == 3)//�˾�4 ���������� 
        {
            Time.timeScale = 0;
            pop_up[3].SetActive(true); pop_up[2].SetActive(false);
        }
        else if (start == 4)//�˾�5 ��Ʋ ���� ��������
        {
            battlebutton.SetActive(true);
            pop_up[4].SetActive(true); pop_up[3].SetActive(false);
        }
        else if (start == 5)//�˾�6 ���� ����
        {
            pop_up[5].SetActive(true); pop_up[4].SetActive(false); monster.SetActive(true);
            Time.timeScale = 0;
        }
        else if (start == 6)//�˾�7
        {
            pop_up[6].SetActive(true); pop_up[5].SetActive(false);
            Time.timeScale = 0;
        }
        else if (start == 7)//�˾�8
        {
            pop_up[7].SetActive(true); pop_up[6].SetActive(false);
            Time.timeScale = 0;
        }
        else if (start == 8)//�˾�9-- ��������
        {
            shop.SetActive(true); overlay.SetActive(false);
            pop_up[8].SetActive(true); pop_up[7].SetActive(false);
            gm.isBattle = false;
        }
        else if (start == 9)//�˾�10
        {
            pop_up[9].SetActive(true); pop_up[8].SetActive(false);
            Time.timeScale = 0;
        }
        else if (start == 10)//�˾�11 -- �ູ����
        {
            shop.SetActive(false); blessing.SetActive(true);
            pop_up[10].SetActive(true); pop_up[9].SetActive(false);
            Time.timeScale = 0;
        }
        else if (start == 11)//�˾�12
        {
            pop_up[11].SetActive(true); pop_up[10].SetActive(false);
        }
        else if (start == 12)//�˾�13
        {

            pop_up[12].SetActive(true); pop_up[11].SetActive(false);
        }
        else if (start == 13)//�˾�14
        {
            pop_up[13].SetActive(true); pop_up[12].SetActive(false);
        }
        else if (start == 14)//�˾�15
        {
            pop_up[14].SetActive(true); pop_up[13].SetActive(false);
        }
        else if (start == 15)//�˾�16
        {
            blessing.SetActive(false);
            pop_up[15].SetActive(true); pop_up[14].SetActive(false);
        }
        else if (start == 16)//�˾�17
        {
            pop_up[16].SetActive(true); pop_up[15].SetActive(false);
        }
        else if (start == 17)//�˾�18
        {
            pop_up[17].SetActive(true); pop_up[16].SetActive(false);
            Time.timeScale = 1;
        }
        else if (start == 18)
        {
            tdd.enabled = true;
            ddd.enabled = true;
            hdd.enabled = true;
            gm.UnitReposition();
            tdd.count = 2; ddd.count = 2; hdd.count = 2;
            overlay.SetActive(true); gm.readyArea.SetActive(true);
            foreach (GameObject unit in control.playerlist)
            {
                unit.GetComponent<Unit>().nowHp = 0;
                unit.GetComponent<Unit>().Die();
            }
            pop_up[17].SetActive(false);
        }
    }
}
