using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class MeteorIsMoving : MonoBehaviour
    {
        public float fallSpeed = 1f;
        private void Update()
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        }
    }
}

