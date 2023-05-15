using UnityEngine;

namespace UI
{
    public class MapScript : UIScript
    {
        [SerializeField]
        private Camera MiniMapCamera;

        internal override void OnBind()
        {
            base.OnBind();

            MiniMapCamera.orthographicSize = 60;
        }

        private void OnDisable()
        {
            MiniMapCamera.orthographicSize = 10;
        }
    }
}
