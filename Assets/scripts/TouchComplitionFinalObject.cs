
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echoes_At_The_Last_Station
{
    public class TouchComplitionFinalObject : LevelCondition
    {
        public override bool IsCompleted
        {
            get 
            {
                if (TouchFinalObject.Instance.EnteredCollider == true)
                {
                    return true;
                }
                return false;
            }
        }
    }
}

