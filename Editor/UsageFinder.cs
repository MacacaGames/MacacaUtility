using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UsageFinder : MonoBehaviour {

	[MenuItem("MacacaGames/Tools/Find references to this")]
	private static void FindReferences (MenuCommand data) {
		Object context = data.context;
		if (context) {
			var comp = context as Component;
			if (comp)
				FindReferencesTo (comp);
		}
	}

	[MenuItem ("Assets/Find references to this")]
	private static void FindReferencesToAsset (MenuCommand data) {
		var selected = Selection.activeObject;
		if (selected)
			FindReferencesTo (selected);
	}

	private static void FindReferencesTo (Object to) {
		var referencedBy = new List<Object> ();
		var allObjects = Object.FindObjectsOfType<GameObject> ();
		for (int j = 0; j < allObjects.Length; j++) {
			var go = allObjects[j];

			if (PrefabUtility.GetPrefabType (go) == PrefabType.PrefabInstance) {
				if (PrefabUtility.GetPrefabParent (go) == to) {
					Debug.Log (string.Format ("referenced by {0}, {1}", go.name, go.GetType ()), go);
					referencedBy.Add (go);
				}
			}

			var components = go.GetComponents<Component> ();
			for (int i = 0; i < components.Length; i++) {
				var c = components[i];
				if (!c) continue;

				var so = new SerializedObject (c);
				var sp = so.GetIterator ();

				while (sp.NextVisible (true))
					if (sp.propertyType == SerializedPropertyType.ObjectReference) {
						if (sp.objectReferenceValue == to) {
							Debug.Log (string.Format ("referenced by {0}, {1}", c.name, c.GetType ()), c);
							referencedBy.Add (c.gameObject);
						}
					}
			}
		}

		if (referencedBy.Any ())
			Selection.objects = referencedBy.ToArray ();
		else Debug.Log ("no references in scene");
	}
}