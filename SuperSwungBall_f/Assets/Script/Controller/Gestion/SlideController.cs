using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gestion
{
    public class SlideController : MonoBehaviour
    {
        [SerializeField]
        private GameObject swungmen_panel;
        [SerializeField]
        private GameObject chest_panel;
        [SerializeField]
        private GameObject challenge_panel;

        private ScrollRect content_scroll_view;
        private float actual_position;

        // -- Setup Slide
        private bool slideAuthorised = true;
        private const int SENSIBILITY = 40;
        private const float INTENSITY = 0.2f;
        // --

        void Awake()
        {
            this.content_scroll_view = GetComponent<ScrollRect>();
        }

        // Update is called once per frame
        void Update()
        {

            if (!this.slideAuthorised)
                return;

            var leftLerp = Input.mousePosition.x;
            var rightLerp = Screen.width - Input.mousePosition.x;

            var min = Mathf.Min(leftLerp, rightLerp);

            var lerpMouse = 0;
            if (min == rightLerp)
                lerpMouse = 1;
            else
                lerpMouse = -1;

            var speed = SENSIBILITY + (int)(min * -INTENSITY);
            var actualPos = content_scroll_view.horizontalNormalizedPosition;
            content_scroll_view.horizontalNormalizedPosition = Mathf.Lerp(
                actualPos,
                actualPos + 0.3f * lerpMouse,
                speed * content_scroll_view.elasticity * Time.deltaTime);
        }

        /// <summary> Instantie un player achetable </summary>
        public void InstanciateChallenge(Player p)
        {
            float scroll_view_w = content_scroll_view.content.sizeDelta.x;

            Transform panel = Instantiate(challenge_panel).transform as Transform;
            ChallengeController script = panel.GetComponent<ChallengeController>();
            script.Player = p;

            float panel_w = ((RectTransform)panel).sizeDelta.x;

            RectTransform scroll_view = content_scroll_view.content.GetComponent<RectTransform>();
            float new_scroll_view_w = scroll_view_w + (panel_w + 5);

            scroll_view.sizeDelta = new Vector2(new_scroll_view_w, scroll_view.sizeDelta.y);

            if (new_scroll_view_w > scroll_view_w)
                scroll_view.anchoredPosition = new Vector3(new_scroll_view_w - scroll_view_w, 0);

            panel.SetParent(content_scroll_view.content.transform, false);
            actual_position += ((panel_w / 2) + 5);
            ((RectTransform)panel).anchoredPosition = new Vector2(actual_position, 0);
            actual_position += ((panel_w / 2));
        }

        /// <summary> Instantie un player achetable </summary>
        public void InstanciatePlayer(Player p)
        {
            float scroll_view_w = content_scroll_view.content.sizeDelta.x;

            Transform panel = Instantiate(swungmen_panel).transform as Transform;
            PlayerController script = panel.GetComponent<PlayerController>();
            script.Player = p;

            float panel_w = ((RectTransform)panel).sizeDelta.x;

            RectTransform scroll_view = content_scroll_view.content.GetComponent<RectTransform>();
            float new_scroll_view_w = scroll_view_w + (panel_w + 5);

            scroll_view.sizeDelta = new Vector2(new_scroll_view_w, scroll_view.sizeDelta.y);

            if (new_scroll_view_w > scroll_view_w)
                scroll_view.anchoredPosition = new Vector3(new_scroll_view_w - scroll_view_w, 0);

            panel.SetParent(content_scroll_view.content.transform, false);
            actual_position += ((panel_w / 2) + 5);
            ((RectTransform)panel).anchoredPosition = new Vector2(actual_position, 0);
            actual_position += ((panel_w / 2));
        }

        /// <summary> Instantie un coffre achetable </summary>
        public void InstanciateChest()
        {
            float scroll_view_w = content_scroll_view.content.sizeDelta.x;

            Transform panel = Instantiate(chest_panel).transform as Transform;

            float panel_w = ((RectTransform)panel).sizeDelta.x;

            RectTransform scroll_view = content_scroll_view.content.GetComponent<RectTransform>();
            float new_scroll_view_w = scroll_view_w + (panel_w + 5);

            scroll_view.sizeDelta = new Vector2(new_scroll_view_w, scroll_view.sizeDelta.y);

            if (new_scroll_view_w > scroll_view_w)
                scroll_view.anchoredPosition = new Vector3(new_scroll_view_w - scroll_view_w, 0);

            panel.SetParent(content_scroll_view.content.transform, false);
            actual_position += ((panel_w / 2) + 5);
            ((RectTransform)panel).anchoredPosition = new Vector2(actual_position, 0);
            actual_position += ((panel_w / 2));
        }

        public void OnSlideStateChange(bool state)
        {
            this.slideAuthorised = state;
        }
    }
}