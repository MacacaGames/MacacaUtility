using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpriteList : ScriptableObject {
	public List<Sprite> spriteList = new List<Sprite>();

	public int Count {
		get {
			return spriteList.Count;
		}
	}

	public Sprite Get(int index) {
		if (index < 0 || index >= Count) {
			return null;
		}

		return spriteList [index];
	}

	public Sprite GetRandom() {
		int min = 0;
		int max = Count;
		int rand = Random.Range (min, max);

		return Get (rand);
	}
}
