using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWR.System
{
    /// <summary>
    /// Реализация паттерна синглтон
    /// </summary>
    /// <typeparam name="T">Класс который должен быть в одном экземпляре</typeparam>
    public abstract class Sington<T> where T : new()
    {
        public static T Instatiate { get => instatiate; }
        private static T instatiate = new T();
    }
}