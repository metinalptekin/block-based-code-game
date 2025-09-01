using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using MG_BlocksEngine2.DragDrop;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;
using MG_BlocksEngine2.Utils;

namespace MG_BlocksEngine2.Core
{
    public class BE2_ExecutionManager : MonoBehaviour
    {
        List<I_BE2_TargetObject> _targetObjectsList;
        List<I_BE2_ProgrammingEnv> _programmingEnvsList;
        public List<I_BE2_ProgrammingEnv> ProgrammingEnvsList => _programmingEnvsList;
        public I_BE2_BlocksStack[] blocksStacksArray;

        static BE2_ExecutionManager _instance;
        public static BE2_ExecutionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<BE2_ExecutionManager>();
                return _instance;
            }
            set => _instance = value;
        }

        BE2_Pointer _pointer;
        I_BE2_InputManager _inputManager;
        List<UnityAction> _actions = new List<UnityAction>();
        UnityEvent OnUpdate = new UnityEvent();
        UnityEvent OnLateUpdate = new UnityEvent();

        void Awake()
        {
            _pointer = BE2_Pointer.Instance;
            _inputManager = BE2_InputManager.Instance;
            UpdateTargetObjects();
            UpdateProgrammingEnvsList();
            Instance = this;
        }

        void Start()
        {
            UpdateBlocksStackList();
        }

        void Update()
        {
            _pointer.OnUpdate();
            _inputManager.OnUpdate();
#if !BE2_FIXED_UPDATE_INSTRUCTIONS
            OnUpdate.Invoke();
#endif
        }

#if BE2_FIXED_UPDATE_INSTRUCTIONS
        void FixedUpdate()
        {
            OnUpdate.Invoke();
        }
#endif

        void LateUpdate()
        {
            OnLateUpdate.Invoke();
        }

        public void AddToUpdate(UnityAction action)
        {
            if (!_actions.Contains(action))
            {
                OnUpdate.AddListener(action);
                _actions.Add(action);
            }
        }

        public void RemoveFromUpdate(UnityAction action)
        {
            if (_actions.Contains(action))
            {
                OnUpdate.RemoveListener(action);
                _actions.Remove(action);
            }
        }

        public void AddToLateUpdate(UnityAction action)
        {
            if (!_actions.Contains(action))
            {
                OnLateUpdate.AddListener(action);
                _actions.Add(action);
            }
        }

        public void RemoveFromLateUpdate(UnityAction action)
        {
            if (_actions.Contains(action))
            {
                OnLateUpdate.RemoveListener(action);
                _actions.Remove(action);
            }
        }

        public void Play()
        {
            BE2_MainEventsManager.Instance.TriggerEvent(BE2EventTypes.OnPlay);
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void Stop()
        {
            BE2_MainEventsManager.Instance.TriggerEvent(BE2EventTypes.OnStop);
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void UpdateBlocksStackList()
        {
            blocksStacksArray = new I_BE2_BlocksStack[0];
            int envsCount = _programmingEnvsList.Count;
            for (int i = 0; i < envsCount; i++)
            {
                I_BE2_ProgrammingEnv programmingEnv = _programmingEnvsList[i];
                int childCount = programmingEnv.Transform.childCount;
                for (int j = 0; j < childCount; j++)
                {
                    I_BE2_BlocksStack blocksStack = programmingEnv.Transform.GetChild(j).GetComponent<I_BE2_BlocksStack>();
                    if (blocksStack != null)
                    {
                        BE2_ArrayUtils.Add(ref blocksStacksArray, blocksStack);
                        blocksStack.TargetObject = programmingEnv.TargetObject;
                        AddToUpdate(blocksStack.Execute);
                    }
                }
            }
            BE2_MainEventsManager.Instance.TriggerEvent(BE2EventTypes.OnBlocksStackArrayUpdate);
        }

        public void AddToBlocksStackArray(I_BE2_BlocksStack blocksStack, I_BE2_TargetObject targetObject)
        {
            if (BE2_ArrayUtils.FindAll(ref blocksStacksArray, x => x == blocksStack).Length == 0)
            {
                BE2_ArrayUtils.Add(ref blocksStacksArray, blocksStack);
                blocksStack.TargetObject = targetObject;
                BE2_MainEventsManager.Instance.TriggerEvent(BE2EventTypes.OnBlocksStackArrayUpdate);
                AddToUpdate(blocksStack.Execute);
            }
        }

        public void RemoveFromBlocksStackList(I_BE2_BlocksStack blocksStack)
        {
            if (BE2_ArrayUtils.FindAll(ref blocksStacksArray, x => x == blocksStack).Length > 0)
            {
                BE2_ArrayUtils.Remove(ref blocksStacksArray, blocksStack);
                BE2_MainEventsManager.Instance.TriggerEvent(BE2EventTypes.OnBlocksStackArrayUpdate);
                RemoveFromUpdate(blocksStack.Execute);
            }
        }

        void UpdateTargetObjects()
        {
            _targetObjectsList = new List<I_BE2_TargetObject>();
            foreach (var go in GameObject.FindObjectsOfType<GameObject>())
            {
                I_BE2_TargetObject targetObject = go.GetComponent<I_BE2_TargetObject>();
                if (targetObject != null)
                    _targetObjectsList.Add(targetObject);
            }
        }

        public void UpdateProgrammingEnvsList()
        {
            _programmingEnvsList = new List<I_BE2_ProgrammingEnv>();
            foreach (var go in GameObject.FindObjectsOfType<GameObject>())
            {
                I_BE2_ProgrammingEnv programmingEnv = go.GetComponent<I_BE2_ProgrammingEnv>();
                if (programmingEnv != null)
                    _programmingEnvsList.Add(programmingEnv);
            }
        }

        public void ResetGame()
        {
            foreach (var target in _targetObjectsList)
            {
                if (target is BE2_TargetObjectSpacecraft3D spacecraft)
                {
                    spacecraft.ResetToStartPosition();
                    var spriteRenderer = spacecraft.Transform.GetComponentInChildren<SpriteRenderer>();
                    if (spriteRenderer != null)
                        spriteRenderer.color = Color.white;
                    Debug.Log("Oyun ve hedefler sıfırlandı.");
                }
            }
        }
    }
}
