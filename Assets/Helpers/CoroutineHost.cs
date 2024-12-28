using System;
using System.Collections;
using UnityEngine;

namespace Helpers
{
    public sealed class CoroutineHost : MonoSingleton<CoroutineHost>
    {
        private static MonoBehaviour Host => Instance;
        public static IDisposable StartDisposable(IEnumerator coroutine) => new DisposableCoroutine(Host.StartCoroutine(coroutine));
        public new static Coroutine StartCoroutine(IEnumerator coroutine) => Host.StartCoroutine(coroutine);
        public new static void StopCoroutine(IEnumerator coroutine) => Host.StopCoroutine(coroutine);
        public new static void StopCoroutine(Coroutine coroutine) => Host.StopCoroutine(coroutine);
        public new static void Invoke(string methodName, float time) => Host.Invoke(methodName, time);

        private struct DisposableCoroutine : IDisposable
        {
            public Coroutine coroutine;
            private bool _disposed;
            public DisposableCoroutine(Coroutine coro)
            {
                _disposed = false;
                coroutine = coro;
            }
            public void Dispose()
            {
                if (_disposed) return;
                _disposed = true;
                StopCoroutine(coroutine);
            }
        }
    }
}
