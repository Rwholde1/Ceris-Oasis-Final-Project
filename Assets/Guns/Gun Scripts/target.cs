
using UnityEngine;

public class target : MonoBehaviour
{
    public float health = 50f;
    public int moneytogive = 0;
    public bool TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            
            Death();
            return true;
        }
        return false;
    }

    void Death()
    {

        Debug.Log("HasDied");
        gimmemoney();
        Destroy(gameObject);
    }
    public int gimmemoney()
    {
        return moneytogive;
    }
}
