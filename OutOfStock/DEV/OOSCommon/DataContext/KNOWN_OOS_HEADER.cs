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
    public partial class KNOWN_OOS_HEADER
    {
        #region Primitive Properties
    
        public virtual int ID
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
    
        public virtual ICollection<KNOWN_OOS_MAP> KNOWN_OOS_MAP
        {
            get
            {
                if (_kNOWN_OOS_MAP == null)
                {
                    var newCollection = new FixupCollection<KNOWN_OOS_MAP>();
                    newCollection.CollectionChanged += FixupKNOWN_OOS_MAP;
                    _kNOWN_OOS_MAP = newCollection;
                }
                return _kNOWN_OOS_MAP;
            }
            set
            {
                if (!ReferenceEquals(_kNOWN_OOS_MAP, value))
                {
                    var previousValue = _kNOWN_OOS_MAP as FixupCollection<KNOWN_OOS_MAP>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupKNOWN_OOS_MAP;
                    }
                    _kNOWN_OOS_MAP = value;
                    var newValue = value as FixupCollection<KNOWN_OOS_MAP>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupKNOWN_OOS_MAP;
                    }
                }
            }
        }
        private ICollection<KNOWN_OOS_MAP> _kNOWN_OOS_MAP;

        #endregion

        #region Association Fixup
    
        private void FixupKNOWN_OOS_DETAIL(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (KNOWN_OOS_DETAIL item in e.NewItems)
                {
                    item.KNOWN_OOS_HEADER = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (KNOWN_OOS_DETAIL item in e.OldItems)
                {
                    if (ReferenceEquals(item.KNOWN_OOS_HEADER, this))
                    {
                        item.KNOWN_OOS_HEADER = null;
                    }
                }
            }
        }
    
        private void FixupKNOWN_OOS_MAP(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (KNOWN_OOS_MAP item in e.NewItems)
                {
                    item.KNOWN_OOS_HEADER = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (KNOWN_OOS_MAP item in e.OldItems)
                {
                    if (ReferenceEquals(item.KNOWN_OOS_HEADER, this))
                    {
                        item.KNOWN_OOS_HEADER = null;
                    }
                }
            }
        }

        #endregion

    }
}
