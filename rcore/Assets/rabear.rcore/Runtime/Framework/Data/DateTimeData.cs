/***
 * Author RadBear - nbhung71711@gmail.com - 2018
 **/

using System;
using UnityEngine;

namespace RCore.Framework.Data
{
    public class DateTimeData : FunData
    {
        private DateTime? m_value;
        private DateTime? m_defaultValue;
        private DateTime? m_compareValue; //If value is changed inside it, like AddDays or AddSeconds
        private bool m_changed;

        public DateTime? Value
        {
            get => m_value;
            set
            {
                if (value != m_value)
                {
                    m_value = value;
                    m_compareValue = value;
                    m_changed = true;
                }
            }
        }

        public DateTimeData(int pId, DateTime? pDefaultValue = null) : base(pId)
        {
            m_defaultValue = pDefaultValue;
        }

        public override void Load(string pBaseKey, string pSaverIdString)
        {
            base.Load(pBaseKey, pSaverIdString);

            m_value = GetSavedValue();
            m_compareValue = m_value;
        }

        public void AddYears(int pYears)
        {
            if (m_value == null)
                return;
            m_value.Value.AddYears(pYears);
            m_changed = true;
        }

        public void AddMonths(int pMonths)
        {
            if (m_value == null)
                return;
            m_value.Value.AddMonths(pMonths);
            m_changed = true;
        }

        public void AddDays(int pDays)
        {
            if (m_value == null)
                return;
            m_value.Value.AddDays(pDays);
            m_changed = true;
        }

        public void AddHours(int pHours)
        {
            if (m_value == null)
                return;
            m_value.Value.AddHours(pHours);
            m_changed = true;
        }

        public void AddMinutes(int pMinutes)
        {
            if (m_value == null)
                return;
            m_value.Value.AddMinutes(pMinutes);
            m_changed = true;
        }

        public void AddSeconds(int pSeconds)
        {
            if (m_value == null)
                return;
            m_value.Value.AddSeconds(pSeconds);
            m_changed = true;
        }

        public override bool Stage()
        {
            if (m_compareValue != m_value || m_changed)
            {
                SetStringValue(m_value == null ? "" : m_value.Value.ToString());
                m_compareValue = m_value;
                m_changed = false;
                return true;
            }
            return false;
        }

        private DateTime? GetSavedValue()
        {
            string val = GetStringValue();
            if (string.IsNullOrEmpty(val))
                return m_defaultValue;

            var output = DateTime.MinValue;
            if (DateTime.TryParse(val, out output))
            {
                return output;
            }
            else
            {
                Debug.LogError("can not parse key " + m_Key + " with value " + val + " to int");
                return m_defaultValue;
            }
        }

        public override void Reload()
        {
            base.Reload();
            m_value = GetSavedValue();
            m_compareValue = m_value;
            m_changed = false;
        }

        public override void Reset()
        {
            Value = m_defaultValue;
        }

        public override bool Cleanable()
        {
            if (m_Index != -1 && Value == m_defaultValue)
            {
                return true;
            }
            return false;
        }

        public bool IsNullOrEmpty()
        {
            return Value == null || Value.ToString() == "";
        }
    }
}