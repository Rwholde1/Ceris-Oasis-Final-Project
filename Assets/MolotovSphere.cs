using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MolotovSphere : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float damagepersecond;
    private int moneytosendtoplayer;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    private void OnTriggerStay(Collider other)
    {
         target enemy = other.gameObject.GetComponent<target>();
        if(other.GetComponent<target>() != null) 
        { 
        StartCoroutine(doDamage(enemy));
        }
    }
    private IEnumerator doDamage(target enemy)
    {
        if (enemy.TakeDamage(damagepersecond/4))
        {
            moneytosendtoplayer = enemy.gimmemoney();
            sendmoney();
        }
        else
        {

            yield return new WaitForSeconds(0.25f);
        }

    }
    void sendmoney()
    {

        player.GetComponentInParent<AddMoney>().money += moneytosendtoplayer;
        moneytosendtoplayer = 0;
    }
}
