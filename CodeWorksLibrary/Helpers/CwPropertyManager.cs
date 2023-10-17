﻿using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace CodeWorksLibrary
{
    internal class CwPropertyManager
    {
        /// <summary>
        /// Set the value of a custom property
        /// </summary>
        /// <param name="swModel">The model object that needs the property to be changed</param>
        /// <param name="propertyName">The name of the property to be changed</param>
        /// <param name="prpValue">The value of the property to be changed</param>
        public void SetCustomProperty(ModelDoc2 swModel, string propertyName, string prpValue)
        {
            CustomPropertyManager swCustPrpMgr = swModel.Extension.get_CustomPropertyManager("");

            swCustPrpMgr.Add3(propertyName, (int)swCustomInfoType_e.swCustomInfoText, prpValue, (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
        }
    }
}
