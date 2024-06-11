using System;
using System.Collections.Generic;

namespace DI
{
    public class DIContainer
    {
        private readonly DIContainer _parentContainer;
        private readonly Dictionary<(string, Type), DIRegistration> _registrations = new();
        private readonly HashSet<(string, Type)> _resoulutions = new();

        public DIContainer(DIContainer parentContainer = null) 
        {
            _parentContainer = parentContainer;
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            RegisterSingleton(null, factory);
        }

        public void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Registor(key, factory, true);
        }

        public void RegisterTransient<T>(Func<DIContainer, T> factory)
        {
            RegisterTransient(null, factory);
        }

        public void RegisterTransient<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Registor(key, factory, false);
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance(null, instance);
        }

        public void RegisterInstance<T>(string tag, T instance)
        {
            var key = (tag, typeof(T));

            if (_registrations.ContainsKey(key))
            {
                throw new Exception($"DI: Factory with tag {key.Item1} and type {key.Item2.FullName} has already registration");
            }

            _registrations[key] = new DIRegistration
            {
                Instance = instance,
                IsSingleton = true
            };
        }

        public T Resolve<T>(string tag = null)
        {
            var key = (tag, typeof(T));

            if (_resoulutions.Contains(key))
            {
                throw new Exception($"Cyclic dependency for tag {tag} and type {key.Item2.FullName}");
            }

            _resoulutions.Add(key);

            try
            {
                if (_registrations.TryGetValue(key, out var registration))
                {
                    if (registration.IsSingleton)
                    {
                        if (registration.Instance == null && registration.Factory != null)
                        {
                            registration.Instance = registration.Factory(this);
                        }

                        return (T)registration.Instance;
                    }

                    return (T)registration.Factory(this);
                }

                if (_parentContainer != null)
                {
                    return _parentContainer.Resolve<T>(tag);
                }
            }
            finally 
            {
                _resoulutions.Remove(key);
            }

            throw new Exception($"Couldn't find dependency for tag {tag} and type {key.Item2.FullName}");
        }

        private void Registor<T>((string, Type) key, Func<DIContainer, T> factory, bool isSingleton)
        {
            if (_registrations.ContainsKey(key))
            {
                throw new Exception($"DI: Factory with tag {key.Item1} and type {key.Item2.FullName} has already registration");
            }

            _registrations[key] = new DIRegistration
            {
                Factory = c => factory,
                IsSingleton = isSingleton
            };
        }
    }
}