/// <summary>
/// LBAO effect inspector. Copyright 2017 Ramiro Oliva (Kronnect) - All rights reserved
/// </summary>
using UnityEngine;
using UnityEditor;
using System;


namespace LBAOFX {
				[CustomEditor (typeof(LBAO))]
				public class LBAOInspector : Editor {
								LBAO _effect;
								GUIStyle titleLabelStyle;
								SerializedProperty radius, threshold, samples, intensity, blur, downsampling, lumaProtect, showAO, directional, direction;

								void OnEnable () {
												_effect = (LBAO)target;

												radius = serializedObject.FindProperty ("radius");
												threshold = serializedObject.FindProperty ("threshold");
												samples = serializedObject.FindProperty ("samples");
												intensity = serializedObject.FindProperty ("intensity");
												blur = serializedObject.FindProperty ("blur");
												downsampling = serializedObject.FindProperty ("downsampling");
												lumaProtect = serializedObject.FindProperty ("lumaProtect");
												showAO = serializedObject.FindProperty ("showAO");
												directional = serializedObject.FindProperty ("directional");
												direction = serializedObject.FindProperty ("direction");

								}

								public override void OnInspectorGUI () {
												if (_effect == null)
																return;

												#if UNITY_5_6_OR_NEWER
												serializedObject.UpdateIfRequiredOrScript ();
												#else
												serializedObject.UpdateIfDirtyOrScript ();
												#endif

												// draw interface
												EditorGUILayout.Separator ();
												EditorGUILayout.BeginHorizontal ();
												DrawLabel ("Luma Based Ambient Occlusion - Settings");
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.PropertyField (samples, new GUIContent ("Sample Count", "Number of image samples used to simulate occlusion"));
												EditorGUILayout.PropertyField (radius, new GUIContent ("Radius", "Sampling radius"));
												EditorGUILayout.PropertyField (threshold, new GUIContent ("Threshold", "Luma threshold"));
												EditorGUILayout.PropertyField (directional, new GUIContent ("Directional", "Forces occlusion in one specific direction"));
												if (directional.boolValue) {
																EditorGUILayout.PropertyField (direction, new GUIContent ("Direction"));
												}
												EditorGUILayout.PropertyField (downsampling, new GUIContent ("Downsampling", "AO and blur buffers downsampling"));
												EditorGUILayout.PropertyField (blur, new GUIContent ("Blur", "Blur AO buffer"));
												EditorGUILayout.PropertyField (lumaProtect, new GUIContent ("Luma Protection", "Prevents occlusion on bright pixels"));
												EditorGUILayout.PropertyField (intensity, new GUIContent ("Intensity", "Final occlusion intensity"));
												EditorGUILayout.PropertyField (showAO, new GUIContent ("Show AO", "Debug mode: show computed ambient occlusion"));

												if (serializedObject.ApplyModifiedProperties () || (Event.current.type == EventType.ExecuteCommand &&
												    Event.current.commandName == "UndoRedoPerformed")) {
																((LBAO)target).UpdateMaterialProperties ();
												}


								}

								void DrawLabel (string s) {
												if (titleLabelStyle == null) {
																GUIStyle skurikenModuleTitleStyle = "ShurikenModuleTitle";
																titleLabelStyle = new GUIStyle (skurikenModuleTitleStyle);
																titleLabelStyle.contentOffset = new Vector2 (5f, -2f);
																Color titleColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color (0.12f, 0.16f, 0.4f);
																titleLabelStyle.normal.textColor = titleColor;
																titleLabelStyle.fixedHeight = 22;
																titleLabelStyle.fontStyle = FontStyle.Bold;
												}

												GUILayout.Label (s, titleLabelStyle);
								}

				}

}
