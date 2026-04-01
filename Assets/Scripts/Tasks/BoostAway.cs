using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class BoostAway : ActionTask {
		public Transform playerTra;
		public Rigidbody rb;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			Vector3 flatPlayerPos, flatPos;
			flatPlayerPos = new Vector3(playerTra.position.x, 0, playerTra.position.z);
			flatPos = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);

            rb.linearVelocity = ((flatPos - flatPlayerPos).normalized * 100) + (Vector3.up * 20);

            EndAction(true);
		}
	}
}