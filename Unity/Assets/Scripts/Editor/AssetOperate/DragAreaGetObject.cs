using UnityEngine;
using System.Collections;
using UnityEditor;
 
public class DragAreaGetObject : Editor
{
 
    public static Object[] GetObject(string meg = null)
    {

        Event aEvent;
        aEvent = Event.current;

        GUI.contentColor = Color.white;
        var dragArea = GUILayoutUtility.GetRect(0f, 35f, GUILayout.ExpandWidth(true));

        GUIContent title = new GUIContent(meg);
        if (string.IsNullOrEmpty(meg))
        {
            title = new GUIContent("拖拽对象至此，添加引用");
        }

        GUI.Box(dragArea, title);
        switch (aEvent.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dragArea.Contains(aEvent.mousePosition))
                {
                    break;
                }

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (aEvent.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    if (DragAndDrop.objectReferences.Length > 0)
                    {
                        return DragAndDrop.objectReferences;
                    }
                }

                Event.current.Use();
                break;
            default:
                break;
        }

        return new Object[0];
    }
}