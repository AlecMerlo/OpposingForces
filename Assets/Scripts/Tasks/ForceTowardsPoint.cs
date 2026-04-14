using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Unity.VisualScripting;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class ForceTowardsPoint : ActionTask {
		public Rigidbody rb;
		public Transform target;
		public float speed;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			Vector3 wantedDir;
			wantedDir = -(new Vector3(agent.transform.position.x, 0, agent.transform.position.z) 
				        - new Vector3(target.position.x, 0, target.position.z)).normalized;

            rb.linearVelocity = (wantedDir * speed * Time.deltaTime * Vector3.Distance(agent.transform.position, target.position*1.6f)) + (rb.linearVelocity.y * Vector3.up);

            EndAction(true);
		}
	}
}