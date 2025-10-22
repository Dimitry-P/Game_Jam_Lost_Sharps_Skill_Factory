using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class AutoDestroyBlastFire : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject, 2.5f);
        }
    }
}



