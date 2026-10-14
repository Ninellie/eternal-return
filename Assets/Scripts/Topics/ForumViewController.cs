using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class ForumViewController : IStartable, ITickable
    {
        private readonly ForumController _forumController;
        private readonly RectTransform _topicSlotsRoot;
        private readonly TopicView _topicSlotPrefab;
        
        private readonly List<TopicView> _topicViews;
        
        public ForumViewController(ForumController forumController,
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

        public void Tick()
        {
            foreach (var topicView in _topicViews)
            {
                var messages = topicView.Messages;
                
                foreach (var messageView in messages)
                {
                    messageView.UpdateDuration();
                }
            }
        }

        private void CreateTopic(TopicSlot topicSlot)
        {
            var topicView = Object.Instantiate(_topicSlotPrefab, _topicSlotsRoot);
            
            topicView.SetId(topicSlot.Id, _topicViews.Count);
            topicSlot.Topic.OnNewMessage += topicView.CreateMessage;
            topicSlot.Topic.OnMessageLost += topicView.DeleteMessage; 
            topicSlot.Topic.OnMessageComplete += topicView.DeleteMessage;
            
            _topicViews.Add(topicView);
            SelectTopic(topicSlot);
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