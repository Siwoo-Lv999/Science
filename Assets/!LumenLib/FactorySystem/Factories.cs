using System.Collections.Generic;
using UnityEngine;

namespace _LumenLib.FactorySystem
{
    public class Factories : MonoBehaviour
    {
        private static Dictionary<FactoryKey, object> _factories = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            _factories.Clear();
        }

        public static void Register<TProduct, TRequest>(IFactory<TProduct, TRequest> factory)
        {
            _factories.Add(FactoryKey.Of<TProduct, TRequest>(), factory);
        }

        public static void UnRegister<TProduct, TRequest>()
        {
            _factories.Remove(FactoryKey.Of<TProduct, TRequest>());
        }

        public static IFactory<TProduct, TRequest> GetFactory<TProduct, TRequest>()
        {
            object factory;

            if (!_factories.TryGetValue(FactoryKey.Of<TProduct, TRequest>(), out factory))
            {
                throw new KeyNotFoundException();
            }
            
            return factory as IFactory<TProduct, TRequest>;
        }
    }
}