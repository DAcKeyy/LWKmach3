using UnityEngine;

[CreateAssetMenu(menuName = "EducationText")]
public class EducationText : ScriptableObject
{
    [TextArea]
    public string text;
}
