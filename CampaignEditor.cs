using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CampaignEditor : EditorWindow {

	Campaign c;
	CampaignDisplay CD;

	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/Campaign Editor")]
	[MenuItem("TTP/Campaign Editor")]
	public static void ShowWindow() {
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(CampaignEditor));
		
	}


	void OnGUI() {
		CD = (CampaignDisplay)EditorGUILayout.ObjectField("Campaign display:",CD, typeof(CampaignDisplay), true);

		if (CD == null) return;

		c = CD.campaign;

		GUILayout.Label ("Missions in this campaign:");

		GUI.enabled = c.missions.Length != 0;
		if (GUILayout.Button ("Remove mission")) {
			Mission[] m = new Mission[c.missions.Length-1];
			for (int i = 0; i < m.Length; i++) {
				m[i] = c.missions[i];
			}
			c.missions[c.missions.Length - 1] = new Mission();
			c.missions = m;
		}
		GUI.enabled = true;

		foreach (Mission m in c.missions) {
			drawMission(m);
		}

		if (GUILayout.Button ("Add mission")) {
			Mission[] m = new Mission[c.missions.Length+1];
			for (int i = 0; i < c.missions.Length; i++) {
				m[i] = c.missions[i];
			}
			c.missions = m;
			c.missions[c.missions.Length-1] = new Mission();
			c.missions[c.missions.Length-1].missionName = "NEW MISSION";
		}

		CD.campaign = c;
		if (GUI.changed)
			EditorUtility.SetDirty(CD);
	}
	
	public Objective getOBJfromSel() {
		foreach (GameObject go in Selection.gameObjects) {
			if (go.GetComponent<Objective>() != null) {
				return go.GetComponent<Objective>();
			}
		}
		return null;
	}
	
	public void drawMission (Mission m) {
		m.missionName = GUILayout.TextField (m.missionName);
		GUILayout.BeginHorizontal();
		GUILayout.Space(15);
		GUILayout.Label ("Objectives in this mission:");

		if (GUILayout.Button ("Add objective")) {
			if (getOBJfromSel() != null) {
				Debug.Log("Attempting to add objective...");
				
				Objective[] o = new Objective[m.objectives.Length+1]; // This sometimes doesn't function properly.

				if (o.Length != 1) {
					for (int i = 0; i < m.objectives.Length; i++) {
						o [i] = m.objectives [i];
					}
				}

				m.objectives = (Objective[])o.Clone();
				m.objectives[m.objectives.Length-1] = getOBJfromSel();
				Debug.Log("Done adding objective " +
				    ((m.objectives[m.objectives.Length-1] != null) ?
					(m.objectives[m.objectives.Length-1].objectiveName + " @ " +
					 m.objectives[m.objectives.Length-1].name)
					: "NULL OBJECTIVE"));

			} else {
				Debug.Log("In order to add an objective, you must have one highlighted in scene or hiearchy view.");
			}
		}




		GUI.enabled = m.objectives.Length != 0;
		if (GUILayout.Button ("Remove objective")) {
			Objective[] o = new Objective[m.objectives.Length-1];
			for (int i = 0; i < o.Length; i++) {
				o[i] = m.objectives[i];
			}
			m.objectives = o;
		}
		GUI.enabled = true;

		GUILayout.EndHorizontal ();

		if (m != null && m.objectives != null) {
			for (int i = 0; i < m.objectives.Length; i++) {
				drawObjective(m.objectives[i]);
			}
		}
		EditorUtility.SetDirty (CD);
	}

	public void drawObjective (Objective o) {
		if (o != null)
			GUILayout.Label (o.objectiveName + " @ " + o.name);
		else
			GUILayout.Label ("NULL OBJECTIVE");
	}
	
		// Custom GUILayout progress bar.
	void  ProgressBar ( float value ,string label  ){
		// Get a rect for the progress bar using the same margins as a textfield:
		Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar (rect, value, label);
		EditorGUILayout.Space();
    }
}
