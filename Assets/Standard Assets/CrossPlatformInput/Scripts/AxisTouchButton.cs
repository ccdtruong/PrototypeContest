using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class AxisTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
        // designed to work in a pair with another axis touch button
        // (typically with one having -1 and one having 1 axisValues)
        public string axisName = "Horizontal"; // The name of the axis
        public float axisValue = 1; // The axis that the value has
        public float responseSpeed = 3; // The speed at which the axis touch button responds
        public float returnToCentreSpeed = 3; // The speed at which the button will return to its centre

        private AxisTouchButton m_PairedWith;
        private CrossPlatformInputManager.VirtualAxis m_Axis;

        public bool IsActivatingOnDragEnter;
        [HideInInspector] public bool IsButtonPressed;

        private void Update()
        {
            if (IsButtonPressed)
            {
                m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, axisValue, responseSpeed * Time.deltaTime));
            }
            else
            {
                if (!m_PairedWith.IsButtonPressed)
                {
                    m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, 0, returnToCentreSpeed / 2f * Time.deltaTime));
                }
            }
        }

        private void OnEnable()
        {
            if (!CrossPlatformInputManager.AxisExists(axisName))
            {
                m_Axis = new CrossPlatformInputManager.VirtualAxis(axisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_Axis);
            }
            else
            {
                m_Axis = CrossPlatformInputManager.VirtualAxisReference(axisName);
            }
            FindPairedButton();
        }

        public void OnPointerDown(PointerEventData data)
        {
            MakeButtonPressed();
        }

        public void OnPointerUp(PointerEventData data)
        {
            IsButtonPressed = false;
        }

        public void OnPointerExit(PointerEventData data)
        {
            IsButtonPressed = false;
        }

        public void OnPointerEnter(PointerEventData data)
        {
            if (IsActivatingOnDragEnter)
            {
                MakeButtonPressed();
            }
        }

        private void FindPairedButton()
        {
            var otherAxisButtons = FindObjectsOfType(typeof(AxisTouchButton)) as AxisTouchButton[];

            if (otherAxisButtons != null)
            {
                for (int i = 0; i < otherAxisButtons.Length; i++)
                {
                    if (otherAxisButtons[i].axisName == axisName && otherAxisButtons[i] != this)
                    {
                        m_PairedWith = otherAxisButtons[i];
                    }
                }
            }
        }

        private void OnDisable()
        {
            m_Axis.Remove();
        }

        private void MakeButtonPressed()
        {
            if (m_PairedWith == null)
            {
                FindPairedButton();
            }
            IsButtonPressed = true;
        }
    }

}