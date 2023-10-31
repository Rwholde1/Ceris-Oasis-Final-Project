using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAttackRate : MonoBehaviour
{
    public float increase = 5.0f;
    [SerializeField] private GameObject boomerangController;
    private MoveTowardMiddle boomerang;

    // Start is called before the first frame update
    void Start()
    {
        boomerang = GetComponent<MoveTowardMiddle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Boomerang")
        {
            boomerang.AttackRateIncrease(increase);
            //maybe call method to add image to set of powerups / increase number on one power-up
            Destroy(gameObject);
        }
    }
}
