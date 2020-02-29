using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWT.System
{
    public interface IPublisher
    {
        void Publish<T>(T message);
    }
}