using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using Common;


namespace SpaceShooter
{
    public class MovementController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }

        [SerializeField] private SpaceShip m_TargetShip;

        [SerializeField] private VirtualJoyStick m_MobileJoystick;

        [SerializeField] private ControlMode m_ControlMode;


       

       
       
        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;
        public void SetTargetShipCollisionEdge(SpaceShip ship) => m_TargetShip = ship;

        // ƒва дополнительных пол€, которые будут непосредственно ссылатьс€ на наши скрипты
        [SerializeField] private PointerClickHold m_MobileFirePrimary; // нопка выстрела из главного оружи€
        [SerializeField] private PointerClickHold m_MobileFireSecondary; // нопка выстрела из дополнительного оружи€
        [SerializeField] private PointerClickHold m_MobileFireLeftRocket; //
        [SerializeField] private PointerClickHold m_MobileFireRightRocket; //
        


        private void Start()
        {
            m_TargetShip = transform.root.GetComponent<SpaceShip>();

            if (m_ControlMode == ControlMode.Keyboard)
            {
                m_MobileJoystick.gameObject.SetActive(false);

                m_MobileFirePrimary.gameObject.SetActive(false);
                m_MobileFireSecondary.gameObject.SetActive(false);
                m_MobileFireLeftRocket.gameObject.SetActive(false);
                m_MobileFireRightRocket.gameObject.SetActive(false);
            }
            else
            {
                m_MobileJoystick.gameObject.SetActive(true);

                m_MobileFirePrimary.gameObject.SetActive(true);
                m_MobileFireSecondary.gameObject.SetActive(true);
                m_MobileFireLeftRocket.gameObject.SetActive(true);
                m_MobileFireRightRocket.gameObject.SetActive(true);
            }
        }


        private void Update()
        {
          
            if (m_TargetShip == null) return;
            if (m_ControlMode == ControlMode.Keyboard) ControlKeyBoard();
            if (m_ControlMode == ControlMode.Mobile) ControlMobile();
        }

      

        private void ControlMobile()
        {
            var dir = m_MobileJoystick.Value;
            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;

            if (m_MobileFirePrimary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (m_MobileFireSecondary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }
            //if (m_MobileFireLeftRocket.IsHold == true)
            //{
            //    m_TargetShip.Fire(TurretMode.LeftRocket);
            //}
            //if (m_MobileFireRightRocket.IsHold == true)
            //{
            //    m_TargetShip.Fire(TurretMode.RightRocket);
            //}
        }

        public void ControlKeyBoard()
        {
            
            float thrust = 0;
            float torque = 0;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                thrust = 0.5f;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                thrust = 0;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                torque = 1.0f;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                torque = -1.0f;
            }


            if (Input.GetKey(KeyCode.Space))
            {
                    m_TargetShip.Fire(TurretMode.Primary);
            }
            if (Input.GetKey(KeyCode.X))
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                m_TargetShip.Fire(TurretMode.Third);
            }


            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;
            
            
        }
    }
}

//private void ControlMobile()
//{
//    Vector3 dir = m_MobileJoystick.Value;

//    var dot = Vector2.Dot(dir, m_TargetShip.transform.up);
//    var dot2 = Vector2.Dot(dir, m_TargetShip.transform.right);

//    m_TargetShip.ThrustControl = Mathf.Max(0, dot);
//    m_TargetShip.TorqueControl = -dot2;
//}