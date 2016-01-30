using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TeamSignal.Utilities.Template
{
	public class EnumObjectTemplateBase
	{
	
	}

	public class EnumObjectTemplate<T,U> : EnumObjectTemplateBase
	{
		public T type;
		public U item;
	}
	
	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(EnumObjectTemplateBase), true)]
	public class TwinObjectDrawer : PropertyDrawer
	{
		const float height = 18f;
	
	//	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) 
	//	{
	//		return property.isExpanded ? height * 2 : height; //單行18f，因為折疊打開後有標籤跟3行property，所以打開後的高要設定18*4=72f
	//	}
	
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
	//		position.height = height;
	//		//設定一個新的折疊標籤
	//		EditorGUI.BeginProperty(position, label, property);
	//		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true); //使用Foldout來開關折疊
	//		
	//		if(property.isExpanded)
	//		{			
	//			EditorGUIUtility.labelWidth = 50f;	//調整欄位label的寬度，避免label跟input之間太多空白，看自己需求或label長度自行調整
	//			position.width /= 2;
	//			
	//			position.y += height;
	//			EditorGUI.PropertyField(position, property.FindPropertyRelative("type"));
	//			position.x += position.width;
	//			EditorGUI.PropertyField(position, property.FindPropertyRelative("item"));
	//		}
	//		EditorGUI.EndProperty();
	
			EditorGUIUtility.labelWidth = 50f;	//調整欄位label的寬度，避免label跟input之間太多空白，看自己需求或label長度自行調整
			position.width /= 2;
			EditorGUI.PropertyField(position, property.FindPropertyRelative("type"), label);
			position.x += position.width;
			EditorGUI.PropertyField(position, property.FindPropertyRelative("item"), new GUIContent(""));
		}
	}
	#endif
}