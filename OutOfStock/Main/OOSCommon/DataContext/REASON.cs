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
    public partial class REASON
    {
        #region Primitive Properties
    
        public virtual int ID
        {
            get;
            set;
        }
    
        public virtual string REASON_DESCRIPTION
        {
            get;
            set;
        }
    
        public virtual string CREATED_BY
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> CREATED_DATE
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
    
        public virtual ICollection<KNOWN_OOS_DETAIL> KNOWN_OOS_DETAIL
        {
            get
            {
                if (_kNOWN_OOS_DETAIL == null)
                {
                    var newCollection = new FixupCollection<KNOWN_OOS_DETAIL>();
                    newCollection.CollectionChanged += FixupKNOWN_OOS_DETAIL;
                    _kNOWN_OOS_DETAIL = newCollection;
                }
                return _kNOWN_OOS_DETAIL;
            }
            set
            {
                if (!ReferenceEquals(_kNOWN_OOS_DETAIL, value))
                {
                    var previousValue = _kNOWN_OOS_DETAIL as FixupCollection<KNOWN_OOS_DETAIL>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupKNOWN_OOS_DETAIL;
                    }
                    _kNOWN_OOS_DETAIL = value;
                    var newValue = value as FixupCollection<KNOWN_OOS_DETAIL>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupKNOWN_OOS_DETAIL;
                    }
                }
            }
        }
        private ICollection<KNOWN_OOS_DETAIL> _kNOWN_OOS_DETAIL;
    
        public virtual ICollection<REPORT_DETAIL> REPORT_DETAIL
        {
            get
            {
                if (_rEPORT_DETAIL == null)
                {
                    var newCollection = new FixupCollection<REPORT_DETAIL>();
                    newCollection.CollectionChanged += FixupREPORT_DETAIL;
                    _rEPORT_DETAIL = newCollection;
                }
                return _rEPORT_DETAIL;
            }
            set
            {
                if (!ReferenceEquals(_rEPORT_DETAIL, value))
                {
                    var previousValue = _rEPORT_DETAIL as FixupCollection<REPORT_DETAIL>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupREPORT_DETAIL;
                    }
                    _rEPORT_DETAIL = value;
                    var newValue = value as FixupCollection<REPORT_DETAIL>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupREPORT_DETAIL;
                    }
                }
            }
        }
        private ICollection<REPORT_DETAIL> _rEPORT_DETAIL;

        #endregion
        #region Association Fixup
    
        private void FixupKNOWN_OOS_DETAIL(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (KNOWN_OOS_DETAIL item in e.NewItems)
                {
                    item.REASON = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (KNOWN_OOS_DETAIL item in e.OldItems)
                {
                    if (ReferenceEquals(item.REASON, this))
                    {
                        item.REASON = null;
                    }
                }
            }
        }
    
        private void FixupREPORT_DETAIL(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (REPORT_DETAIL item in e.NewItems)
                {
                    item.REASON = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (REPORT_DETAIL item in e.OldItems)
                {
                    if (ReferenceEquals(item.REASON, this))
                    {
                        item.REASON = null;
                    }
                }
            }
        }

        #endregion
    }
}
