using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class MatchVelocity : ActionTask {
		public Rigidbody playerRigid, myRigid;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			myRigid.linearVelocity = playerRigid.linearVelocity;
			EndAction(true);
		}
	}
}