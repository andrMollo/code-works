using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

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

        /// <summary>
        /// Set the name of user that print a drawing
        /// </summary>
        /// <param name="swModel">The model object that needs the property to be changed</param>
        /// <returns></returns>
        public bool SetPrintedByProperty(ModelDoc2 swModel)
        {
            var user = CwPdmManager.GetPdmUserName();

            if (user != null)
            {
                SetCustomProperty(swModel, GlobalConfig.PrintedBy, user);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the date when the document is printed
        /// </summary>
        /// <param name="swModel">The model object that needs the property to be changed</param>
        /// <returns></returns>
        public bool SetPrintedOnProperty(ModelDoc2 swModel)
        {
            var currentDate = DateTime.Now.ToString(@"MM\/dd\/yyyy HH\:mm\:ss");

            SetCustomProperty(swModel, GlobalConfig.PrintedOn, currentDate);
            
            return true;
        }
    }
}
