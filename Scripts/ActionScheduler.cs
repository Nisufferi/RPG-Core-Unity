using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if(currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }
    }
}
