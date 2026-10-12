using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    
    
    
    public class TopicSlotsViewController : IStartable
    {
        private readonly ForumController _forumController;
        private readonly RectTransform[] _topicSlots;
        
        public TopicSlotsViewController(
            [Key("topic-slot-1")]RectTransform topicSlot1,
            [Key("topic-slot-2")]RectTransform topicSlot2,
            [Key("topic-slot-3")]RectTransform topicSlot3,
            [Key("topic-slot-4")]RectTransform topicSlot4,
            [Key("topic-slot-5")]RectTransform topicSlot5,
            ForumController forumController)
        {
            _forumController = forumController;
            _topicSlots = new[] { topicSlot1, topicSlot2, topicSlot3, topicSlot4,  topicSlot5 };
        }
        
        public void Start()
        {
            _forumController.OnTopicSlotSelected += SelectTopic;
        }

        private void SelectTopic(TopicSlot value)
        {
            var topicSlot = _topicSlots[value.Id - 1];
            topicSlot.SetAsFirstSibling();
            
            for (var i = 0; i < _topicSlots.Length; i++)
            {
                var button = _topicSlots[i].GetComponentInChildren<Button>();

                button.interactable = i == value.Id - 1;
            }
        }
    }
}