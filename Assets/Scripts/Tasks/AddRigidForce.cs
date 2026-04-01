using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class AddRigidForce : ActionTask {
		public Vector3 direction;
		public Rigidbody rb;
		public bool groundOnly;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			if (groundOnly)
			{
				RaycastHit hit;
				if (Physics.SphereCast(agent.transform.position, 0.3f, Vector3.down, out hit, 1.2f) && rb.linearVelocity.y < 0.1f)
				{
                    rb.linearVelocity += direction;
                }
				else
				{
                    EndAction(false);
                }
			}
			else { rb.linearVelocity += direction; }

			EndAction(true);
		}
	}
}