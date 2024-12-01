﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Animation;

namespace Assets.Scripts.Desk.Objects
{
    [RequireComponent(typeof(Collider2D), typeof(AudioSource))]
    public abstract class InteractableObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer highlightsSpriteRenderer;

        [Header("Animation")]
        [SerializeField] private Animation.Animation m_animation;

        [Header("Sprites")]
        [SerializeField] private Sprite spriteHighlight0;
        [SerializeField] private Sprite spriteHighlight1;

        [Header("Sound effects")]
        [SerializeField] private AudioClip onTakeSound;
        [SerializeField] private AudioClip onDragSound;

        private bool _canHover = true;
        private bool _isDragging = false;

        protected Vector2 _basePosition;
        protected Vector2 _mousePositionFromObject;

        private AudioSource m_audioSource;

        protected virtual void Start()
        {
            m_animation.UpdateAction = UpdateAnimation;
            m_animation.EndAction = EndAnimation;

            _basePosition = transform.position;

            m_audioSource = GetComponent<AudioSource>();
        }

        protected virtual void Update()
        {
            if (_canHover)
            {
                m_animation.Update(Time.deltaTime);
            }

            if (_isDragging)
            {
                OnDragUpdate();
            }
        }

        public virtual void OnBeginDrag()
        {
            if (onTakeSound)
            {
                m_audioSource.clip = onTakeSound;
                m_audioSource.Play();
            }
        }

        public virtual void OnEndDrag()
        {
            if (onDragSound)
            {
                m_audioSource.clip = onDragSound;
                m_audioSource.Play();
            }
        }

        public virtual void OnDragUpdate()
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x, cursorPos.y) - _mousePositionFromObject;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!_canHover)
                return;

            highlightsSpriteRenderer.enabled = true;
            highlightsSpriteRenderer.sprite = spriteHighlight0;

            m_animation.StartAnimation();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!_canHover)
                return;

            highlightsSpriteRenderer.enabled = false;
        }

        private void UpdateAnimation(float t)
        {
            Sprite newSprite = spriteHighlight0;

            if (t >= .5f)
            {
                newSprite = spriteHighlight1;
            }

            highlightsSpriteRenderer.sprite = newSprite;
        }

        private void EndAnimation()
        {
            m_animation.StartAnimation();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _canHover = false;
            _isDragging = true;

            highlightsSpriteRenderer.enabled = false;

            _mousePositionFromObject = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            OnBeginDrag();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _canHover = true;
            _isDragging = false;

            transform.position = _basePosition;

            OnEndDrag();
        }
    }
}