using UnityEngine;

public class Witcher : EnemyScript, IEnemy
{
#pragma warning disable CS0649 // Полю "Witcher.FireBall" нигде не присваивается значение, поэтому оно всегда будет иметь значение по умолчанию null.
    GameObject FireBall;
#pragma warning restore CS0649 // Полю "Witcher.FireBall" нигде не присваивается значение, поэтому оно всегда будет иметь значение по умолчанию null.

    public override void Start()
    {
        base.Start();
        Enemy = this;
    }

    public void Attack()
    {
        Rigidbody rb = Instantiate(FireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
    }

    public void SpecialPower()
    {
        Debug.Log(gameObject.name + " has used Special Power!");

        Rigidbody rb = Instantiate(FireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
    }
}

