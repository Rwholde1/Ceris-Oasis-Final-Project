using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class cloudmovement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public GameObject cloud;
    [SerializeField] public GameObject goal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cloud.transform.position = Vector3.MoveTowards(cloud.transform.position, goal.transform.position, speed);  
    }
}
