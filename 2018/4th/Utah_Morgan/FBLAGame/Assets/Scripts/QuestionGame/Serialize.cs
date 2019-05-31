using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Utility class used for all serialization including converting a json file to an object and using a serialized property in an editor
/// </summary>
[Serializable]
public static class Serialize
{ 
          
    public static class JsonSerializer
    {
        /// <summary>
        /// Saves data of an object to the given file
        /// </summary>
        /// <param name="data">The json data to save</param>
        /// <param name="filePath">The path of the file to save it to</param>
        public static void SaveData(object data, string filePath)
        {
            string dataAsJson = JsonUtility.ToJson(data);

            //string filePath = Application.dataPath + gameDataProjectFilePath;
            File.WriteAllText(filePath, dataAsJson);
        }

        /// <summary>
        /// Loads data from a given file path
        /// </summary>
        /// <param name="filePath">The path of the file to load from</param>
        /// <param name="data">The json data from this path</param>
        /// <returns>True if this data was successfully loaded/returns>
        public static bool LoadData(string filePath, out object data)
        {
            string FilePath = Application.dataPath + filePath;
            object gameData;
            bool flag = File.Exists(FilePath);
            if (flag)
            {
                string dataAsJson = File.ReadAllText(FilePath);
                gameData = JsonUtility.FromJson<object>(dataAsJson);
            }
            else
            {
                gameData = new object();
            }
            data = gameData;
            return flag;
        }

        /// <summary>
        /// Loads data from a given file path
        /// </summary>
        /// <param name="filePath">The path of the file to load from</param>
        /// <param name="data">The json data from this path</param>
        /// <returns>True if this data was successfully loaded/returns>
        public static bool LoadData<T>(string filePath, out T data) where T : new()
        {            
            T gameData;
            bool flag = File.Exists(filePath);
            if (flag)
            {
                string dataAsJson = File.ReadAllText(filePath);
                gameData = JsonUtility.FromJson<T>(dataAsJson);
            }
            else
            {                
                gameData = new T();
            }
            data = gameData;
            return flag;
        }
    }


#if UNITY_EDITOR

    /// <summary>
    /// Gets the object that the given serialized property represents
    /// </summary>
    /// <param name="prop">The serialized property version of the object wanted</param>
    /// <returns>The object version of the serialized property</returns>
    public static object GetThis(UnityEditor.SerializedProperty prop)
    {
        string path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        string[] elements = path.Split('.');
        
        foreach (string element in elements)
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue(obj, elementName, index);
            }
            else
            {
                obj = GetValue(obj, element);
            }
        }
        return obj;
    }

    /// <summary>
    /// Gets the parent of a serialized object
    /// </summary>
    /// <param name="prop">The child of the object wanted</param>
    /// <returns>The parent of the given serialized property</returns>
    public static object GetParent(UnityEditor.SerializedProperty prop)
    {
        string path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        string[] elements = path.Split('.');
        
        foreach (string element in elements.Take(elements.Length - 1))
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue(obj, elementName, index);
            }
            else
            {
                obj = GetValue(obj, element);
            }
        }
        return obj;
    }

    /// <summary>
    /// Gets the root parent of a serialized property
    /// </summary>
    /// <param name="property">The serialized property given</param>
    /// <returns>The root parent</returns>
    public static object GetRootParent(UnityEditor.SerializedProperty property)
    {
        return property.serializedObject.targetObject;
    }

    /// <summary>
    /// Gets the value of a given object
    /// </summary>
    /// <param name="source">The source object</param>
    /// <param name="name">The name of the value wanted</param>
    /// <returns>The value</returns>
    public static object GetValue(object source, string name)
    {
        if (source == null)
            return null;
        Type type = source.GetType();
        FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (f == null)
        {
            PropertyInfo p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p == null)
                return null;
            return p.GetValue(source, null);
        }
        return f.GetValue(source);
    }

    /// <summary>
    /// Gets the value of a given object
    /// </summary>
    /// <param name="source">The source object</param>
    /// <param name="name">The name of the value wanted</param>
    /// <param name="index">The index of the object</param>
    /// <returns>The value</returns>
    public static object GetValue(object source, string name, int index)
    {
        IEnumerable enumerable = GetValue(source, name) as IEnumerable;
        IEnumerator enm = enumerable.GetEnumerator();
        while (index-- >= 0)
            enm.MoveNext();
        return enm.Current;
    }
#endif
}

/// <summary>
/// Data used in the dialogue editor
/// </summary>
[Serializable]
public class ReuseableData
{
    /// <summary>
    /// The data as a string value
    /// </summary>
    public string DataAsString;

    /// <summary>
    /// The data as a list of dialogues
    /// </summary>
    public List<Dialogue> DataAsDialogue;

    /// <summary>
    /// The data as a list of responses
    /// </summary>
    public List<Response> DataAsResponse;

    /// <summary>
    /// The id name of this data
    /// </summary>
    public string IdName;

    /// <summary>
    /// Initalizes a new data
    /// </summary>
    /// <param name="Data">The data to use</param>
    /// <param name="IdName">The id name of the data</param>
    public ReuseableData(object Data, string IdName)
    {
        if (Data is List<Dialogue>)
        {
            DataAsDialogue = (List<Dialogue>)Data;
        }
        else if (Data is List<Response>)
        {
            DataAsResponse = (List<Response>)Data;
        }
        else if (Data is string)
        {
            DataAsString = (string)Data;
        }        
        this.IdName = IdName;
    }
    
    /// <summary>
    /// Determines if two data have the same id name
    /// </summary>
    /// <param name="obj">The data to compare to</param>
    /// <returns>If the two data have the same id name</returns>
    public override bool Equals(object obj)
    {
        ReuseableData o = (ReuseableData)obj;
        return (o != null && IdName == o.IdName);
    }
}

