using UnityEngine;

// Editor
[CreateAssetMenu(fileName = "Title", menuName = "Debug")]
public class EditorDebugSo : ScriptableObject
{
    public string[] VoiceText;
    public float FadeTime;
    public float FadeoutDelayTime;
    public float TextDuration;
}