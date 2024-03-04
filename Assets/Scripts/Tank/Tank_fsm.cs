using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class Tank_fsm : MonoBehaviour
{
    float atkspeed;
    public Attack_Sword attack_sword;
    public Tank_UnitMovement tank_unitmovement;
    public GameObject target;
    public List<GameObject> targetList = new List<GameObject>();
    private Camera myCam;
    float MaxDistance = 15f;
    Vector3 MousePosition;
    public bool aclick = false;
    public bool fight = false;
    public float DelayTime = 3.0f;
    bool on = false;
    bool player_move = false;
    float startTime = 0;
    public float currentTime = 0;
    Unit unit;
    Skill skill;
    Control control;
    SoundEffect soundeffect;
    AudioSource audiosource;
    AudioSource Se;
    Rigidbody2D rigid2d;
    public bool stop = false;
    public bool stopMp = false;
    public enum CharacterStates
    {
        Idle,
        Move,
        Attack,
        Death
    }
    private StateMachine<CharacterStates, StateDriverUnity> fsm;
    void Awake()
    {
        skill = GameObject.Find("Object_control").GetComponent<Skill>();
        control = GameObject.Find("Object_control").GetComponent<Control>();
        myCam = Camera.main;
        unit = GetComponent<Unit>();
        tank_unitmovement = gameObject.GetComponent<Tank_UnitMovement>();
        fsm = new StateMachine<CharacterStates, StateDriverUnity>(this);
        fsm.ChangeState(CharacterStates.Idle);
        control.playerlist.Add(this.gameObject);
        soundeffect = GameObject.Find("Audio_Source").GetComponent<SoundEffect>();
        audiosource = GetComponent<AudioSource>();
        Se = GameObject.Find("Audio_Source").GetComponent<AudioSource>();
        rigid2d = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        atkspeed = unit.atkSpeed;
        DelayTime = atkspeed;
        audiosource.volume = Se.volume;
        fsm.Driver.Update.Invoke();
        currentTime = Time.time - startTime;
        SwitchingTime();
        if (!unit.newChar)
        {
            if (tank_unitmovement.selected)
            {
                if (aclick == true)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        tank_unitmovement.aClick = false;
                        aclick = false;
                        MousePosition = Input.mousePosition;
                        MousePosition = myCam.ScreenToWorldPoint(MousePosition);

                        RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, MaxDistance);

                        if (hit)
                        {
                            if (hit.collider.gameObject.tag == "enemy")    //��Ʈ�� ������Ʈ�� �±װ� enemy�� ���
                            {
                                target = hit.collider.gameObject;       //Ÿ������ ����
                                if (fight)
                                {
                                    fsm.ChangeState(CharacterStates.Idle);
                                }
                                else
                                {
                                    tank_unitmovement.GoEnemy();
                                }
                            }
                        }
                        else
                        {
                            // ���� ���� �ƴϰ� ���� Ŭ���ϸ� Ÿ���� �ʱ�ȭ
                            if (!fight)
                            {
                                target = null;
                            }
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        tank_unitmovement.aClick = false;
                        aclick = false;
                        MousePosition = Input.mousePosition;
                        MousePosition = myCam.ScreenToWorldPoint(MousePosition);

                        RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, MaxDistance);
                        if (hit)
                        {
                            if (hit.collider.gameObject.tag == "enemy")    //��Ʈ�� ������Ʈ�� �±װ� enemy�� ���
                            {
                                target = hit.collider.gameObject;
                                if (fight)
                                {
                                    fsm.ChangeState(CharacterStates.Idle);
                                }
                            }
                        }
                        else
                        {
                            target = null;
                            fight = false;
                        }
                    }
                }
            }
        }
    }
    void Idle_Enter()
    {
        //player_move = false;
        //tank_unitmovement.PlayerStop();
    }
    void Idle_Update()
    {
        rigid2d.mass = 100;
        if (unit.nowHp > 0)
        {
            if (gameObject.GetComponent<Tank_UnitMovement>().arrived == false)
            {
                fsm.ChangeState(CharacterStates.Move);
            }

            // ��Ŭ������ �̵��ϴٰ� ������ �� �ֺ��� ���� ������ �ο�
            // ���� ����� ���� target���� �ٲ�� �ϴ°��� ��ǥ
            if (target == null && tank_unitmovement.arrived == true)
            {
                if (targetList.Count > 0)
                {
                    target = targetList[0];
                    fight = true;
                }
            }
            // ����µ� Ÿ���� �������� �i�ư���
            else
            {
                FoundTarget();
            }
            //�ڽĿ�����Ʈ Ʈ���� �ȿ� target�� ������
            if (fight)
            {
                if (currentTime >= DelayTime)
                {
                    if (on == false)
                    {
                        on = true;
                        StartCoroutine(Delay());
                    }
                }
            }
            else
            {
                if (gameObject.tag == "tankdps")
                {
                    if (unit.nowMp == unit.maxMp) // ����� ������ ������ �ִ� ���� ���佺Ʈ ��
                    {
                        StartCoroutine(skill.Tempest_Skill(this.gameObject));
                        unit.nowMp = 0;
                    }
                }
            }
        }

    }
    void Move_Enter()
    {
        on = false;
        player_move = true;
        //Debug.Log("�̵�");
        StopCoroutine(Attack());
    }

    void Move_Update()
    {
        rigid2d.mass = 1;
        if (gameObject.GetComponent<Tank_UnitMovement>().arrived == true)
        {
            fsm.ChangeState(CharacterStates.Idle);
        }
        //�̵��ϴ� ���߿��� �������� ��� �i�ư�
        if (target != null)
        {
            FoundTarget();
        }
        if (targetList.Count > 0)
        {
            if (tank_unitmovement.only_move == false)
            {
                player_move = false;
                tank_unitmovement.PlayerStop();
            }
        }
    }
    void Attack_Enter()
    {
        StartCoroutine(Attack());
    }
    void Attack_Update()
    {
        rigid2d.mass = 100;
        if (gameObject.GetComponent<Tank_UnitMovement>().arrived == false)
        {
            fsm.ChangeState(CharacterStates.Move);
        }
    }

    // target�� �ִµ� target�� �������� ����� �i�ư���
    void FoundTarget()
    {
        if (target != null)
        {
            bool foundenemy = false;
            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                if (target == targetList[i])
                {
                    foundenemy = true;
                    break;
                }
            }

            if (!foundenemy)
            {
                tank_unitmovement.GoEnemy();
            }
        }
    }

    IEnumerator Attack()
    {
        on = false;
        stop = true;
        // �����ϴ� �ð�
        yield return new WaitForSeconds(0.0f);
        if (player_move == false)
        {
            if (unit.maxMp != 0)
            {
                if (unit.nowMp >= unit.maxMp)
                {
                    StartCoroutine(skill.Tempest_Skill(this.gameObject));
                    attack_sword.Swing();
                    unit.nowMp = 0;
                }
                else
                {
                    attack_sword.Swing();
                }
            }
            else
            {
                attack_sword.Swing();
                if (gameObject.tag == "tankheal")
                {
                    unit.nowHp += 8;
                }
            }
            if (!stopMp)
            {
                unit.nowMp += 10;
                unit.stopMp = false;
            }
            else
            {
                unit.stopMp = true;
            }
            StartCoroutine(soundeffect.swordAtk2E(audiosource));
            // Ÿ���� �ִٸ� Ÿ���� �������� ����
            //if(target != null)
            //target.GetComponent<Enemy>().TakeDamage(unit.dmg);
            fsm.ChangeState(CharacterStates.Idle);
            yield return new WaitForSeconds(1.0f);
            stop = false;
        }
    }

    IEnumerator Delay()
    {
        startTime = Time.time;
        player_move = false;
        // �������� ���� �ð�
        fsm.ChangeState(CharacterStates.Attack);
        yield return 0;
    }



    void SwitchingTime() //unit��ũ��Ʈ���� attacking�� ��ȯ(������ Ȯ��)
    {
        if (currentTime >= DelayTime)
        {
            unit.attacking = false;
        }
        else
        {
            if (fight)
                unit.attacking = true;
        }
    }

    public void Idle()
    {
        fsm.ChangeState(CharacterStates.Idle);
    }

    private void OnDisable()
    {
        if (GetComponent<Unit>().usingSkill == true)
        {
            GetComponent<Unit>().usingSkill = false;
        }
    }
}

