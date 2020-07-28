using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class Empty4Raycast : MaskableGraphic,IPointerDownHandler, IPointerUpHandler
    {
        public Image Image;
        public Color NormalColor = new Color(1, 1, 1, 1);
        //public Color PressendColor = Util.HexToColor("C8C8C8FF");

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Image != null&&Image.sprite != null)
            {
                NormalColor = Image.color;
                //Image.color = PressendColor;
            }

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Image != null&&Image.sprite != null)
            {
                Image.color = NormalColor;
            }
        }

        protected Empty4Raycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}
