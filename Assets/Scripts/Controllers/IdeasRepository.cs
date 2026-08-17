using System.Collections.Generic;
using UnityEngine;

namespace EternalReturn.Controllers
{
    [CreateAssetMenu(menuName = "Eternal Return/Ideas Repository")]
    public class IdeasRepository : ScriptableObject
    {
        [SerializeField] private List<Idea> ideas;

        public Idea GetRandomIdea()
        {
            var r = Random.Range(0, ideas.Count);
            return ideas[r];
        }
    }
}