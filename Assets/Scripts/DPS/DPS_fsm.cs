using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
public class DPS_fsm : MonoBehaviour
{
    public GameObject sacredEffect;
    public Attack_Bow attack_bow;
    public UnitMovement unitmovement;
    public GameObject target;
    public List<GameObject> targetList = new List<GameObject>();
    private Camera myCam;
    float MaxDistance = 15f;
    Vector3 MousePosition;
    public bool aclick = false;
    public bool fight = false;
    float atkspeed;
    public float DelayTime = 4f;
    bool on = false;
    bool player_move = false;
    float startTime = 0;
    public float currentTime = 10f;
    //bool isTarget = false;
    Unit unit;
    Control control;
    Skill skill;
    public List<GameObject> sacredBuff = new List<GameObject>();
    SoundEffect soundeffect;
    AudioSource audiosource;
    AudioSource Se;
    Rigidbody2D rigid2d;
    public bool stop = false;
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
        control = GameObject.Find("Object_control").GetComponent<Control>();
        skill = GameObject.Find("Object_control").GetComponent<Skill>();
        myCam = Camera.main;
        unit = GetComponent<Unit>();
        unitmovement = gameObject.GetComponent<UnitMovement>();
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
            if (unitmovement.selected)
            {
                if (aclick == true)
                {
                    if (Input.GetMouseButton(0))
                    {
                        unitmovement.aClick = false;
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
                                    unitmovement.GoEnemy();
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
                else //aŬ���� ������ ���
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        unitmovement.aClick = false;
                        aclick = false;
                        MousePosition = Input.mousePosition;
                        MousePosition = myCam.ScreenToWorldPoint(MousePosition);

                        RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, MaxDistance);
                        if (hit)
                        {
                            if (hit.collider.gameObject.tag == "enemy")    //��Ʈ�� ������Ʈ�� �±װ� enemy�� ���
                            {
                                target = hit.collider.gameObject;       //Ÿ������ ����
                                //if (targetList.Count > 0 && unitmovement.only_move == false)
                                //{
                                //    fsm.ChangeState(CharacterStates.Idle);
                                //}
                            }
                        }
                        else // ��Ŭ��
                        {
                            target = null;
                            fight = false;
                        }
                    }
                }
            }
        }
    }


    //--------------------------------------------------------------------------
    void Idle_Enter()
    {
        //player_move = false;
        //unitmovement.PlayerStop();
    }
    void Idle_Update()
    {
        rigid2d.mass = 100;
        if (unit.nowHp > 0)
        {
            if (gameObject.GetComponent<UnitMovement>().arrived == false)
            {
                fsm.ChangeState(CharacterStates.Move);
            }

            // ��Ŭ������ �̵��ϴٰ� ������ �� �ֺ��� ���� ������ �ο�
            // ���� ����� ���� target���� �ٲ�� �ϴ°��� ��ǥ
            if (target == null)
            {
                if (targetList.Count > 0 && unitmovement.arrived == true)
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
                if (gameObject.tag == "dpsheal") // Ranger�� ������ �ؾ� �ϴ� ��ų�̱� ������ ��ũ���常 ����
                {
                    if (unit.nowMp == unit.maxMp)
                    {
                        StartCoroutine(Skill());
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
    }

    void Move_Update()
    {
        if (target == null)
        {
            if (targetList.Count > 0 && unitmovement.arrived == true)
            {
                target = targetList[0];
                fight = true;
            }
        }
        rigid2d.mass = 1;
        if (gameObject.GetComponent<UnitMovement>().arrived == true)
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
            if (unitmovement.only_move == false)
            {
                player_move = false;
                unitmovement.PlayerStop();
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
        if (gameObject.GetComponent<UnitMovement>().arrived == false && stop == false)
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
                unitmovement.GoEnemy();
            }
        }
    }

    IEnumerator Attack()
    {
        on = false;
        stop = true;
        // �����ϴ� �ð�
        if (player_move == false)
        {
            if (unit.maxMp != 0)
            {
                if (unit.nowMp == unit.maxMp)
                {
                    StartCoroutine(Skill());
                }
                else // ������ �ִµ� �� ���� �ʾ��� ��
                {
                    yield return new WaitForSeconds(1.0f);
                    if (target != null)
                    {
                        if (gameObject.tag == "dpstank") //������
                        {
                            attack_bow.Penetrate_Shoot();
                        }
                        else // �׿� ������
                        {
                            attack_bow.Shoot(1);
                            StartCoroutine(soundeffect.arrowE(audiosource));
                        }
                    }
                }
            }
            else // ������ ���� ��
            {
                yield return new WaitForSeconds(1.0f);
                if (target != null)
                {
                    if (gameObject.tag == "dpstank")
                    {
                        attack_bow.Penetrate_Shoot();
                    }
                    else
                    {
                        attack_bow.Shoot(1);
                        StartCoroutine(soundeffect.arrowE(audiosource));
                    }
                }
            }
            unit.durability += 1;
            Idle();
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
        stop = false;
    }

    IEnumerator Skill()
    {
        unit.nowMp = 0;
        if (gameObject.tag == "dpsdps")
        {
            StartCoroutine(skill.Ranger_Skill(attack_bow));
        }
        else if (gameObject.tag == "dpsheal")
        {
            StartCoroutine(skill.Sacred_Skill(sacredBuff));
            GameObject Effect1 = Instantiate(sacredEffect);
            Effect1.GetComponent<Transform>().position = new Vector3(gameObject.GetComponent<Transform>().position.x, gameObject.GetComponent<Transform>().position.y + 3, 0);
            Effect1.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<MeshRenderer>().sortingOrder;
            StartCoroutine(unit.DmgBuff());
            GetComponent<Unit>().usingSkill = true;
            yield return new WaitForSeconds(1.0f);
            GetComponent<Unit>().usingSkill = false;
        }
    }

    private void OnDisable()
    {
        aclick = false;
        if (GetComponent<Unit>().usingSkill == true)
        {
            GetComponent<Unit>().usingSkill = false;
        }
    }
}