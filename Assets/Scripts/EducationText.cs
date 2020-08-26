using UnityEngine;

[CreateAssetMenu(menuName = "EducationText")]
public class EducationText : ScriptableObject
{
    [TextArea(0,30)]
    public string text;
}
