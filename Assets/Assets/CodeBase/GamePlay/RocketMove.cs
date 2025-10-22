using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RocketMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
   

    private void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }  
}
