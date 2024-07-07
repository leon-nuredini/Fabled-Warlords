/// <summary>
/// Copyright 2017 Ramiro Oliva (Kronnect) - All rights reserved
/// </summary>
using UnityEngine;
using System;

namespace LBAOFX {
				[ExecuteInEditMode, RequireComponent (typeof(Camera))]
				[AddComponentMenu ("Image Effects/Rendering/LBAO")]
				[HelpURL ("http://kronnect.com/taptapgo")]
				[ImageEffectAllowedInSceneView]
				public class LBAO : MonoBehaviour {
								[Range (0.01f, 0.5f)]
								public float threshold = 0.1f;

								[Range (2f, 32f)]
								public float radius = 12.0f;

								[Range (2, 15)]
								public int samples = 8;

								[Range (0, 1f)]
								public float intensity = 0.5f;

								[Range (0f, 1f)]
								public float lumaProtect = 0.1f;

								public bool blur = true;

								public bool showAO;

								public bool directional = false;

								public Vector2 direction = new Vector2(-1,1);

								[Range (1, 4)]
								public int downsampling = 1;

								public static LBAO instance { 
												get { 
																if (_instance == null) {
																				foreach (Camera camera in Camera.allCameras) {
																								_instance = camera.GetComponent<LBAO> ();
																								if (_instance != null)
																												break;
																				}
																}
																return _instance;
												} 
								}




								// Internal stuff **************************************************************************************************************

								const string SKW_DEBUG = "LBAO_DEBUG_ON";
								const string SKW_BLUR = "LBAO_BLUR_ON";
								const string SKW_DIRECTIONAL = "LBAO_DIRECTIONAL";
								static LBAO _instance;

								[SerializeField]
								Material mat;

								void OnEnable () {
												Reset ();
								}

								void Reset () {
												if (mat == null)
																Init ();
												UpdateMaterialProperties ();
								}

								void Init () {
												mat = Instantiate (Resources.Load<Material> ("Materials/LBAO") as Material);
								}

								void OnRenderImage (RenderTexture source, RenderTexture destination) {
												if (mat == null) {
																Graphics.Blit (source, destination);
																return;
												}

												int width = source.width / downsampling;
												int height = source.height / downsampling;
												RenderTexture rt = source;

												if (downsampling > 1 || blur) {
																rt = RenderTexture.GetTemporary (width, height, 0, RenderTextureFormat.ARGB32);
																Graphics.Blit (source, rt, mat, 0);
												}

												if (blur) {
																RenderTexture blurH = RenderTexture.GetTemporary (rt.width, rt.height, 0, RenderTextureFormat.ARGB32);
																Graphics.Blit (rt, blurH, mat, 1);
																source.DiscardContents ();
																Graphics.Blit (blurH, rt, mat, 2);
																mat.SetTexture ("_BlurTex", rt);
																Graphics.Blit (source, destination, mat, 3);
																RenderTexture.ReleaseTemporary (blurH);
												} else {
																Graphics.Blit (rt, destination, mat, 0);
												}

												if (rt != source) {
																RenderTexture.ReleaseTemporary (rt);
												}
								}

								public void UpdateMaterialProperties () {
												if (mat == null)
																return;
												mat.SetFloat ("_Threshold", threshold);
												mat.SetInt ("_Samples", samples);
												mat.SetFloat ("_Radius", radius);
												mat.SetFloat ("_Intensity", intensity);
												mat.SetFloat ("_LumaProtect", 1f - lumaProtect);
												mat.SetFloat ("_BlurSpread", Mathf.Max(radius / 16f, 1f));
												mat.SetVector("_Direction", direction.normalized);
												if (showAO) {
																mat.EnableKeyword (SKW_DEBUG);
												} else {
																mat.DisableKeyword (SKW_DEBUG);
												}
												if (blur) {
																mat.EnableKeyword (SKW_BLUR);
												} else {
																mat.DisableKeyword (SKW_BLUR);
												}
												if (directional) {
																mat.EnableKeyword (SKW_DIRECTIONAL);
												} else {
																mat.DisableKeyword (SKW_DIRECTIONAL);
												}
								}


				}

}