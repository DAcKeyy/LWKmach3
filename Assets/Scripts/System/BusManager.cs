using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWT.System
{
    public class MessageBusManager : IPublisher, ISubscriber
    {
        private Dictionary<Type, Action<object>> subscriders = new Dictionary<Type, Action<object>>();

        /// <summary>
        /// Подпись на рассылку сообщения  
        /// </summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="handler">Обработчик</param>
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

        /// <summary>
        /// Отпись от сообщения 
        /// </summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="handler">Обработчик</param>
        public void UnSubscribe<T>(Action<T> handler)
        {
            //Реализую позже
        }

        /// <summary>
        /// Публикация сообщения
        /// </summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="message">Сообщение</param>
        public void Publish<T>(T message)
        {
            if (subscriders.ContainsKey(typeof(T)))
            {
                subscriders[typeof(T)].Invoke(message);
            }
        }
    }
}
