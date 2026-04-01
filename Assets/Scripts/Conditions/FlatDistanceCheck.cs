using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions {

	public class FlatDistanceCheck : ConditionTask {
		public Transform otherTra;
		public int distance;
		
		protected override string OnInit(){
			return null;
		}

		protected override bool OnCheck() {
			Vector3 myGroundedPos, otherGroundedPos;
			myGroundedPos = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
			otherGroundedPos = new Vector3(otherTra.position.x, 0, otherTra.position.z);
			return Vector3.Distance(myGroundedPos, otherGroundedPos) > distance;
		}
	}
}