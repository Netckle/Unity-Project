﻿using UnityEngine;
using UnityEditor;

public class ChangeFont : EditorWindow
{
    [MenuItem("Assets/Change EditorFonts")]
    static void _NewBehaviourScript ()
    {
        GetWindow<ChangeFont> ();
    }

    void OnGUI ()
    {
        var normal = (Font)AssetDatabase.LoadAssetAtPath ("Assets/Fonts/NanumGothic/NanumGothicBold.ttf", typeof(Font));
        var small = (Font)AssetDatabase.LoadAssetAtPath ("Assets/Fonts/NanumGothic/NanumGothicBold.ttf", typeof(Font));
        var big = (Font)AssetDatabase.LoadAssetAtPath ("Assets/Fonts/NanumGothic/NanumGothicBold.ttf", typeof(Font));
        var bold = (Font)AssetDatabase.LoadAssetAtPath ("Assets/Fonts/NanumGothic/NanumGothicExtraBold.ttf", typeof(Font));
        var bold_small = (Font)AssetDatabase.LoadAssetAtPath ("Assets/Fonts/NanumGothic/NanumGothicExtraBold.ttf", typeof(Font));
        var warning = (Font)AssetDatabase.LoadAssetAtPath ("Assets/Fonts/NanumGothic/NanumGothicBold.ttf", typeof(Font));
        var skin = GUI.skin;
        var so = new SerializedObject (skin);

        so.Update ();
        var prop = so.GetIterator ();

        while (prop.Next(true)) {

            if (prop.propertyType == SerializedPropertyType.ObjectReference) {
                if (prop.type == "PPtr<Font>") 
                {
                    if (prop.objectReferenceValue) 
                    {
                        var fontName = prop.objectReferenceValue.name;

                        if (fontName == "Lucida Grande") 
                        {

                        } 
                        else if (fontName == "Lucida Grande") 
                        {
                            prop.objectReferenceValue = normal;
                        } 
                        else if (fontName == "Lucida Grande small") 
                        {
                            prop.objectReferenceValue = small;
                        }
                        else if (fontName == "Lucida Grande Bold") 
                        {
                            prop.objectReferenceValue = bold;
                        }
                        else if (fontName == "Lucida Grande Big") 
                        {
                            prop.objectReferenceValue = big;
                        }
                        else if (fontName == "Lucida Grande small bold") 
                        {
                            prop.objectReferenceValue = bold_small;
                        } 
                        else if (fontName == "Lucida Grande Warning") 
                        {
                            prop.objectReferenceValue = warning;
                        }
                    } 
                    else 
                    {
                        prop.objectReferenceValue = normal;
                    }
                }
            }
        }
        so.ApplyModifiedProperties ();
        Close ();
    }
}
