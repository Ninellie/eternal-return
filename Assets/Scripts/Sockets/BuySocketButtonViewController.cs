using System.Collections.Generic;
using EternalReturn.Resources_Feature;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Sockets
{
    public class BuySocketButtonViewController : MonoBehaviour
    {
        [SerializeField] private List<Button> buttons;
        [SerializeField] private List<TMP_Text> text;
        [SerializeField] private ResourceConfigs resourceConfigs;
        [SerializeField] private int socketPrice;
        [SerializeField] private bool isCanBuySocket;

        private Resource _intel;
        
        private void OnEnable()
        {
            _intel = resourceConfigs.Intel;
            _intel.OnChange += RefreshView;

            // todo если сокет первый, то он стоит 0
            foreach (var t in text)
            {
                t.text = $"Купить сокет за {socketPrice} знаний";
            }
            
            RefreshView(_intel.Amount);
        }

        private void RefreshView(int intelValue)
        {
            isCanBuySocket = intelValue >= socketPrice;

            foreach (var button in buttons)
            {
                button.interactable = isCanBuySocket;
            }
        }
    }
}