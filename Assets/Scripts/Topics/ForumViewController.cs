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
        
        private readonly Dictionary<Topic, TopicView> _topicViewsByTopic;
        
        public ForumViewController(ForumController forumController,
            [Key("topics-root")] RectTransform topicSlotsRoot,
            TopicView topicSlotPrefab)
        {
            _forumController = forumController;
            _topicSlotsRoot = topicSlotsRoot;
            _topicSlotPrefab = topicSlotPrefab;
            
            _topicViewsByTopic = new Dictionary<Topic, TopicView>();
        }
        
        public void Start()
        {
            _forumController.OnTopicOpened += CreateTopic;
            _forumController.OnTopicClosed += CloseTopic;
            _forumController.OnTopicAborted += AbortTopic;
            _forumController.OnTopicSelected += SelectTopic;
        }

        public void Tick()
        {
            foreach (var topicView in _topicViewsByTopic.Values)
            {
                topicView.Tick();
            }
        }

        private void CloseTopic(Topic topic)
        {
            _topicViewsByTopic.Remove(topic, out var topicView);
            
            Object.Destroy(topicView.gameObject);
        }

        private void AbortTopic(Topic topic)
        {
            _topicViewsByTopic.Remove(topic, out var topicView);

            Object.Destroy(topicView.gameObject);
        }

        private void CreateTopic(TopicSlot topicSlot)
        {
            var topicView = Object.Instantiate(_topicSlotPrefab, _topicSlotsRoot);
            
            topicView.Init(topicSlot.Topic, _forumController.SlotCount);
            
            topicSlot.Topic.OnNewMessage += topicView.CreateMessage;
            topicSlot.Topic.OnMessageLost += topicView.DeleteMessage; 
            topicSlot.Topic.OnMessageComplete += topicView.DeleteMessage;
            
            _topicViewsByTopic.Add(topicSlot.Topic, topicView);
        }

        private void SelectTopic(Topic topic)
        {
            foreach (var view in _topicViewsByTopic.Values)
            {
                if (view.SlotId == topic.SlotId)
                {
                    view.Select();
                    continue;
                }
                view.Deselect();
            }
        }
    }
}