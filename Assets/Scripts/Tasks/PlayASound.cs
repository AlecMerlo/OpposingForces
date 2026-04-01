using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions {

	public class PlayASound : ActionTask {
		public AudioSource auSo;
		public float timer;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			auSo.Play();
			timer = auSo.clip.length * 0.75f;
        }

		protected override void OnUpdate() {
			timer -= Time.deltaTime;
			if (timer < 0)
			{
                EndAction(true);
            }
		}
	}
}