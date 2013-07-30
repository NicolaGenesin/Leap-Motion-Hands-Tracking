using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    
    static Rect CenteredRect(float x, float y,float width, float height)
    {
        return new Rect(x - (width/2), y - (height/2), width, height);
    }
    
    public GUISkin m_Skin;
    
    private bool m_DisplayHelp = false;
    
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.H) )
        {
            m_DisplayHelp = !m_DisplayHelp;
        }
    }

    void OnGUI()
    {
        GUI.skin = m_Skin;
        
        GUILayout.BeginArea(new Rect(10, 10, 450, 80), new GUIStyle("HelpText"));
        GUILayout.Label("[T]ranslation " + (LeapSandboxController.enableTranslation ? "Enabled" : "Disabled"), new GUIStyle("InfoText"));
        GUILayout.Label("[R]otation " + (LeapSandboxController.enableRotation ? "Enabled" : "Disabled"), new GUIStyle("InfoText"));
        GUILayout.Label("[S]caling " + (LeapSandboxController.enableScaling ? "Enabled" : "Disabled"), new GUIStyle("InfoText"));
        GUILayout.EndArea();
        
        if( m_DisplayHelp )
        {
            GUILayout.BeginArea(new Rect(10, Screen.height-150, 450, 150),new GUIStyle("HelpText"));
            GUILayout.Label("Touch to select",new GUIStyle("HelpText"));
            GUILayout.Label("Move to translate the selected object",new GUIStyle("HelpText"));
            GUILayout.Label("Pinch to scale the selected object",new GUIStyle("HelpText"));
            GUILayout.Label("Twist to rotate the selected object",new GUIStyle("HelpText"));
            GUILayout.Label("Make a fist to de-select",new GUIStyle("HelpText"));
            GUILayout.Label("Press T, R or S to toggle translation, rotation, or scaling", new GUIStyle("HelpText"));
            GUILayout.EndArea();
            
        }
        else
        {
            GUILayout.BeginArea(new Rect(10, Screen.height-45, 450, 40), new GUIStyle("HelpText"));
            GUILayout.Label("Press 'h' for help", new GUIStyle("HelpText"));
            GUILayout.EndArea();
        }
            
    }
}
