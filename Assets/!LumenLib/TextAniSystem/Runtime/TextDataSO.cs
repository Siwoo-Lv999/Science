using UnityEngine;

namespace _LumenLib.TextAniSystem.Runtime
{
    [CreateAssetMenu(fileName = "Text Data", menuName = "Lib/TextAni/Text Data", order = 0)]
    public class TextDataSO : ScriptableObject
    {
        [TextArea(3, 10)]
        public string text;
        public float waitDuration = 0.05f;
    }
}