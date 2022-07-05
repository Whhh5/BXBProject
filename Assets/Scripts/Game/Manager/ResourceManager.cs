using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

namespace BXB
{
    namespace Core
    {
        public class ResourceManager : MiSingleton<ResourceManager>
        {
            public async Task<T> LoadAsync<T>(string filePath, string name,bool isInstantiate = false,RectTransform rectTr = null,Transform trTr = null) 
                where T : UnityEngine.Object
            {
                await Task.Delay(System.TimeSpan.Zero);
                string paths = $"{filePath}/{name}";
                T obj = Resources.Load<T>(paths);
                if (isInstantiate)
                {
                    obj = (await MiFactory.Instance.InstantiateAsync(obj, rectTr, trTr)) as T;
                    obj.name = name;
                }
                return obj;
            }
            public async Task<T> loadUIElementAsync<T>(string filePath,string name, CanvasLayer layer) where T : UnityEngine.Object
            {
                var parent = await UISceneManager.Instance.GetCanvasRectAsync(layer);
                T obj = await LoadAsync<T>(filePath, name, true,rectTr: parent);
                return obj;
            }



            public T Load<T>(string filePath, string name, bool isInstantiate = false, RectTransform rectTr = null, Transform trTr = null) where T : UnityEngine.Object
            {
                string paths = $"{filePath}/{name}";
                T obj = Resources.Load<T>(paths);
                if (isInstantiate)
                {
                    obj = MiFactory.Instance.Instantiate(obj, rectTr, trTr) as T;
                    obj.name = name;
                }
                return obj;
            }

            public T loadUIElement<T>(string filePath, string name, CanvasLayer layer) where T : Object
            {
                var parent = UISceneManager.Instance.GetCanvasRect(layer);
                T obj = Load<T>(filePath, name, true, rectTr: parent);

                return obj;
            }

            public async Task<AsyncOperation> LoadSceneAsync(string name, LoadSceneMode mode)
            {
                await AsyncDefaule();
                //SceneManager.UnloadSceneAsync(name, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                var operation = SceneManager.LoadSceneAsync(name, mode);
                return operation;
            }
        }
    }
}
