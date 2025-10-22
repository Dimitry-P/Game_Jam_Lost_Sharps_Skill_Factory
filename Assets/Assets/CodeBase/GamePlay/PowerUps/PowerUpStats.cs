using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//описывает поведение бонуса, который подбирается кораблём 
//и добавляет ему либо энергию, либо боезапас, в зависимости от настроек.
//Для чего нужен скрипт:
//Чтобы легко добавлять бонусы в игру.

//Чтобы разные бонусы имели разное поведение, но работали по единой системе.

//Чтобы при подборе бонуса корабль автоматически усиливался.
namespace SpaceShooter
{
    public class PowerUpStats : PowerUp
    {
        public enum EffectType
        {
            AddAmmo,
            AddEnergy
        }

        [SerializeField] private EffectType m_EffectType;
        [SerializeField] private float m_Value; //Значение. Сколько мы прибавляем


        //Этот метод срабатывает, когда корабль подбирает бонус.
        protected override void OnPickedUp(SpaceShip ship)
        {
            if (m_EffectType == EffectType.AddEnergy)
                ship.AddEnergy((int) m_Value);

            if (m_EffectType == EffectType.AddAmmo)
                ship.AddAmmo((int) m_Value);

        }
    }
}

