using System;
using DTT.Utils.Extensions;
using UnityEngine;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Used to make sure you're desired UI elements will be kept in view for the user when the keyboard covers the screen
    /// </summary>
    public class UIKeyboardRaiser : MonoBehaviour
    {
        /// <summary>
        /// If enabled the object being raised will be moved smoothly instead of instantly.
        /// </summary>
        public bool IsSmooth
        {
            get => _isSmooth;
            set => _isSmooth = value;
        }

        /// <summary>
        /// The amount of padding on the bottom from where the object should start being dragged by the keyboard.
        /// </summary>
        public float Padding
        {
            get => _padding;
            set => _padding = value;
        }

        /// <summary>
        /// The keyboard state that's being used to gain keyboard information.
        /// </summary>
        public IKeyboardState KeyboardState
        {
            get => _keyboardState;
            set => _keyboardState = value ?? new NullKeyboardState();
        }

        /// <summary>
        /// The amount of padding on the bottom from where the object should start being dragged by the keyboard.
        /// </summary>
        [SerializeField]
        private float _padding;

        /// <summary>
        /// If enabled the object being raised will be moved smoothly instead of instantly.
        /// </summary>
        [SerializeField]
        private bool _isSmooth;
        
        /// <summary>
        /// The state that's being used to gain keyboard information.
        /// </summary>
        private IKeyboardState _keyboardState;
        
        /// <summary>
        /// The position before the raising so we know where to return to after keyboard lowering.
        /// </summary>
        private Vector3 _originalPosition;
        
        /// <summary>
        /// The rect before the raising so we know where to return to after keyboard lowering.
        /// </summary>
        private Rect _originalRect;
        
        /// <summary>
        /// The canvas this component is being contained in.
        /// </summary>
        private Canvas _canvas;
        
        /// <summary>
        /// The position we want to go to when the keyboard raises, so we can move there slowly.
        /// </summary>
        private Vector3 _targetPos;
        
        /// <summary>
        /// The rect transform of this game object.
        /// </summary>
        private RectTransform _rectTransform;

        /// <summary>
        /// The time when the keyboard was lowered last.
        /// </summary>
        private float _timeOfLastLowering;
        
        private const float TIMEOUT_DURATION = 0.5f;

        /// <summary>
        /// Retrieves original data.
        /// <remarks>This is done in Start since in OnEnable canvas positioning doesn't seem to be correct.</remarks>
        /// </summary>
        private void Start()
        {
            _originalPosition = transform.position;
            _originalRect = _rectTransform.GetWorldRect();
        }

        /// <summary>
        /// Retrieve references and setup event handlers.
        /// </summary>
        private void OnEnable()
        {
            _canvas = GetComponentInParent<Canvas>();
            _rectTransform = transform.GetRectTransform();
            _keyboardState = KeyboardStateManager.Current;
            _originalPosition = transform.position;
            _originalRect = _rectTransform.GetWorldRect();

            _keyboardState.Raised += OnKeyboardRaised;
            _keyboardState.Lowered += OnKeyboardLowered;
        }

        /// <summary>
        /// Remove event handlers.
        /// </summary>
        private void OnDisable()
        {
            _keyboardState.Raised -= OnKeyboardRaised;
            _keyboardState.Lowered -= OnKeyboardLowered;
        }

        /// <summary>
        /// Save state of <see cref="_rectTransform"/> to revert to later.
        /// </summary>
        private void OnKeyboardRaised()
        {
            if (Time.time - _timeOfLastLowering > TIMEOUT_DURATION)
            {
                _originalPosition = transform.position;
                _originalRect = _rectTransform.GetWorldRect();
            }
        }
        
        /// <summary>
        /// Save time when keyboard was closed.
        /// </summary>
        private void OnKeyboardLowered()
        {
            _timeOfLastLowering = Time.time;
        }

        /// <summary>
        /// Updates the position of the <see cref="_rectTransform"/> based on the state of keyboard.
        /// </summary>
        private void Update()
        {
            if (Time.time - _timeOfLastLowering > TIMEOUT_DURATION && !_keyboardState.IsRaised)
                return;
            
            float delta = 0;
            if (_canvas != null && _keyboardState.IsRaised)
            {
                Rect canvasRect = _canvas.GetRectTransform().GetWorldRect();

                delta = _keyboardState.ProportionalHeight * canvasRect.height - (_originalRect.yMin - _padding * transform.lossyScale.y);
            }

            _targetPos = _originalPosition + Vector3.up * Mathf.Max(delta, 0);
            transform.position = Vector3.Lerp(transform.position, _targetPos, _isSmooth ? Time.deltaTime * 10 : 1);
        }
    }
}