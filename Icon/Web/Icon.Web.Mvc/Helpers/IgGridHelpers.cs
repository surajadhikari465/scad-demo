using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Helpers
{
    public static class IgGridHelpers
    {
        public static ColumnUpdatingSettingWrapper CreateComboEditor(this ColumnUpdatingSettingBuilder<ColumnUpdatingSetting> colSettings, string columnKey, IQueryable dataSource, string valueKey, string textKey)
        {
            return colSettings.ColumnSetting()
                .ColumnKey(columnKey)
                .EditorType(ColumnEditorType.Combo)
                .ComboEditorOptions(options =>
                    options.DataSource(dataSource)
                        .ValueKey(valueKey)
                        .TextKey(textKey)
                        .ShowDropDownButton(true)
                        .AllowCustomValue(false)
                        .Virtualization(true)
                        .DropDownWidth(300)
                        .FilteringType(ComboFilteringType.Local)
                );
        }

        public static ColumnUpdatingSettingWrapper CreateComboEditor(this ColumnUpdatingSettingBuilder<ColumnUpdatingSetting> colSettings, string columnKey, IQueryable dataSource)
        {
            return colSettings.ColumnSetting()
                .ColumnKey(columnKey)
                .EditorType(ColumnEditorType.Combo)
                .ComboEditorOptions(options =>
                    options.DataSource(dataSource)
                        .ShowDropDownButton(true)
                        .AllowCustomValue(false)
                        .Virtualization(true)
                        .DropDownWidth(300)
                        .FilteringType(ComboFilteringType.Local)
                );
        }
    }
}