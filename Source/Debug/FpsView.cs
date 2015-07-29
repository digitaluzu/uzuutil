using UnityEngine;
using System.Collections;

namespace Uzu
{
    /// <summary>
    /// Displays FPS to the screen.
    /// </summary>
    public class FpsView : BaseBehaviour
    {
        [SerializeField]
        private GUIStyle _guiStyle;
        [SerializeField]
        private Vector2 _relativeOffset;
        [SerializeField]
        private float _timeSpan = 1.0f;
        private int _frameCount;
        private float _deltaTime;
        private float _averageFPS;

        private void Update ()
        {
            _deltaTime += Time.deltaTime;
            _frameCount++;

            if (_deltaTime > _timeSpan) {
                _averageFPS = _frameCount / _timeSpan;

                _frameCount = 0;
                _deltaTime = 0;
            }
        }

        private void OnGUI ()
        {
            float x = _relativeOffset.x * Screen.width;
            float y = _relativeOffset.y * Screen.height;
            Rect displayRect = new Rect (x, y, Screen.width, Screen.height);

            GUI.Label (displayRect, ((int)_averageFPS).ToString (), _guiStyle);
        }
    }
}
