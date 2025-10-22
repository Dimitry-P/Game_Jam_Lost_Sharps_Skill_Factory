using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echoes_At_The_Last_Station
{
    public abstract class LevelCondition : MonoBehaviour
    {
        public virtual bool IsCompleted {  get; }    
    }
}

