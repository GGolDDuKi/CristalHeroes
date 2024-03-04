using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
public class Heal_fsm : MonoBehaviour
{
    public GameObject reviveEffect;
    float atkspeed;
    Control control;
    public Attack_Wand attack_wand;
    public Heal_Unitmovement heal_unitmovement;
    public GameObject target;
    public List<GameObject> targetList = new List<GameObject>();
    private Camera myCam;
    float MaxDistance = 15f;
    Vector3 MousePosition;
    public bool aclick = false;
    public bool fight = false;
    public bool on = false;
    public bool player_move = false;
    public float DelayTime = 4f;
    float startTime = 0;
    public float currentTime = 0;
    Unit unit;
    Skill skill;
    public List<GameObject> crusaderHeal = new List<GameObject>();
    SoundEffect soundeffect;
    AudioSource audiosource;
    AudioSource Se;
    Rigidbody2D rigid2d;
    public bool stop = false;
    bool dealbuff = false;
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
        heal_unitmovement = gameObject.GetComponent<Heal_Unitmovement>();
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
            if (heal_unitmovement.selected)
            {
                if (aclick == true) //a�� ��������
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        heal_unitmovement.aClick = false;
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
                                    heal_unitmovement.GoEnemy();
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
                else // �ȴ�������
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        heal_unitmovement.aClick = false;
                        aclick = false;
                        MousePosition = Input.mousePosition;
                        MousePosition = myCam.ScreenToWorldPoint(MousePosition);

                        RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, MaxDistance);
                        if (hit)
                        {
                            if (hit.collider.gameObject.tag == "enemy")    //��Ʈ�� ������Ʈ�� �±װ� enemy�� ���
                            {
                                target = hit.collider.gameObject;
                                //if (fight)
                                //{
                                //    fsm.ChangeState(CharacterStates.Idle);
                                //}
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
        //heal_unitmovement.PlayerStop();
    }
    void Idle_Update()
    {
        rigid2d.mass = 100;
        if (unit.nowHp > 0)
        {
            if (gameObject.GetComponent<Heal_Unitmovement>().arrived == false)
            {
                fsm.ChangeState(CharacterStates.Move);
            }

            // ��Ŭ������ �̵��ϴٰ� ������ �� �ֺ��� ���� ������ �ο�
            // ���� ����� ���� target���� �ٲ�� �ϴ°��� ��ǥ
            if (target == null && heal_unitmovement.arrived == true)
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

                if (unit.nowMp == unit.maxMp)
                {
                    unit.nowMp = 0;
                    StartCoroutine(Skill());
                }
            }
        }
    }
    void Move_Enter()
    {
        on = false;
        player_move = true;
        StopCoroutine(Attack());
    }

    void Move_Update()
    {
        rigid2d.mass = 1;
        if (heal_unitmovement.arrived == true)
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
            if (heal_unitmovement.only_move == false)
            {
                player_move = false;
                heal_unitmovement.PlayerStop();
            }
            if (heal_unitmovement.arrived == true)
            {
                fsm.ChangeState(CharacterStates.Idle);
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
        if (gameObject.GetComponent<Heal_Unitmovement>().arrived == false)
        {
            fsm.ChangeState(CharacterStates.Move);
        }
    }

    // target�� �ִµ� target�� �������� ����� �i�ư���
    void FoundTarget()
    {
        bool foundenemy = false;
        if (target != null)
        {
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
                heal_unitmovement.GoEnemy();
            }
        }
    }

    IEnumerator Attack()
    {
        stop = true;
        on = false;
        if (player_move == false)
        {
            if (unit.nowMp == unit.maxMp)
            {
                if (gameObject.tag == "healheal")//����� ���� �Ʊ��� ������ ��ų�� ���� �ʱ� ������ ��
                {
                    if (skill.death.Count > 0)
                    {
                        StartCoroutine(soundeffect.reviveE(audiosource));
                        GetComponent<Unit>().usingSkill = true;
                        StartCoroutine(skill.Bishop_Skill());
                        unit.nowMp = 0;
                        GameObject effect = Instantiate(reviveEffect);
                        effect.GetComponent<Transform>().position = new Vector3(gameObject.GetComponent<Transform>().position.x, gameObject.GetComponent<Transform>().position.y + 1.5f, 0);
                        yield return new WaitForSeconds(1.0f);
                        GetComponent<Unit>().usingSkill = false;
                    }
                    else //���� ����� ������ �Ϲ� ����
                    {
                        attack_wand.Shoot();
                    }
                }
                else
                {
                    StartCoroutine(Skill());
                }
            }
            else
            {
                if (dealbuff == true)
                {
                    unit.dmg += 10;
                }
                yield return new WaitForSeconds(1.0f); //�ִϸ��̼� ���
                if (target != null)
                {
                    if (gameObject.tag == "healtank")
                        attack_wand.Swing();
                    else
                        attack_wand.Shoot();
                    if (dealbuff == true)
                    {
                        unit.dmg -= 10;
                        dealbuff = false;
                    }
                }
            }
            Healer_durability();
            stop = false;
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

    void Healer_durability()
    {
        if (unit.healOneTime == false)
        {
            unit.healOneTime = true;
        }
        else
        {
            unit.healOneTime = false;
            unit.durability += 1;
        }
    }

    public void Idle()
    {
        fsm.ChangeState(CharacterStates.Idle);
    }

    IEnumerator Skill()
    {
        if (gameObject.tag == "heal")
        {
            GetComponent<Unit>().usingSkill = true;
            StartCoroutine(soundeffect.healE(audiosource));
            StartCoroutine(skill.Heal_Skill());
            unit.nowMp = 0;
            yield return new WaitForSeconds(1.2f);
            GetComponent<Unit>().usingSkill = false;
        }
        else if (gameObject.tag == "healtank") 
        {
            StartCoroutine(soundeffect.healE(audiosource));
            GetComponent<Unit>().usingSkill = true;
            StartCoroutine(skill.Crusader_Skill(crusaderHeal));
            unit.nowMp = 0;
            unit.nowHp += 6;
            yield return new WaitForSeconds(1.0f);
            GetComponent<Unit>().usingSkill = false;
        }
        else if (gameObject.tag == "healdps") //��Ŭ����
        {
            StartCoroutine(soundeffect.healE(audiosource));
            GetComponent<Unit>().usingSkill = true;
            StartCoroutine(skill.Eclipse_Skill());
            unit.nowMp = 0;
            yield return new WaitForSeconds(1.0f);
            GetComponent<Unit>().usingSkill = false;
            dealbuff = true;
        }
        else if (gameObject.tag == "healheal")
        {
            if (skill.death.Count > 0)
            {
                StartCoroutine(soundeffect.reviveE(audiosource));
                GetComponent<Unit>().usingSkill = true;
                StartCoroutine(skill.Bishop_Skill());
                unit.nowMp = 0;
                GameObject effect = Instantiate(reviveEffect);
                effect.GetComponent<Transform>().position = new Vector3(gameObject.GetComponent<Transform>().position.x, gameObject.GetComponent<Transform>().position.y + 1.5f, 0);
                yield return new WaitForSeconds(1.0f);
                GetComponent<Unit>().usingSkill = false;
            }
        }
    }

    private void OnDisable()
    {
        if(GetComponent<Unit>().usingSkill == true)
        {
            GetComponent<Unit>().usingSkill = false;
        }
    }
}

