using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public Transform target;
    public int maxHp = 100;
    int hp = 100;
    NavMeshAgent agent;
    Animator animator;
    public EnemyUIManager enemyUIManager;
    public Collider weaponCollider;
    public GameObject gameClearText;
    void Start()
    {
        hp = maxHp;
        enemyUIManager.Init(this);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
        HideColliderWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
        animator.SetFloat("Distance", agent.remainingDistance);
    }
    public void LookAtTarget()
    {
        transform.LookAt(target);
    }
   public void HideColliderWeapon()
    {
        weaponCollider.enabled = false;
    }
    public void ShowColliderWeapon()
    {
        weaponCollider.enabled = true;
    }
    void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        { 
            hp = 0;
            animator.SetTrigger("Die");
            gameClearText.SetActive(true);
        }
        enemyUIManager.UpdateHP(hp);
        Debug.Log("敵の残りHP:"+hp);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerDamager playerdamager = other.GetComponent<PlayerDamager>();
        if (playerdamager != null)
        {
            Debug.Log("敵がダメージを受ける");
            animator.SetTrigger("Hurt");
            Damage(playerdamager.playerdamage);
        }
    
    }
}
