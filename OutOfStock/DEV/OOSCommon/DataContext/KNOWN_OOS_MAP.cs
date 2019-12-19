//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace OOSCommon.DataContext
{
    public partial class KNOWN_OOS_MAP
    {
        #region Primitive Properties
    
        public virtual int ID
        {
            get;
            set;
        }
    
        public virtual int REGION_ID
        {
            get { return _rEGION_ID; }
            set
            {
                try
                {
                    _settingFK = true;
                    if (_rEGION_ID != value)
                    {
                        if (REGION != null && REGION.ID != value)
                        {
                            REGION = null;
                        }
                        _rEGION_ID = value;
                    }
                }
                finally
                {
                    _settingFK = false;
                }
            }
        }
        private int _rEGION_ID;
    
        public virtual Nullable<int> KNOWN_OOS_HEADER_ID
        {
            get { return _kNOWN_OOS_HEADER_ID; }
            set
            {
                try
                {
                    _settingFK = true;
                    if (_kNOWN_OOS_HEADER_ID != value)
                    {
                        if (KNOWN_OOS_HEADER != null && KNOWN_OOS_HEADER.ID != value)
                        {
                            KNOWN_OOS_HEADER = null;
                        }
                        _kNOWN_OOS_HEADER_ID = value;
                    }
                }
                finally
                {
                    _settingFK = false;
                }
            }
        }
        private Nullable<int> _kNOWN_OOS_HEADER_ID;
    
        public virtual string VENDOR_KEY
        {
            get;
            set;
        }
    
        public virtual string CREATED_BY
        {
            get;
            set;
        }
    
        public virtual System.DateTime CREATED_DATE
        {
            get;
            set;
        }
    
        public virtual string LAST_UPDATED_BY
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> LAST_UPDATED_DATE
        {
            get;
            set;
        }

        #endregion

        #region Navigation Properties
    
        public virtual KNOWN_OOS_HEADER KNOWN_OOS_HEADER
        {
            get { return _kNOWN_OOS_HEADER; }
            set
            {
                if (!ReferenceEquals(_kNOWN_OOS_HEADER, value))
                {
                    var previousValue = _kNOWN_OOS_HEADER;
                    _kNOWN_OOS_HEADER = value;
                    FixupKNOWN_OOS_HEADER(previousValue);
                }
            }
        }
        private KNOWN_OOS_HEADER _kNOWN_OOS_HEADER;
    
        public virtual REGION REGION
        {
            get { return _rEGION; }
            set
            {
                if (!ReferenceEquals(_rEGION, value))
                {
                    var previousValue = _rEGION;
                    _rEGION = value;
                    FixupREGION(previousValue);
                }
            }
        }
        private REGION _rEGION;

        #endregion

        #region Association Fixup
    
        private bool _settingFK = false;
    
        private void FixupKNOWN_OOS_HEADER(KNOWN_OOS_HEADER previousValue)
        {
            if (previousValue != null && previousValue.KNOWN_OOS_MAP.Contains(this))
            {
                previousValue.KNOWN_OOS_MAP.Remove(this);
            }
    
            if (KNOWN_OOS_HEADER != null)
            {
                if (!KNOWN_OOS_HEADER.KNOWN_OOS_MAP.Contains(this))
                {
                    KNOWN_OOS_HEADER.KNOWN_OOS_MAP.Add(this);
                }
                if (KNOWN_OOS_HEADER_ID != KNOWN_OOS_HEADER.ID)
                {
                    KNOWN_OOS_HEADER_ID = KNOWN_OOS_HEADER.ID;
                }
            }
            else if (!_settingFK)
            {
                KNOWN_OOS_HEADER_ID = null;
            }
        }
    
        private void FixupREGION(REGION previousValue)
        {
            if (previousValue != null && previousValue.KNOWN_OOS_MAP.Contains(this))
            {
                previousValue.KNOWN_OOS_MAP.Remove(this);
            }
    
            if (REGION != null)
            {
                if (!REGION.KNOWN_OOS_MAP.Contains(this))
                {
                    REGION.KNOWN_OOS_MAP.Add(this);
                }
                if (REGION_ID != REGION.ID)
                {
                    REGION_ID = REGION.ID;
                }
            }
        }

        #endregion

    }
}
