using System.Collections;
using _LumenLib.CoreSystem.EventChannelSystem.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _LumenLib.TextAniSystem.Runtime
{
    public class ShowTextEvent : GameEvent
    {
        public TextDataSO TextData { get; private set; }
        
        public ShowTextEvent(TextDataSO textData)
        {
            TextData = textData;
        }
    }

    public class SkipTextAniEvent : GameEvent
    {
        public static SkipTextAniEvent Instance { get; private set; } = new();
    }
    
    public class TextAnimation : MonoBehaviour
    {
        [SerializeField] private EventChannelSO evtChannel;
        [SerializeField] private TextMeshProUGUI textMesh;

        public UnityEvent onTextAniFinished;
        
        private WaitForSeconds _waitDuration;
        private Coroutine _currentCoroutine;
        private TextDataSO _currentTextData;
        private string _currentString;

        private void OnEnable()
        {
            evtChannel.AddListener<ShowTextEvent>(StartShow);
            evtChannel.AddListener<SkipTextAniEvent>(SkipAni);
        }

        private void OnDisable()
        {
            evtChannel.RemoveListener<ShowTextEvent>(StartShow);
            evtChannel.RemoveListener<SkipTextAniEvent>(SkipAni);
        }

        public void StartShow(ShowTextEvent evt)
        {
            _currentTextData = evt.TextData;
            _waitDuration = new WaitForSeconds(evt.TextData.waitDuration);
            _currentCoroutine = StartCoroutine(TextAniCoroutine());
        }

        public void SkipAni(SkipTextAniEvent evt)
        {
            if (_currentCoroutine == null)
                return;
            
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            
            textMesh.SetText(_currentTextData.text);
            
            onTextAniFinished?.Invoke();
            _currentTextData = null;
        }

        private IEnumerator TextAniCoroutine()
        {
            foreach (var c in _currentTextData.text)
            {
                _currentString += c;
                textMesh.SetText(_currentString);
                yield return _waitDuration;
            }
            
            onTextAniFinished?.Invoke();
            _currentTextData = null;
        }
    }
}