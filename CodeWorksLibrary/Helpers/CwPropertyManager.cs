using CADBooster.SolidDna;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace CodeWorksLibrary.Helpers
{
    public class CwPropertyManager
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
        /// Get the content of the custom property
        /// </summary>
        /// <param name="swModel">The model object to read the custom property from</param>
        /// <param name="prpName">The name of the custom property to be read</param>
        /// <returns>The value of the custom property</returns>
        public string GetCustomProperty(ModelDoc2 swModel, string prpName)
        {
            CustomPropertyManager swCustPrpMgr = swModel.Extension.get_CustomPropertyManager("");

            var getResult = swCustPrpMgr.Get6(prpName, false, out string prpValue, out string prpResValue, out bool wasRes, out bool linkPrp);

            return prpValue;
        }

        /// <summary>
        /// Set the name of user that print a drawing
        /// </summary>
        /// <param name="swModel">The model object that needs the property to be changed</param>
        /// <returns>True if the property is set</returns>
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
        /// <returns>True if the property is set</returns>
        public bool SetPrintedOnProperty(ModelDoc2 swModel)
        {
            var currentDate = DateTime.Now.ToString(@"MM\/dd\/yyyy HH\:mm\:ss");

            SetCustomProperty(swModel, GlobalConfig.PrintedOn, currentDate);
            
            return true;
        }

        /// <summary>
        /// Get the quantity custom property in a double format
        /// </summary>
        /// <param name="swModel">The pointer to the SolidWorks ModelDoc2 object</param>
        /// <returns>The double corresponding to quantity custom property</returns>
        public double GetModelQuantity(ModelDoc2 swModel)
        {
            Model model = new Model(swModel);
            
            string quantityValue = model.GetCustomProperty(GlobalConfig.QuantityProperty);

            if (quantityValue != null && quantityValue != string.Empty)
            {
                if (double.TryParse(quantityValue, out double qtyDouble))
                {
                    return qtyDouble;
                }
                else
                {
                    CwMessage.QuantityParseError();
                    return -1;
                }
            }

            return -1;
        }
    }
}
