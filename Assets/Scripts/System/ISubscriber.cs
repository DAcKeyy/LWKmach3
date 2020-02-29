using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWT.System
{
    public interface ISubscriber
    {
        void Subscribe<T>(Action<T> handler);
        void UnSubscribe<T>(Action<T> handler);
    }
}