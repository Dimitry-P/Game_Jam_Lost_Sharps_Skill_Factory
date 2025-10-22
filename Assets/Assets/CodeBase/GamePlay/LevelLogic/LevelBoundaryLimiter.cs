using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace SpaceShooter
{
    public class LevelBoundaryLimiter : MonoBehaviour
    {

        /// <summary>
        /// Ограничитель позиции. Работает в связке со скриптом LevelBoundary если таковой имеется на сцене
        /// </summary>
        /// 




        private void Update()
        {
            if (LevelBoundary.Instance == null) return;

            var size = LevelBoundary.Instance.SizeForLBLimiter;

            Vector3 pos = transform.position;
            Debug.Log("Позиция: " + transform.position + " / Размеры ограничителя: " + size);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float length = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y;



            if (
                     Mathf.Abs(transform.position.x) > (size.x / 2f - length/2f) ||
                     transform.position.y > (size.y / 2f - length / 2f) ||
                     transform.position.y < (-size.y + 398f + length / 2f)
                 )
            {
                if (LevelBoundary.Instance.LimitMode == LevelBoundary.Mode.Limit)
                {
                    // Ограничиваем по X
                    if (Mathf.Abs(pos.x) > size.x / 2f)
                        pos.x = Mathf.Sign(pos.x) * size.x / 2f;

                    // Ограничиваем по Y
                    if (Mathf.Abs(pos.y) > size.y / 2f)
                        pos.y = Mathf.Sign(pos.y) * size.y / 2f;
                }
                if (LevelBoundary.Instance.LimitMode == LevelBoundary.Mode.Teleport)
                {
                    // Телепортируем по X
                    if (Mathf.Abs(pos.x) > size.x / 2f)
                        pos.x = -pos.x;

                    // Телепортируем по Y
                    if (Mathf.Abs(pos.y) > size.y / 2f)
                        pos.y = -pos.y;
                }
                if (LevelBoundary.Instance.LimitMode == LevelBoundary.Mode.Death)
                {
                    GetComponent<Destructible>().ApplyDamage(999);
                    return;
                }
                transform.position = pos;
            }
        }
    }
}

