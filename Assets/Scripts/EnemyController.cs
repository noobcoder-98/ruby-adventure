using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public AudioClip hitForEnemy;
    public float speed = 2.0f;
    public float changeTime = 3.0f;
    public bool vertical;
    public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2D;
    Animator animator;
    float timer;
    int direction = 1;

    bool broken = true;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
    }

    private void Update() {
        if (!broken) return;

        timer -= Time.deltaTime;
        if (timer < 0) {
            direction = -direction;
            timer = changeTime;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken) return;

        Vector2 position = rigidbody2D.position;
        if (vertical) {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        } else {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        RubyController player = other.gameObject.GetComponent<RubyController>();
        if (player != null) {
            player.ChangeHealth(-1);
            player.PlaySound(hitForEnemy);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");

        smokeEffect.Stop();
    }
}
