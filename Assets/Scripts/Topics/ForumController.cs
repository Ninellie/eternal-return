using System;
using System.Linq;
using EternalReturn.Input;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class ForumController : IStartable, ITickable
    {
        private readonly InputManager _inputManager;
        private readonly MessageGenerator _messageGenerator;
        private readonly int _slotsAmount;
        private readonly TopicSlot[] _slots;
        
        public event Action<Topic> OnTopicOpened;
        public event Action<Topic> OnTopicClosed;
        public event Action<TopicSlot> OnTopicSlotSelected;
        public event Action<TopicMessage> OnMessageComplete;
        public event Action<TopicMessage> OnSymbolPressed;
        
        public ForumController(TopicSettings settings, InputManager inputManager)
        {
            _inputManager = inputManager;
            _messageGenerator = new MessageGenerator(settings);
            _slotsAmount = settings.SlotsAmount;
            _slots = new TopicSlot[_slotsAmount];
        }
        
        public void Start()
        {
            for (var i = 0; i < _slotsAmount; i++)
            {
                _slots[i] = new TopicSlot
                {
                    Id = i + 1,
                    Status = SlotStatus.Locked,
                    IsSelected = false
                };
            }
            
            _slots[0].Status = SlotStatus.Unoccupied;
            _slots[0].IsSelected = true;

            _inputManager.OnSelectSlot1 += () => SelectTopic(1);
            _inputManager.OnSelectSlot2 += () => SelectTopic(2);
            _inputManager.OnSelectSlot3 += () => SelectTopic(3);
            _inputManager.OnSelectSlot4 += () => SelectTopic(4);
            _inputManager.OnSelectSlot5 += () => SelectTopic(5);
        }

        public void SelectTopic(int id)
        {
            foreach (var slot in _slots)
            {
                slot.IsSelected = slot.Id == id;
            }
            
            OnTopicSlotSelected?.Invoke(_slots[id - 1]);
        }
        
        public void Tick()
        {
            foreach (var slot in _slots)
            {
                // Если слот не занят - далее
                if (slot.Status is not SlotStatus.Occupied) continue;
                
                // Если слот не выбран - далее
                if (!slot.IsSelected) continue;

                return;
                // todo позже перевести на InputController и добавить сброс при неправильном нажатии
                
                // Найти первое сообщение топика в очереди. Если нет - далее
                if (!slot.Topic.CurrentMessages.TryPeek(out var message)) continue;
                
                // Найди следующий активный символ
                var nextSymbol = message.Symbols[message.Pointer]; 
                    
                // Проверить действительно ли символ был нажат. Если нет - далее
                // if (!Input.GetKey((KeyCode)nextSymbol.Text)) continue;

                OnSymbolPressed?.Invoke(message);

                // Символ успешно нажат - увеличить указатель
                message.Pointer++;
                message.Symbols[message.Pointer].IsPressed = true;
                        
                // Проверить нажаты ли все символы. Если нет - далее
                if (message.Pointer < message.Symbols.Length) continue;
                        
                // Сообщение завершено так как все символы нажаты
                var completedMessage = slot.Topic.CurrentMessages.Dequeue();
                            
                // Вызвать событие
                OnMessageComplete?.Invoke(completedMessage);
            }
            
            foreach (var slot in _slots)
            {
                if (slot.Status is not SlotStatus.Occupied) continue;

                var topic = slot.Topic;
                topic.Tick(Time.deltaTime);

                // Если тема мёртвая - надо закрыть.
                if (!topic.IsDead()) continue;
                
                OnTopicClosed?.Invoke(topic);

                slot.Topic = null;
                slot.Status = SlotStatus.Unoccupied;    
            }
        }
        
        public void CreateTopic()
        {
            var slot = _slots.First(x => x.IsSelected);
            if (slot.Status is not SlotStatus.Unoccupied) return;

            var duration = _messageGenerator.RollDuration();
            var messages = _messageGenerator.Generate(duration);

            slot.Topic = new Topic(duration, messages);
            slot.Status = SlotStatus.Occupied;

            OnTopicOpened?.Invoke(slot.Topic);
        }
    }
}