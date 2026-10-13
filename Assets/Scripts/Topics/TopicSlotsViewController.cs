using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class TopicSlotsViewController : IStartable
    {
        private const int TopicCount = 5;
        
        private readonly ForumController _forumController;
        private readonly RectTransform _topicSlotsRoot;
        private readonly TopicView _topicSlotPrefab;
        
        private readonly List<TopicView> _topicViews;
        
        public TopicSlotsViewController(ForumController forumController,
            [Key("topics-root")] RectTransform topicSlotsRoot,
            TopicView topicSlotPrefab)
        {
            _forumController = forumController;
            _topicSlotsRoot = topicSlotsRoot;
            _topicSlotPrefab = topicSlotPrefab;
            
            _topicViews = new List<TopicView>();
        }
        
        public void Start()
        {
            for (var i = 0; i < TopicCount; i++)
            {
                var topicView = Object.Instantiate(_topicSlotPrefab, _topicSlotsRoot);
                _topicViews.Add(topicView);
                topicView.SetId(i + 1, TopicCount);
                topicView.Deselect();
            }
            
            _topicViews[0].Select();
            
            _forumController.OnTopicSlotSelected += SelectTopic;
        }

        private void SelectTopic(TopicSlot value)
        {
            foreach (var view in _topicViews)
            {
                if (view.Id == value.Id)
                {
                    view.Select();
                    continue;
                }
                view.Deselect();
            }
        }
    }
}