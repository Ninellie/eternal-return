using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalReturn
{
    [CreateAssetMenu(menuName = "Eternal Return/Resource Repository")]
    public class ResourceRepository : ScriptableObject
    {
        [SerializeField] private List<Resource> resources;

        public Resource GetByName(string resourceName)
        {
            return resources.First(x => x.Name == resourceName);
        }
    }
}