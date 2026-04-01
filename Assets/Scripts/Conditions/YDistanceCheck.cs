using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

	public class YDistanceCheck : ConditionTask {

        public Transform otherTra;
        public int distance;

        protected override string OnInit()
        {
            return null;
        }

        protected override bool OnCheck()
        {
            return otherTra.position.y > distance;
        }
    }
}