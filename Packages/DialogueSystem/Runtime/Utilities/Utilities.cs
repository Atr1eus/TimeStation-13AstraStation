using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ExtensionMethods
{

    public static List<int> AllIndexesOf(this string str, string value)
    {
        if (String.IsNullOrEmpty(value))
            throw new ArgumentException("the string to find may not be empty", "value");
        List<int> indexes = new List<int>();
        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);
            if (index == -1)
                return indexes;
            indexes.Add(index);
        }
    }

}

namespace DialogueSystem
{
    public static class DialogueUtilities
    {
        public static float DecreasingFunction(float input)
        {
            return 1 / input;
        }

        public static float FloatToByte(float valueToConvert)
        {
            float b = (valueToConvert >= 1.0 ? 255 : (valueToConvert <= 0.0 ? 0 : (int)Mathf.Floor(valueToConvert * 256.0f)));
            b = Mathf.FloorToInt(b);
            return b;
        }

        public static string ReplaceWords(string originalString, DialogueDictionary dictionary)
        {

            Regex reg = new Regex(@"<.*?/>");
            MatchCollection matches = reg.Matches(originalString);
            string interpolatedString = originalString;
            foreach (Match match in matches)
            {
                GroupCollection group = match.Groups;
                foreach (Group key in group)
                {
                    if (key.Value.Length <= 3 || string.IsNullOrWhiteSpace(key.Value.Substring(1, key.Value.Length - 3)))
                    {
                        Debug.LogWarning("Empty dictionary key found in text.");
                        continue;
                    }
                    else
                    {
                        string keyValue = key.Value.Substring(1, key.Value.Length - 3);
                        string dictionaryValue = dictionary.GetEntry(keyValue);
                        if (dictionaryValue != null)
                        {
                            interpolatedString = interpolatedString.Replace(key.Value, dictionaryValue);
                        }
                    }
                }
            }

            return interpolatedString;
        }

        public static void RotateVertices(float degrees, ref Vector3[] vertices)
        {
            Vector3 center = CalculateCenter(vertices);//new Vector3(x, y, z);//any V3 you want as the pivot point.
            center.z = vertices[0].z;
            Debug.DrawLine(center, center+Vector3.up * 5);
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = new Vector3(0, 0, degrees);//the degrees the vertices are to be rotated, for example (0,90,0) 

            for (int i = 0; i < vertices.Length; i++)
            {//vertices being the array of vertices of your mesh
                vertices[i] = newRotation * (vertices[i] - center) + center;
            }
        }

        public static void RotateVertex(float degrees, ref Vector3 vertex, Vector3 center)
        {
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = new Vector3(0, 0, degrees);//the degrees the vertices are to be rotated, for example (0,90,0) 


            vertex = newRotation * (vertex - center) + center;
            
        }

        public static Vector3 CalculateCenter(Vector3[] vertices)
        {
            //Triangle 1
            float tx1 = (vertices[0].x + vertices[1].x + vertices[3].x) / 3;
            float ty1 = (vertices[0].y + vertices[1].y + vertices[3].y) / 3;

            //Triangel 2
            float tx2 = (vertices[1].x + vertices[2].x + vertices[3].x) / 3;
            float ty2 = (vertices[1].y + vertices[2].y + vertices[3].y) / 3;

            float midx = (tx1 + tx2) / 2;
            float midy = (ty1 + ty2) / 2;
            return new Vector3(midx, midy, 0);
        }

        public static void ScaleVertices(float scale, ref Vector3[] vertices)
        {
            Vector3 center = CalculateCenter(vertices);

            for (int i = 0; i < vertices.Length; i++)
            {//vertices being the array of vertices of your mesh
                vertices[i] = (vertices[i] - center) * scale + center;
            }


        }

    }
}

