using AxisInstallerPackage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AxisInstallerPackage.Helpers
{
    public static class ControlBinding
    {
        public static void BindComboBox(ComboBox cmb, List<LookUpModel> dataList, int setIndex = 0, bool displayIsValueMember = false)
        {
            //cmb.Items.Clear();
            cmb.ItemsSource = dataList;
            if (displayIsValueMember)
            {
                cmb.DisplayMemberPath = "displayMember";
                cmb.SelectedValuePath = "displayMember";
            }
            else
            {
                cmb.DisplayMemberPath = "displayMember";
                cmb.SelectedValuePath = "valueMember";
            }

            cmb.SelectedIndex = setIndex;
        }
    }
}
