//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Icon.Framework
{
    using System;
    using System.Collections.Generic;
    
    public partial class CharacterSets
    {
        public CharacterSets()
        {
            this.AttributeCharacterSets = new HashSet<AttributeCharacterSets>();
        }
    
        public int CharacterSetId { get; set; }
        public string Name { get; set; }
        public string RegEx { get; set; }
    
        public virtual ICollection<AttributeCharacterSets> AttributeCharacterSets { get; set; }
    }
}
