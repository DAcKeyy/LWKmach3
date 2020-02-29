using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWR.System
{
    public class MessageBusManager : Sington<MessageBusManager>, IPublisher, ISubscriber
    {
        private Dictionary<Type, Action<object>> subscriders = new Dictionary<Type, Action<object>>();

        public void Subscribe<T>(Action<T> handler)
        {
            if (subscriders == null)
            {
                return;
            }

            if (!subscriders.ContainsKey(typeof(T)))
            {
                subscriders[typeof(T)] = o => { };
            }

            subscriders[typeof(T)] =
                (Action<object>)Delegate.Combine(subscriders[typeof(T)], (Action<object>)(o => { handler((T)o); }));
        }
        public void UnSubscribe<T>(Action<T> handler)
        {

        }

        public void Publish<T>(T message)
        {
            if (subscriders.ContainsKey(typeof(T)))
            {
                subscriders[typeof(T)].Invoke(message);
            }
        }
    }
}
