using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace NodeCanvas.Tasks.Actions {

	public class EnemyChaseAttack : ActionTask {
		public AudioClip hurtAC, parryAC;
		public AudioSource auSo;
		public Transform playerTra;
		public Image hurtImg;

		protected override string OnInit() {
			return null;
		}

		protected override void OnExecute() {
			if (Vector3.Distance(agent.transform.position, playerTra.position) < 10f) {
				if (Input.GetMouseButton(1))
				{// parry
					auSo.clip = parryAC;
				}
				else
				{// hurt
					auSo.clip = hurtAC;
                    if (hurtImg.color.a > 0.15f)
                    {
                        SceneManager.LoadScene("Dead Scene", LoadSceneMode.Single);
                    }
                    hurtImg.color = new Color(1, 1, 1, 0.75f);
                }
				auSo.Play();
			}

			EndAction(true);
		}
	}
}