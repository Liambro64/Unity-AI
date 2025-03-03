using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework.Constraints;
using System.Data.Common;
using System;
using Unity.VisualScripting.FullSerializer;

[CustomPropertyDrawer(typeof(NeuralNetwork))]
public class NNCPD : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		NeuralNetwork n = (NeuralNetwork)property.objectReferenceValue;
		float size = 5;
		float vertGap = 5;
		float hozGap = 35;


		float InputHeight = vertGap * 2 + size * 2 + (size * 2 + vertGap) * n.inputs.Count;
		float OutputHeight = vertGap * 2 + size * 2 + (size * 2 + vertGap) * n.outputs.Count;
		float rectHeight = Mathf.Max(250, InputHeight);
		float rectWidth = 500;
		float startX = (rectWidth / 2) - ((size * 2 + hozGap) * (n.middle.Count - 1) / 2);
		if (n == null)
			return;
		Rect rect = GUILayoutUtility.GetRect(rectWidth, rectWidth, rectHeight, rectHeight);
		GUI.BeginClip(rect);
		for (int i = 0; i < n.inputs.Count; i++)
		{
			for (int j = 0; j < n.inputs[i].synapses.Count; j++)
			{
				Handles.color = new Color(1f, 1f, 1f, 1f);
				float MiddleHeight = vertGap * 2 + size * 2 + (size * 2 + vertGap) * n.middle[0].Count;
				Handles.DrawLine(new Vector3(hozGap + size, (rectHeight / 2) - (InputHeight / 2 - vertGap) + (size * 2 + vertGap) * i),
								 new Vector3(startX, (rectHeight / 2) - (MiddleHeight / 2 - vertGap) + ((size * 2 + vertGap) * j)));
			}
			Handles.color = new Color(0.5f, 0.5f, 0.5f, 1f) * (float)(n.inputs[i].Value + 1);
			Handles.DrawSolidDisc(new Vector3(hozGap + size, (rectHeight / 2) - (InputHeight / 2 - vertGap) + (size * 2 + vertGap) * i), Vector3.forward, size);
		}
		for (int i = 0; i < n.middle.Count; i++)
		{
			float MiddleHeight = vertGap * 2 + size * 2 + (size * 2 + vertGap) * n.middle[i].Count;
			for (int j = 0; j < n.middle[i].Count; j++)
			{
				for (int k = 0; k < n.middle[i][j].synapses.Count; k++)
				{
					Handles.color = new Color(1f, 1f, 1f, 1f);
					float Height = vertGap * 2 + size * 2 + (size * 2 + vertGap) * (i == n.middle.Count-1 ? n.outputs.Count : n.middle[i + 1].Count);
					float x = i == n.middle.Count-1 ? rectWidth - (hozGap + size) : startX + ((size * 2 + hozGap) * (i+1));
					Handles.DrawLine(new Vector3(startX + ((size * 2 + hozGap) * i), (rectHeight / 2) - (MiddleHeight / 2 - vertGap) + ((size * 2 + vertGap) * j)),
									 new Vector3(x, (rectHeight / 2) - (Height / 2 - vertGap) + ((size * 2 + vertGap) * k))
									 );
				}
				Handles.color = new Color(0.5f, 0.5f, 0.5f, 1f) * (float)(n.middle[i][j].Value + 1);
				Handles.DrawSolidDisc(new Vector3(startX + ((size * 2 + hozGap) * i), (rectHeight / 2) - (MiddleHeight / 2 - vertGap) + ((size * 2 + vertGap) * j)), Vector3.forward, size);
			}
		}
		for (int i = 0; i < n.outputs.Count; i++)
		{
			Handles.color = new Color(0.5f, 0.5f, 0.5f, 1f) * (float)(n.outputs[i].Value + 1);
			Handles.DrawSolidDisc(new Vector3(rectWidth - (hozGap + size), (rectHeight / 2) - (OutputHeight / 2 - vertGap) + (size * 2 + vertGap) * i), Vector3.forward, size);
		}
		GUI.EndClip();
		base.OnGUI(position, property, label);
	}


}