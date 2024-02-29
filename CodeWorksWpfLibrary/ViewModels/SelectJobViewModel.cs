using CodeWorksWpfLibrary.Interfaces;
using CodeWorksWpfLibrary.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeWorksWpfLibrary.ViewModels
{
    /// <summary>
    /// The view model for the job selection window
    /// </summary>
    public class SelectJobViewModel : BaseViewModel, ICloseWindow
    {
        #region Private fields

        /// <summary>
        /// The error message displayed to the user
        /// </summary>
        private string _validationErrorMessage;

        /// <summary>
        /// The message for an invalid job number
        /// </summary>
        private string _jobNumberValidNumber = "Job Number must contain a valid number";

        /// <summary>
        /// The message for a positive job number
        /// </summary>
        private string _jobNumberPositiveNumber = "Job Number must contain a positive number";

        /// <summary>
        /// The message for an invalid job year
        /// </summary>
        private string _jobYearValidNumber = "Job Year must contain a valid number";

        /// <summary>
        /// The message for a positive job year
        /// </summary>
        private string _jobYearPositiveNumber = "Job Year must contain a positive number";

        /// <summary>
        /// The message for an invalid quantity
        /// </summary>
        private string _quantityValidNumber = "Quantity must contain a valid number";

        /// <summary>
        /// The message for a positive job quantity
        /// </summary>
        private string _quantityPositiveNumber = "Quantity must contain a positive number";

        #endregion

        #region Public properties

        /// <summary>
        /// The job number specified by the user
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// The job year specified by the user
        /// </summary>
        public string JobYear { get; set; }

        /// <summary>
        /// The ticket number specified by the user
        /// </summary>
        public string TicketNumber { get; set; }

        /// <summary>
        /// A string with the quantity to be manufactured
        /// </summary>
        public string JobQuantity { get; set; }

        /// <summary>
        /// Define whether all the components must be exported or just the remaining
        /// </summary>
        public bool ExportAgain { get; set; }

        /// <summary>
        /// True if the OK button can be pressed
        /// </summary>
        public bool IsOkButtonEnabled
        {
            get
            {
                return ValidateFields();
            }
            set
            {

            }
        }

        /// <summary>
        /// True if the ok button have been pressed
        /// </summary>
        public bool IsOkButtonPressed { get; set; } = false;

        /// <summary>
        /// True if the cancel button have been pressed
        /// </summary>
        public bool IsCancelButtonPressed { get; set; } = false;

        #endregion

        #region Public commands

        /// <summary>
        /// Command to close the windows and proceed with the executions
        /// </summary>
        public ICommand OkCommand { get; set; }

        /// <summary>
        /// Command to cancel the operation
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Action that close the window
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Action that cancel the window
        /// </summary>
        public Action Cancel { get; set; }

        #endregion

        #region Constructor

        public SelectJobViewModel()
        {
            this.OkCommand = new RelayCommand(OkPress);

            this.CancelCommand = new RelayCommand(CancelPress);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Close the window
        /// </summary>
        private void OkPress()
        {
            IsOkButtonPressed = true;
            Close?.Invoke();
        }

        /// <summary>
        /// Cancel the operation
        /// </summary>
        private void CancelPress()
        {
            IsCancelButtonPressed = true;
            Cancel?.Invoke();
        }

        /// <summary>
        /// Check whether or not the input are valid
        /// </summary>
        /// <returns>True if the input are valid</returns>
        private bool ValidateFields()
        {
            bool output = true;

            _validationErrorMessage = string.Empty;

            // Check job number
            if (string.IsNullOrEmpty(JobNumber))
            {
                output = false;
                _validationErrorMessage = _jobNumberValidNumber;
            }

            bool jobNumberValidNumber = int.TryParse(JobNumber, out int jobNumberInt);

            if (jobNumberValidNumber == false)
            {
                output = false;
                _validationErrorMessage = _jobNumberValidNumber;
            }

            if (jobNumberInt < 1)
            {
                output = false;
                _validationErrorMessage = _jobNumberPositiveNumber;
            }

            // Check job year
            if (string.IsNullOrEmpty(JobYear))
            {
                output = false;
                _validationErrorMessage = _jobYearValidNumber;
            }

            bool jobYearValidNumber = int.TryParse(JobYear, out int jobYearInt);

            if (jobYearValidNumber == false)
            {
                output = false;
                _validationErrorMessage = _jobYearValidNumber;
            }

            if (jobYearInt < 1)
            {
                output = false;
                _validationErrorMessage = _jobYearPositiveNumber;
            }

            // Validate export quantity
            if (string.IsNullOrEmpty(JobQuantity))
            {
                output = false;
                _validationErrorMessage = _quantityValidNumber;
            }

            bool jobQuantityValidNumber = int.TryParse(JobQuantity, out int jobQuantityInt);

            if (jobQuantityValidNumber == false)
            {
                output = false;
                _validationErrorMessage = _quantityValidNumber;
            }

            if (jobQuantityInt < 1)
            {
                output = false;
                _validationErrorMessage = _quantityPositiveNumber;
            }

            return output;
        }

        #endregion
    }
}
