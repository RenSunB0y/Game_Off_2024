using Assets.Scripts.Animation;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Desk.Objects
{
    public class Rope : InteractableObject
    {
        [SerializeField] private Vector2 ropeVerticalBounds = new Vector2(-1, 1);

        private AudioSource m_audioSource_rope;
        float cursorPositionY;


        private void Awake()
        {
            m_audioSource_rope = GetComponent<AudioSource>();
        }


/*        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {            
            base.OnPointerExit(eventData);
        }*/

        public override void OnEndDrag()
        {
            if (cursorPositionY < 4)
            {
                m_audioSource_rope.clip = base.onDragSound;
                m_audioSource_rope.Play();

            }
        }

        public override void OnDragUpdate()
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 newPosition = new Vector2(cursorPos.x, cursorPos.y) - _mousePositionFromObject;

            float xPosition = Mathf.Clamp(newPosition.x, transform.position.x, transform.position.x);
            float yPosition = Mathf.Clamp(newPosition.y, _basePosition.y + ropeVerticalBounds.x, _basePosition.y + ropeVerticalBounds.y);
            cursorPositionY = yPosition;
            transform.position = new Vector2 (xPosition, yPosition);
        }
    }
}