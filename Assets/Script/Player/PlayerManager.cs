using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerWeapon; // プレイヤーの武器を指定するためのGameObject変数
    private Vector3 originalScale; // 初期のスケールを保存する変数
    float x;
    float z;
    public float moveSpeed = 3;
    public Collider weaponCollider;
    public PlayerUIManager playerUIManager;
    public GameObject gameOverText;
    public Transform target;
    public int maxHp = 100;
    int hp;
    bool isDie;
    Rigidbody rb;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = playerWeapon.transform.localScale; // 初期のスケールを保存
        hp = maxHp;
        playerUIManager.Init(this);
        rb = GetComponent<Rigidbody>();
        animator =GetComponent<Animator>();
        HideColliderWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // 左Shiftキーが押されている間
        {
            moveSpeed = 8f; // 移動速度を6に設定
        }
        else
        {
            moveSpeed = 4f; // 通常の移動速度を設定
        }
        if (Input.GetKey(KeyCode.B)) // Bキーが押されている間
        {
            // 武器のYスケールを5倍に変更する
            Vector3 newScale = playerWeapon.transform.localScale;
            newScale.y = 5f;
            playerWeapon.transform.localScale = newScale;
        }
        else // Bキーが離された場合
        {
            // 武器のスケールを元のサイズに戻す
            playerWeapon.transform.localScale = originalScale;
        }
        if (isDie)
        {
            return;
        }
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LookAtTarget();
            Debug.Log("攻撃！");
            animator.SetTrigger("Attack");
        }
    }
    void LookAtTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if ( distance <= 2f)
        {
            transform.LookAt(target);
        }
    }
    private void FixedUpdate()
    {
        if (isDie)
        {
            return;
        }
        Vector3 direction = transform.position + new Vector3(x, 0, z) * moveSpeed;
        transform.LookAt(direction);
        rb.velocity = new Vector3(x, 0, z) * moveSpeed;
        animator.SetFloat("Speed", rb.velocity.magnitude);
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
            isDie = true;
            animator.SetTrigger("Die");
            gameOverText.SetActive(true);
            rb.velocity = Vector3.zero;
        }
        playerUIManager.UpdateHP(hp);
        Debug.Log("残りHP:"+hp);
    }
   private void OnTriggerEnter(Collider other)
    {
        if (isDie)
        {
            return;
        }
        EnemyDamager enemydamager = other.GetComponent<EnemyDamager>();
        if (enemydamager != null)
        {
            Debug.Log("己がダメージを受ける");
            animator.SetTrigger("Hurt");
            Damage(enemydamager.enemydamage);
        }
    }
}
