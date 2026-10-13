using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class TopicSlotsViewController : IStartable
    {
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
            _forumController.OnTopicOpened += CreateTopic;
            _forumController.OnTopicSlotSelected += SelectTopic;
        }

        private void CreateTopic(TopicSlot topic)
        {
            var topicView = Object.Instantiate(_topicSlotPrefab, _topicSlotsRoot);
            topicView.SetId(topic.Id, _topicViews.Count);
            topic.Topic.OnNewMessage += topicView.CreateMessage;
            
            
            _topicViews.Add(topicView);
            SelectTopic(topic);
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