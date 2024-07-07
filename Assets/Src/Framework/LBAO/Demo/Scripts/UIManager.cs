using UnityEngine;
using System.Collections;

namespace LBAOFX {
				public class UIManager : MonoBehaviour {

								public GameObject[] carrousel;

								int index = 0;

								void Update () {
												if (Input.GetMouseButtonDown (0))
																ToggleEffect ();
												if (Input.GetKeyDown (KeyCode.Space))
																TogglePicture ();
												if (Input.GetKeyDown (KeyCode.A))
																ToggleAO ();
								}

								public void ToggleEffect () {
												LBAO.instance.enabled = !LBAO.instance.enabled;
								}


								public void TogglePicture () {
												if (carrousel == null)
																return;
												index++;
												if (index >= carrousel.Length)
																index = 0;
												for (int k = 0; k < carrousel.Length; k++) {
																carrousel [k].SetActive (k == index);
												}
								}

								public void ToggleAO () {
												LBAO.instance.enabled = true;
												LBAO.instance.showAO = !LBAO.instance.showAO;
												LBAO.instance.UpdateMaterialProperties ();
								}

				}
}