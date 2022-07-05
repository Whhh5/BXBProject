using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BXB
{
    namespace Core
    {
        public abstract partial class MiBaseClass
        {
            protected MiTPool<GameObject> ObjPool => MiPool.Instance.PoolObj;
            protected void Log(Color color, params object[] parameter)
            {
                string str = "";
                foreach (var para in parameter)
                {
                    str += (string)para;
                }
                var col = ColorUtility.ToHtmlStringRGBA(color);
                Debug.Log($"{string.Format("<color=#FFFF00FF>{0}</color>", GetType())}:  {string.Format("<color=#{0}>{1}</color>", col, str)}");
            }
            protected void LogError(string str)
            {
                Debug.LogError($"{GetType()} -- {str}");
            }
            protected async Task AsyncDefaule()
            {
                await Task.Delay(TimeSpan.Zero);
            }
        }
        public abstract partial class MiBaseMonoBeHaviourClass : MonoBehaviour 
        {
            protected MiTPool<GameObject> ObjPool => MiPool.Instance.PoolObj;
            protected virtual void Awake()
            {
                InitalizationInteriorParameterAsync().Wait();
                InitalizationInteriorParameter();
            }
            protected virtual void Start()
            {
                //MiAsync.MiAsyncManager.Instance.StartAsync(InitializationAsync);
                InitializationAsync().Wait();
                Initialization();
            }
            #region Async Initialization
            protected virtual async Task InitalizationInteriorParameterAsync()
            {
                await MiAsyncManager.Instance.Default();
            }
            protected virtual async Task InitializationAsync()
            {
                await MiAsyncManager.Instance.Default();
            }
            #endregion
            #region Initialization
            protected virtual void InitalizationInteriorParameter()
            {
                
            }
            protected virtual void Initialization()
            {
            }
            protected virtual void Initialization<T0>(T0 t0)
            {
            }
            protected void Log(Color color, params object[] parameter)
            {
                string str = "";
                foreach (var para in parameter)
                {
                    str += (string)para;
                }
                var col = ColorUtility.ToHtmlStringRGBA(color);
                var data = $"{string.Format("<color=#FFFF00FF>{0}</color>", GetType())}:  {string.Format("<color=#{0}>{1}</color>", col, str)}";
                Debug.Log(data);
            }

            protected void LogError(string str)
            {
                Debug.LogError($"{GetType()} -- {str}");
            }
            protected async Task AsyncDefaule()
            {
                await Task.Delay(TimeSpan.Zero);
            }
            #endregion
        }
        public abstract class MiSingleton<T> : MiBaseClass where T : new()
        {
            public static T Instance = new T();
        }
        public abstract class MiSingletonMonoBeHaviour<T> : MiBaseMonoBeHaviourClass where T : MiSingletonMonoBeHaviour<T>
        {
            public static T Instance = null;
            protected override void Awake()
            {
                base.Awake();
                if(Instance == null) Instance = (T)this;
            }
        }

        public abstract class Dataitembase : ScriptableObject, IDataItemMothodBase
        {
            public abstract List<object> GetData();
        }
    }
}

