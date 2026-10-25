using EternalReturn.Skills;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class LearningSkillChoiceViewController : IStartable
    {
        private const float CardScale = 2.5f;
        private const float CardSpacing = 220f;
        private const float DimmerAlpha = 0.72f;

        private readonly LearningController _learningController;
        private readonly SkillView _skillViewPrefab;
        private readonly Button _startLearningButton;

        private GameObject _root;
        private static Sprite _whiteSprite;

        public LearningSkillChoiceViewController(
            LearningController learningController,
            SkillView skillViewPrefab,
            [Key("start-learning")] Button startLearningButton)
        {
            _learningController = learningController;
            _skillViewPrefab = skillViewPrefab;
            _startLearningButton = startLearningButton;
        }

        public void Start()
        {
            _learningController.OnSkillChoiceOpened += Show;
            _learningController.OnSkillChoiceClosed += Hide;
        }

        private void Show()
        {
            Hide();

            var canvas = _startLearningButton.GetComponentInParent<Canvas>();
            _root = new GameObject("Learning Skill Choice", typeof(RectTransform));
            var rootTransform = _root.GetComponent<RectTransform>();
            rootTransform.SetParent(canvas.transform, false);
            Stretch(rootTransform);
            rootTransform.SetAsLastSibling();

            var dimmerObject = new GameObject("Dimmer", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            var dimmerTransform = dimmerObject.GetComponent<RectTransform>();
            dimmerTransform.SetParent(rootTransform, false);
            Stretch(dimmerTransform);

            var dimmer = dimmerObject.GetComponent<Image>();
            dimmer.sprite = WhiteSprite();
            dimmer.color = new Color(0f, 0f, 0f, DimmerAlpha);
            dimmer.raycastTarget = true;

            var offers = _learningController.Offers;
            for (var i = 0; i < offers.Length; i++)
            {
                var card = Object.Instantiate(_skillViewPrefab, rootTransform);
                card.BindSelection(OnCardSelected);
                card.SetSkill(offers[i]);

                var rect = card.RectTransform;
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                var offset = i - (offers.Length - 1) / 2f;
                rect.anchoredPosition = new Vector2(offset * CardSpacing, 0f);
                rect.localScale = Vector3.one * CardScale;
            }
        }

        private void OnCardSelected(Skill skill)
        {
            _learningController.ChooseSkill(skill);
        }

        private void Hide()
        {
            if (_root == null) return;

            _root.SetActive(false);
            Object.Destroy(_root);
            _root = null;
        }

        private static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;
        }

        private static Sprite WhiteSprite()
        {
            if (_whiteSprite != null) return _whiteSprite;

            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            _whiteSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f));
            return _whiteSprite;
        }
    }
}
