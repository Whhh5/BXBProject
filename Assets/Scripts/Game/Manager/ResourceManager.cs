using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
//using UnityEngine.Rendering.Universal;

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




            Dictionary<string, MiUIDialog> dialogs = new Dictionary<string, MiUIDialog>();
            Dictionary<string, GameObject> objs = new Dictionary<string, GameObject>();

            public T1 GetWorldObject<T1>(string f_path, string f_name, Vector3 f_startPosition, params object[] f_status)
            where T1 : MiObjPoolPublicParameter, ICommon_GameObject
            {
                var path = f_path;
                var prefab = f_name;
                var startPosition = f_startPosition;
                var status = f_status;

                GetOriginal(path, prefab, out GameObject original);
                if (original == null)
                {
                    Log(color: Color.red, path + "/" + prefab);
                    return null;
                }

                var obj = ObjPool.GetObject(original);
                obj.transform.Normalization(null);
                obj.transform.position = startPosition;
                T1 result = obj.GetComponent<T1>();
                if (result != null)
                {
                    obj.SetActive(true);
                    result.GetMain().transform.Normalization(obj.transform);
                    result.SettingId(0);
                    result.Prepare();
                    result.SetParameter(status);
                }
                return result;
            }
            public async Task<T> ShowDialogAsync<T>(string path, string name, CanvasLayer layerGroup, params object[] parameters)
            where T : MiUIDialog
            {
                T obj;
                if (!dialogs.ContainsKey(name))
                {
                    var prefabName = name;
                    var o = await ResourceManager.Instance.loadUIElementAsync<GameObject>(path, prefabName, layerGroup);
                    var dialog = o.GetComponent<MiUIDialog>();
                    dialogs.Add(name, dialog);
                }
                else if (dialogs[name] == null)
                {
                    var prefabName = name;
                    var o = await ResourceManager.Instance.loadUIElementAsync<GameObject>(path, prefabName, layerGroup);
                    var dialog = o.GetComponent<MiUIDialog>();
                    dialogs[name] = dialog;
                }
                obj = (T)dialogs[name];
                obj.Prepare();
                obj.SetParameter(parameters);
                return obj;
            }


            public async Task<T> GetUIElementAsync<T>(string f_path, string f_name, RectTransform f_parent, Vector3 f_anchor, params object[] f_parameter)
            where T : MiObjPoolPublicParameter, IUIElementPoolBase
            {
                var prefabName = f_name;
                var path = f_path;
                var startPosition = f_anchor;
                var parameter = f_parameter;

                GetOriginal(path, prefabName, out GameObject original);
                if (original == null)
                {
                    Log(color: Color.red, f_path + "/" + prefabName);
                    return null;
                }

                var obj = await ObjPool.GetObjectAsync(original);
                var rect = obj.GetComponent<RectTransform>();
                rect.Normalization(f_parent);
                rect.anchoredPosition3D = startPosition;
                T result = obj.GetComponent<T>();
                if (result != null)
                {
                    obj.SetActive(true);
                    result.GetMain().GetComponent<RectTransform>().Normalization(rect);
                    result.SettingId(0);
                    result.Prepare();
                    result.SetParameter(parameter);
                }
                return result;
            }

            public void GetOriginal(string path, string name, out GameObject original)
            {
                string prefabName = name;

                if (objs.ContainsKey(prefabName))
                    if (objs[prefabName] != null)
                        original = objs[prefabName];
                    else
                    {
                        original = ResourceManager.Instance.Load<GameObject>(path, $"{prefabName}");
                        if (original != null)
                            objs[prefabName] = original;
                        else
                            Log(Color.red, $" Absent   Prefab   Path   {path}/{prefabName}");
                    }
                else
                {
                    original = ResourceManager.Instance.Load<GameObject>(path, $"{prefabName}");
                    if (original != null)
                        objs.Add(prefabName, original);
                    else
                        Log(Color.red, $" Absent   Prefab   Path   {path}/{prefabName}");
                }
            }
        }
    }
}
