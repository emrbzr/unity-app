using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
namespace UnityEngine.Replay
{
    /// <summary>
    ///     A UI image of type Image or RawImage
    /// </summary>
    public class UIImage : MonoBehaviour
    {
        public ImageType imageType;
        public TextType url;
        private Image m_Image;
        private RawImage m_RawImage;

        /// <summary>
        ///     Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Init();
        }

        /// <summary>
        ///     Gets a reference to the Image or RawImage component, or creates one
        /// </summary>
        private void Init()
        {
            if (m_Image != null || m_RawImage != null) return;
            m_Image = GetComponent<Image>();

            if (m_Image != null) return;
            m_RawImage = GetComponent<RawImage>();

            if (m_RawImage != null) return;
            m_RawImage = gameObject.AddComponent<RawImage>();
        }

        /// <summary>
        ///     Sets the image texture
        /// </summary>
        /// <param name="tex">The Texture2D to use</param>
        public void SetImage(string url)
        {
            StartCoroutine(GetText(url));
        }
        IEnumerator GetText(string url)
            {
                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(uwr.error);
                    }
                    else
                    {
                        // Get downloaded asset bundle
                        var texture = DownloadHandlerTexture.GetContent(uwr);
                        if (m_Image != null)
                        {
                          m_Image.sprite = TextureToSprite(texture);
                        }
                        if (m_RawImage != null)
                        {
                          m_RawImage.texture = texture;
                        }
                    }
                }
            }
        /// <summary>
        /// Creates a Sprite from a Texture2D
        /// </summary>
        /// <param name="tex">The Texture2D to be converted</param>
        /// <returns>The newly created Sprite</returns>
        private Sprite TextureToSprite(Texture2D tex) {
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        }
    }
}
