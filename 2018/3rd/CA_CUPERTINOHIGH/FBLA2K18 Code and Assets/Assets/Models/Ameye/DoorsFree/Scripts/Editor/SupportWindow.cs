using UnityEngine;
using UnityEditor;

public class SupportWindow : EditorWindow
{
    [MenuItem("Tools/Doors Free/Support")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SupportWindow));
        SupportWindow myWindow = (SupportWindow)GetWindow(typeof(SupportWindow));
        myWindow.titleContent = new GUIContent("Support");
    }

    public static void Init()
    {
        SupportWindow myWindow = (SupportWindow)GetWindow(typeof(SupportWindow));
        myWindow.Show();
    }

    void OnGUI()
    {
        SupportWindow myWindow = (SupportWindow)GetWindow(typeof(SupportWindow));
        myWindow.minSize = new Vector2(300, 213);
        myWindow.maxSize = myWindow.minSize;

        if (GUILayout.Button(Styles.Forum, Styles.helpbox))
        {
            Application.OpenURL("https://forum.unity3d.com/threads/released-doors-free-v2-3.445297/");
        }

        if (GUILayout.Button(Styles.Documentation, Styles.helpbox))
        {
            Application.OpenURL("http://docdro.id/JY0BUIP");
        }

        if (GUILayout.Button(Styles.Contact, Styles.helpbox))
        {
            Application.OpenURL("mailto:alexanderameye@gmail.com?");
        }

        if (GUILayout.Button(Styles.Twitter, Styles.helpbox))
        {
            Application.OpenURL("https://twitter.com/blacksadunity");
        }

        if (GUILayout.Button(Styles.Review, Styles.helpbox))
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/account/downloads/search=Doors%20Free");
        }
    }

    static class Styles
    {
        internal static GUIContent Forum;
        internal static GUIContent Documentation;
        internal static GUIContent Contact;
        internal static GUIContent Twitter;
        internal static GUIContent Review;
        internal static GUIStyle helpbox;

        static Styles()
        {
            Forum = IconContent("forum_colored", "<size=11><b> Support Forum</b></size>");
            Documentation = IconContent("documentation_colored", "<size=11><b> Online Documentation</b></size>");
            Contact = IconContent("contact_colored", "<size=11><b> Contact</b></size>");
            Review = IconContent("review_colored", "<size=11><b> Rate and Review</b></size>");
            Twitter = IconContent("twitter_colored", "<size=11><b> Twitter</b></size>");

            helpbox = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleLeft,
                richText = true
            };
        }

        static GUIContent IconContent(string icon, string text)
        {
            Texture2D cached = EditorGUIUtility.Load("Assets/Ameye/DoorsFree/Icons/" + icon + ".png") as Texture2D;
            return new GUIContent(text, cached);
        }
    }

}
