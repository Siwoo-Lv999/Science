using System;

namespace _LumenLib.FactorySystem
{
    public struct FactoryKey : IEquatable<FactoryKey>
    {
        private readonly Type _product;
        private readonly Type _request;

        public static FactoryKey Of<TProduct, TRequest>()
        {
            return new(typeof(TProduct), typeof(TRequest));
        }

        private FactoryKey(Type product, Type request)
        {
            _product = product;
            _request = request;
        }
        
        public bool Equals(FactoryKey other)
        {
            return _product.Equals(other._product) && _product.Equals(other._request);
        }

        public override bool Equals(object obj)
        {
            return obj is FactoryKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_product.GetHashCode(), _request.GetHashCode());
        }
    }
}