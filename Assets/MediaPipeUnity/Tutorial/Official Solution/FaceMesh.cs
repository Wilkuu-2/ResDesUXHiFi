using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Tutorial
{
    public class FaceMesh : MonoBehaviour
    {
        [SerializeField] private TextAsset _configAsset;
        [SerializeField] private RawImage _screen;
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _fps;
        [SerializeField] private MultiFaceLandmarkListAnnotationController _multiFaceLandmarksAnnotationController;

        private CalculatorGraph _graph;
        private ResourceManager _resourceManager;

        private WebCamTexture _webCamTexture;
        private Texture2D _inputTexture;
        private Color32[] _inputPixelData;

        private IEnumerator Start()
        {
            if (WebCamTexture.devices.Length == 0)
            {
                throw new System.Exception("Web Camera devices are not found");
            }
            var webCamDevice = WebCamTexture.devices[0];
            _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
            _webCamTexture.Play();

            yield return new WaitUntil(() => _webCamTexture.width > 16);

            _screen.rectTransform.sizeDelta = new Vector2(_width, _height);

            _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            _inputPixelData = new Color32[_width * _height];

            _screen.texture = _webCamTexture;

            _resourceManager = new StreamingAssetsResourceManager();
            yield return _resourceManager.PrepareAssetAsync("face_detection_short_range.bytes");
            yield return _resourceManager.PrepareAssetAsync("face_landmark_with_attention.bytes");

            var stopwatch = new Stopwatch();

            _graph = new CalculatorGraph(_configAsset.text);
            var multiFaceLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(_graph, "multi_face_landmarks");
            multiFaceLandmarksStream.StartPolling().AssertOk();
            _graph.StartRun().AssertOk();
            stopwatch.Start();

            var screenRect = _screen.GetComponent<RectTransform>().rect;

            while (true)
            {
                _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
                var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, _width, _height, _width * 4, _inputTexture.GetRawTextureData<byte>());
                var currentTimestamp = stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000);
                _graph.AddPacketToInputStream("input_video", new ImageFramePacket(imageFrame, new Timestamp(currentTimestamp))).AssertOk();

                yield return new WaitForEndOfFrame();

                if (multiFaceLandmarksStream.TryGetNext(out var multiFaceLandmarks))
                {
                    _multiFaceLandmarksAnnotationController.DrawNow(multiFaceLandmarks);
                }
                else
                {
                    _multiFaceLandmarksAnnotationController.DrawNow(null);
                }
            }
        }

        private void OnDestroy()
        {
            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
            }

            if (_graph != null)
            {
                try
                {
                    _graph.CloseInputStream("input_video").AssertOk();
                    _graph.WaitUntilDone().AssertOk();
                }
                finally
                {

                    _graph.Dispose();
                }
            }
        }
    }
}
