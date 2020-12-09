using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace MacacaGames.EditorTools
{
    public class ImageConverter : EditorWindow
    {

        [MenuItem("MacacaGames/EditorTools/ImageConverter")]
        public static void Init()
        {
            EditorWindow window = GetWindow<ImageConverter>();
        }
		Texture2D t;
        private void OnGUI()
        {
			t = (Texture2D)EditorGUILayout.ObjectField(t, typeof(Texture2D), true);

			if (GUILayout.Button("Convert"))
            {
                var s = System.Convert.ToBase64String (t.EncodeToPNG ()); 
				Debug.Log("result:" + s);
            }
        }
    }

}
