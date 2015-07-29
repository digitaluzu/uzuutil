using UnityEngine;
using System.Collections;

namespace Uzu
{
    /// <summary>
    /// Shows a detailed perf view of the current scene.
    /// </summary>
    public class PerfView : BaseBehaviour
    {
        public float TargetFrameRate {
            get { return _targetFrameRate; }
            set { _targetFrameRate = value; }
        }

        #region Implementation.
        [SerializeField]
        private GUIStyle _guiStyle;
        [SerializeField]
        private Vector2 _relativeOffset;
        [SerializeField]
        private float _targetFrameRate = 30.0f;
        [SerializeField]
        private float _updateIntervalInSeconds = 1.0f;

        private float _thisFrameTimeMs;
        private float _averageFrameTimeMs;
        private float _worstFrameTimeMs;

        private float _elapsedTime;
        private float _elapsedFrames;
        private float _worstFrameTimeRunning;

        private void Update ()
        {
            const float sToMs = 1000.0f;

            float deltaTime = Time.deltaTime;

            _thisFrameTimeMs = deltaTime * sToMs;

            _elapsedTime += deltaTime;
            _elapsedFrames++;

            if (deltaTime > _worstFrameTimeRunning) {
                _worstFrameTimeRunning = deltaTime;
            }

            if (_elapsedTime >= _updateIntervalInSeconds) {
                _averageFrameTimeMs = _elapsedTime / _elapsedFrames * sToMs;
                _worstFrameTimeMs = _worstFrameTimeRunning * sToMs;

                _worstFrameTimeRunning = 0.0f;

                _elapsedTime = 0.0f;
                _elapsedFrames = 0;
            }
        }

        private void OnGUI ()
        {
            BeginGUIUpdate ();

            AddLine ("Performance View:");

            float totalFrameTime = 1.0f / _targetFrameRate * 1000.0f;

            {
                float a = _thisFrameTimeMs;
                float b = a / totalFrameTime * 100.0f;
                AddLine ("Current - " + FloatToStr (a) + "ms / " + FloatToStr (b) + "%");
            }

            {
                float a = _averageFrameTimeMs;
                float b = a / totalFrameTime * 100.0f;
                AddLine ("Average - " + FloatToStr (a) + "ms / " + FloatToStr (b) + "%");
            }

            {
                float a = _worstFrameTimeMs;
                float b = a / totalFrameTime * 100.0f;
                AddLine ("Worst - " + FloatToStr (a) + "ms / " + FloatToStr (b) + "%");
            }
        }

        private static string FloatToStr (float f)
        {
            return f.ToString ("F2");
        }

        private Rect _displayRect;

        private void BeginGUIUpdate ()
        {
            // Update the display rect.
            {
                float x = _relativeOffset.x * Screen.width;
                float y = _relativeOffset.y * Screen.height;
                _displayRect = new Rect (x, y, Screen.width, Screen.height);
            }
        }

        private void AddLine (string str)
        {
            GUI.Label (_displayRect, str, _guiStyle);

            float lineOffset = _guiStyle.lineHeight;
            _displayRect.y += lineOffset;
        }
        #endregion
    }
}
