using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

	public static class JSONHelper
	{

		public static T[] FromJson<T>(string json) {
			Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		public static string ToJson<T>(T[] array) {
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return UnityEngine.JsonUtility.ToJson(wrapper);
		}

		[Serializable]
		private class Wrapper<T> {
			public T[] Items;
		}
	}


