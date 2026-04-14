using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class RunAway : ActionTask {
		public Rigidbody rb;
		public Transform playerTra;
		public int speed;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			Vector3 pTra = new Vector3(playerTra.position.x, 0, playerTra.position.z);
			Vector3 rbTra = new Vector3(rb.transform.position.x, 0, rb.transform.position.z);
			rb.AddForce(-(pTra - rbTra).normalized * 400 * Time.deltaTime, ForceMode.Impulse);
			rb.maxLinearVelocity = speed;

			EndAction(true);
		}
	}
}