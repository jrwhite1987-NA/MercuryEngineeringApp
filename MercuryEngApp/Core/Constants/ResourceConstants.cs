// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="ResourceConstants.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Core.Constants
{
    /// <summary>
    /// Class ResourceConstants.
    /// </summary>
    public class ResourceConstants
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="ResourceConstants"/> class from being created.
        /// </summary>
        private ResourceConstants()
        {
        }

        //Syntax for naming variables - PageNameMessageText

        #region "Login Page"

        /// <summary>
        /// The login username password not match
        /// </summary>
        public static string LoginUsernamePasswordNotMatch = "LoginUsernamePasswordNotMatch";

        /// <summary>
        /// The login user not exist
        /// </summary>
        public static string LoginUserNotExist = "LoginUserNotExist";

        /// <summary>
        /// The login user name cannot be blank
        /// </summary>
        public static string LoginUserNameCannotBeBlank = "LoginUserNameCannotBeBlank";

        /// <summary>
        /// The login password cannot be blank
        /// </summary>
        public static string LoginPasswordCannotBeBlank = "LoginPasswordCannotBeBlank";

        #endregion "Login Page"

        #region "Change Password Page"

        /// <summary>
        /// The change password success
        /// </summary>
        public static string ChangePasswordSuccess = "ChangePasswordSuccess";

        /// <summary>
        /// The change password new old password not same
        /// </summary>
        public static string ChangePasswordNewOldPasswordNotSame = "ChangePasswordNewOldPasswordNotSame";

        /// <summary>
        /// The change password error update try again
        /// </summary>
        public static string ChangePasswordErrorUpdateTryAgain = "ChangePasswordErrorUpdateTryAgain";

        /// <summary>
        /// The change password confirm password not match
        /// </summary>
        public static string ChangePasswordConfirmPasswordNotMatch = "ChangePasswordConfirmPasswordNotMatch";

        /// <summary>
        /// The change password all mandatory fields
        /// </summary>
        public static string ChangePasswordAllMandatoryFields = "ChangePasswordAllMandatoryFields";

        /// <summary>
        /// The change password incorrect
        /// </summary>
        public static string ChangePasswordIncorrect = "ChangePasswordIncorrect";

        /// <summary>
        /// The change password validator characters long
        /// </summary>
        public static string ChangePasswordValidatorCharactersLong = "ChangePasswordValidatorCharactersLong";

        /// <summary>
        /// The change password validator no white space
        /// </summary>
        public static string ChangePasswordValidatorNoWhiteSpace = "ChangePasswordValidatorNoWhiteSpace";

        #endregion "Change Password Page"

        #region Manage Users

        /// <summary>
        /// The manage users user not found
        /// </summary>
        public static string ManageUsersUserNotFound = "ManageUsersUserNotFound";

        /// <summary>
        /// The manage users select user from list
        /// </summary>
        public static string ManageUsersSelectUserFromList = "ManageUsersSelectUserFromList";

        /// <summary>
        /// The manage users cannot delete own account
        /// </summary>
        public static string ManageUsersCannotDeleteOwnAccount = "ManageUsersCannotDeleteOwnAccount";

        /// <summary>
        /// The manage users delete confirmation
        /// </summary>
        public static string ManageUsersDeleteConfirmation = "ManageUsersDeleteConfirmation";

        /// <summary>
        /// The manage users delete success message
        /// </summary>
        public static string ManageUsersDeleteSuccessMessage = "ManageUsersDeleteSuccessMessage";

        /// <summary>
        /// The manage users cannot add more than20 users
        /// </summary>
        public static string ManageUsersCannotAddMoreThan20Users = "ManageUsersCannotAddMoreThan20Users";

        /// <summary>
        /// The manage users delete user unsuccessful
        /// </summary>
        public static string ManageUsersDeleteUserUnsuccessful = "ManageUsersDeleteUserUnsuccessful";

        #endregion Manage Users

        #region Update Users

        /// <summary>
        /// The update user error binding roles
        /// </summary>
        public static string UpdateUserErrorBindingRoles = "UpdateUserErrorBindingRoles";

        /// <summary>
        /// The update user data updated
        /// </summary>
        public static string UpdateUserDataUpdated = "UpdateUserDataUpdated";

        /// <summary>
        /// The update user error updating user
        /// </summary>
        public static string UpdateUserErrorUpdatingUser = "UpdateUserErrorUpdatingUser";

        /// <summary>
        /// The update user all mandatory fields
        /// </summary>
        public static string UpdateUserAllMandatoryFields = "UpdateUserAllMandatoryFields";

        /// <summary>
        /// The update user button text
        /// </summary>
        public static string UpdateUserButtonText = "UpdateUserButtonText";

        #endregion Update Users

        #region User Details

        /// <summary>
        /// The user validator f name not less than a character
        /// </summary>
        public static string UserValidatorFNameNotLessThanACharacter = "UserValidatorFNameNotLessThanACharacter";

        /// <summary>
        /// The user validator l name not less than a character
        /// </summary>
        public static string UserValidatorLNameNotLessThanACharacter = "UserValidatorLNameNotLessThanACharacter";

        /// <summary>
        /// The user validator login name not less than five characters
        /// </summary>
        public static string UserValidatorLoginNameNotLessThanFiveCharacters = "UserValidatorLoginNameNotLessThanFiveCharacters";

        /// <summary>
        /// The user validator password not less than six characters
        /// </summary>
        public static string UserValidatorPasswordNotLessThanSixCharacters = "UserValidatorPasswordNotLessThanSixCharacters";

        /// <summary>
        /// The user details login name already exist
        /// </summary>
        public static string UserDetailsLoginNameAlreadyExist = "UserDetailsLoginNameAlreadyExist";

        /// <summary>
        /// The user details new user added
        /// </summary>
        public static string UserDetailsNewUserAdded = "UserDetailsNewUserAdded";

        /// <summary>
        /// The user details error adding new user
        /// </summary>
        public static string UserDetailsErrorAddingNewUser = "UserDetailsErrorAddingNewUser";

        /// <summary>
        /// The user details all mandatory fields
        /// </summary>
        public static string UserDetailsAllMandatoryFields = "UserDetailsAllMandatoryFields";

        /// <summary>
        /// The user validator login name cannot contain whitespaces
        /// </summary>
        public static string UserValidatorLoginNameCannotContainWhitespaces = "UserValidatorLoginNameCannotContainWhitespaces";

        #endregion User Details

        #region Test Review Session

        /// <summary>
        /// The test review session select one snap shot
        /// </summary>
        public static string TestReviewSessionSelectOneSnapShot = "TestReviewSessionSelectOneSnapShot";

        /// <summary>
        /// The test review session spectrogram
        /// </summary>
        public static string TestReviewSessionSpectrogram = "TestReviewSessionSpectrogram";

        /// <summary>
        /// The test review session m mode
        /// </summary>
        public static string TestReviewSessionMMode = "TestReviewSessionMMode";

        /// <summary>
        /// The test review session select CVR
        /// </summary>
        public static string TestReviewSessionSelectCVR = "TestReviewSessionSelectCVR";

        /// <summary>
        /// The test review vessel name not updated
        /// </summary>
        public static string TestReviewVesselNameNotUpdated = "TestReviewVesselNameNotUpdated";

        #endregion Test Review Session

        #region Patient Setup

        /// <summary>
        /// The patient setup required day month year for dob
        /// </summary>
        public static string PatientSetupRequiredDayMonthYearForDOB = "PatientSetupRequiredDayMonthYearForDOB";

        /// <summary>
        /// The patient setup exist with patient identifier MRN
        /// </summary>
        public static string PatientSetupExistWithPatientIdMRN = "PatientSetupExistWithPatientIdMRN";

        /// <summary>
        /// The patient setup dob not greater than current date
        /// </summary>
        public static string PatientSetupDOBNotGreaterThanCurrentDate = "PatientSetupDOBNotGreaterThanCurrentDate";

        /// <summary>
        /// The patient setup new patient added
        /// </summary>
        public static string PatientSetupNewPatientAdded = "PatientSetupNewPatientAdded";

        /// <summary>
        /// The patient setup enter required details
        /// </summary>
        public static string PatientSetupEnterRequiredDetails = "PatientSetupEnterRequiredDetails";

        #endregion Patient Setup

        #region Patient Edit

        /// <summary>
        /// The patient edit required day month year for dob
        /// </summary>
        public static string PatientEditRequiredDayMonthYearForDOB = "PatientEditRequiredDayMonthYearForDOB";

        /// <summary>
        /// The patient edit exist with patient identifier MRN
        /// </summary>
        public static string PatientEditExistWithPatientIdMRN = "PatientEditExistWithPatientIdMRN";

        /// <summary>
        /// The patient edit dob not greater than current date
        /// </summary>
        public static string PatientEditDOBNotGreaterThanCurrentDate = "PatientEditDOBNotGreaterThanCurrentDate";

        /// <summary>
        /// Date of birth Invalid date
        /// </summary>
        public static string PatientDOBInvalidDate = "PatientDOBInvalidDate";

        /// <summary>
        /// The patient edit patient updated success
        /// </summary>
        public static string PatientEditPatientUpdatedSuccess = "PatientEditPatientUpdatedSuccess";

        /// <summary>
        /// The patient edit enter required details
        /// </summary>
        public static string PatientEditEnterRequiredDetails = "PatientEditEnterRequiredDetails";

        /// <summary>
        /// The patient edit patient updated failed
        /// </summary>
        public static string PatientEditPatientUpdatedFailed = "PatientEditPatientUpdatedFailed";

        /// <summary>
        /// The patient edit/Review
        /// </summary>
        public static string PatientRecordsTxtBlockReviewEditButton = "PatientRecords_TxtBlock_ReviewEditButton";

        #endregion Patient Edit

        #region Patient Search

        /// <summary>
        /// The patient search patient not found
        /// </summary>
        public static string PatientSearchPatientNotFound = "PatientSearchPatientNotFound";

        /// <summary>
        /// The patient search select patient from list
        /// </summary>
        public static string PatientSearchSelectPatientFromList = "PatientSearchSelectPatientFromList";

        /// <summary>
        /// The patient search display message
        /// </summary>
        public static string PatientSearchDisplayMessage = "PatientSearchDisplayMessage";

        /// <summary>
        /// The patient search select patient to delete
        /// </summary>
        public static string PatientSearchSelectPatientToDelete = "PatientSearchSelectPatientToDelete";

        /// <summary>
        /// The patient search patient delete successful
        /// </summary>
        public static string PatientSearchPatientDeleteSuccessful = "PatientSearchPatientDeleteSuccessful";

        /// <summary>
        /// The patient search delete patient partial
        /// </summary>
        public static string PatientSearchDeletePatientPartial = "PatientSearchDeletePatientPartial";

        /// <summary>
        /// The patient search delete patient unsuccessful
        /// </summary>
        public static string PatientSearchDeletePatientUnsuccessful = "PatientSearchDeletePatientUnsuccessful";

        #endregion Patient Search

        #region "Exam Notes Page"

        /// <summary>
        /// The exam notes save notes and assessment success
        /// </summary>
        public static string ExamNotesSaveNotesAndAssessmentSuccess = "ExamNotesSaveNotesAndAssessmentSuccess";

        /// <summary>
        /// The exam notes save notes and assessemnt failed
        /// </summary>
        public static string ExamNotesSaveNotesAndAssessemntFailed = "ExamNotesSaveNotesAndAssessemntFailed";

        /// <summary>
        /// The exam notes generate PDF success
        /// </summary>
        public static string ExamNotesGeneratePdfSuccess = "ExamNotesGeneratePdfSuccess";

        /// <summary>
        /// The exam notes generate PDF failed
        /// </summary>
        public static string ExamNotesGeneratePdfFailed = "ExamNotesGeneratePdfFailed";

        #endregion "Exam Notes Page"

        #region "Exam Procedures Page"

        /// <summary>
        /// The save exam not enough disk space
        /// </summary>
        public static string SaveExamNotEnoughDiskSpace = "SaveExamNotEnoughDiskSpace";

        #endregion "Exam Procedures Page"

        #region "Patient Records"

        /// <summary>
        /// The patient record delete exam display
        /// </summary>
        public static string PatientRecordDeleteExamDisplay = "PatientRecordDeleteExamDisplay";

        /// <summary>
        /// The select an exam to delete
        /// </summary>
        public static string SelectAnExamToDelete = "SelectAnExamToDelete";

        /// <summary>
        /// The patient record delete exam successful
        /// </summary>
        public static string PatientRecordDeleteExamSuccessful = "PatientRecordDeleteExamSuccessful";

        /// <summary>
        /// The patient record delete exam unsuccessful
        /// </summary>
        public static string PatientRecordDeleteExamUnsuccessful = "PatientRecordDeleteExamUnsuccessful";

        /// <summary>
        /// The patient record delete partial success
        /// </summary>
        public static string PatientRecordDeletePartialSuccess = "PatientRecordDeletePartialSuccess";

        #endregion "Patient Records"

        #region "Exam"

        /// <summary>
        /// The exam create snapshot success
        /// </summary>
        public static string ExamCreateSnapshotSuccess = "ExamCreateSnapshotSuccess";

        /// <summary>
        /// The exam create snapshot failed
        /// </summary>
        public static string ExamCreateSnapshotFailed = "ExamCreateSnapshotFailed";

        /// <summary>
        /// The exam TCD connection failed
        /// </summary>
        public static string ExamTCDConnectionFailed = "ExamTCDConnectionFailed";

        /// <summary>
        /// The exam select vessel
        /// </summary>
        public static string ExamSelectVessel = "ExamSelectVessel";

        /// <summary>
        /// The exam streaming failed
        /// </summary>
        public static string ExamStreamingFailed = "ExamStreamingFailed";

        /// <summary>
        /// The exam recording CVR information
        /// </summary>
        public static string ExamRecordingCVRInfo = "ExamRecordingCVRInfo";

        public static string HighAcousticPower = "HighAcousticPower";

        /// <summary>
        /// The no snapshots exit message
        /// </summary>
        public static string NoSnapshotsExitMessage = "NoSnapshotsExitMessage";

        /// <summary>
        /// The no snapshots exit button text
        /// </summary>
        public static string NoSnapshotsExitBtnText = "NoSnapshotsExitBtnText";

        #endregion "Exam"

        #region "Lable Constants"

        /// <summary>
        /// The time lable
        /// </summary>
        public static string TimeLable = "TimeLable";

        /// <summary>
        /// The maximum lable
        /// </summary>
        public static string MaxLable = "MaxLable";

        /// <summary>
        /// The minimum lable
        /// </summary>
        public static string MinLable = "MinLable";

        /// <summary>
        /// The mean lable
        /// </summary>
        public static string MeanLable = "MeanLable";

        /// <summary>
        /// The maximum lable
        /// </summary>
        public static string PILable = "PILable";

        /// <summary>
        /// The minimum lable
        /// </summary>
        public static string PowerLable = "PowerLable";

        /// <summary>
        /// The mean lable
        /// </summary>
        public static string SVolLable = "SVolLable";

        /// <summary>
        /// The CVR lable
        /// </summary>
        public static string CVRLable = "CVRLable";

        /// <summary>
        /// The tic lable
        /// </summary>
        public static string TICLable = "TICLable";

        #endregion "Lable Constants"

        #region "Settings"

        /// <summary>
        /// The settings saved success
        /// </summary>
        public static string SettingsSavedSuccess = "SettingsSavedSuccess";

        /// <summary>
        /// The settings saved un success
        /// </summary>
        public static string SettingsSavedUnSuccess = "SettingsSavedUnSuccess";

        /// <summary>
        /// The settings Logs cleared success
        /// </summary>
        public static string SettingsLogsClearedSuccess = "SettingsLogsClearedSuccess";

        /// <summary>
        /// The settings Logs cleared un success
        /// </summary>
        public static string SettingsLogsClearedUnSuccess = "SettingsLogsClearedUnSuccess";

        /// <summary>
        /// The settings invalid ip address
        /// </summary>
        public static string SettingsInvalidIpAddress = "SettingsInvalidIpAddress";

        /// <summary>
        /// The settings server details null
        /// </summary>
        public static string SettingsServerDetailsNull = "SettingsServerDetailsNull";

        /// <summary>
        /// The traceability export success
        /// </summary>
        public static string TraceabilityExportSuccess = "TraceabilityExportSuccess";

        public static string DateTimeUpdateSuccess = "DateTimeUpdateSuccess";
        public static string DateTimeUpdateFailed = "DateTimeUpdateFailed";

        #endregion "Settings"

        #region Usb

        /// <summary>
        /// The usb log success MSG
        /// </summary>
        public static string UsbLogsuccessMsg = "UsbLogsuccessMsg";

        /// <summary>
        /// The usb failure MSG
        /// </summary>
        public static string UsbFailureMsg = "UsbFailureMsg";

        /// <summary>
        /// The usb traceability success MSG
        /// </summary>
        public static string UsbTraceabilitySuccessMsg = "UsbTraceabilitySuccessMsg";

        /// <summary>
        /// The usb report success MSG
        /// </summary>
        public static string UsbReportSuccessMsg = "UsbReportSuccessMsg";

        /// <summary>
        /// The dicom usb report success MSG
        /// </summary>
        public static string DICOMUsbReportSuccessMsg = "DICOMUsbReportSuccessMsg";

        /// <summary>
        /// The usb no log files to transfer
        /// </summary>
        public static string UsbNoLogFilesToTransfer = "UsbNoLogFilesToTransfer";

        /// <summary>
        /// The usb no exam file to transfer
        /// </summary>
        public static string UsbNoExamFileToTransfer = "UsbNoExamFileToTransfer";

        #endregion Usb

        #region Setting Validator

        /// <summary>
        /// The setting validator exam numeric valuefor no of exams
        /// </summary>
        public static string SettingValidatorExamNumericValueforNoOfExams = "SettingValidatorExamNumericValueforNoOfExams";

        /// <summary>
        /// The setting validator log numeric value for maximum log storage
        /// </summary>
        public static string SettingValidatorLogNumericValueForMaxLogstorage = "SettingValidatorLogNumericValueForMaxLogstorage";

        public static string SettingValidatorGreaterThan0NoOfExamHours = "SettingValidatorGreaterThan0NoOfExamHours";
        public static string SettingValidatorGreaterThan0MaxLogstorage = "SettingValidatorGreaterThan0MaxLogstorage";

        /// <summary>
        /// The setting validator digital streaming enter valid ip address
        /// </summary>
        public static string SettingValidatorDigitalStreamingEnterValidIPAddress = "SettingValidatorDigitalStreamingEnterValidIPAddress";

        /// <summary>
        /// The setting test connection successful
        /// </summary>
        public static string SettingTestConnectionSuccessful = "SettingTestConnectionSuccessful";

        /// <summary>
        /// The settings connection to queues failed
        /// </summary>
        public static string SettingsConnectionToQueuesFailed = "SettingsConnectionToQueuesFailed";

        /// <summary>
        /// The settings connection to server successful
        /// </summary>
        public static string SettingsConnectionToServerSuccessful = "SettingsConnectionToServerSuccessful";

        /// <summary>
        /// The settings connection failed
        /// </summary>
        public static string SettingsConnectionFailed = "SettingsConnectionFailed";

        /// <summary>
        /// The settings log file export success
        /// </summary>
        public static string SettingsLogFileExportSuccess = "SettingsLogFileExportSuccess";

        #endregion Setting Validator

        #region Patient Validators

        /// <summary>
        /// The patient validators no blanck space
        /// </summary>
        public static string PatientValidatorsNoBlanckSpace = "PatientValidatorsNoBlanckSpace";

        /// <summary>
        /// The patient validators first name length
        /// </summary>
        public static string PatientValidatorsFirstNameLength = "PatientValidatorsFirstNameLength";

        /// <summary>
        /// The patient validators last name length
        /// </summary>
        public static string PatientValidatorsLastNameLength = "PatientValidatorsLastNameLength";

        /// <summary>
        /// The patient validators MRN length
        /// </summary>
        public static string PatientValidatorsMRNLength = "PatientValidatorsMRNLength";

        /// <summary>
        /// The patient validators vin should be no
        /// </summary>
        public static string PatientValidatorsVINShouldBeNo = "PatientValidatorsVINShouldBeNo";

        /// <summary>
        /// The patient validators vin less than0
        /// </summary>
        public static string PatientValidatorsVINLessThan0 = "PatientValidatorsVINLessThan0";

        /// <summary>
        /// The patient report is not available
        /// </summary>
        public static string PatientReportIsNotAvailable = "PatientReportIsNotAvailable";

        /// <summary>
        /// The patient validators middle name length
        /// </summary>
        public static string PatientValidatorsMiddleNameLength = "PatientValidatorsMiddleNameLength";

        public static string PatientValidatorsMRNNoBlankspace = "PatientValidatorsMRNNoBlankspace";
        #endregion Patient Validators

        #region NA Tech Home Page

        /// <summary>
        /// The na tech home page display reset password
        /// </summary>
        public static string NATechHomePageDisplayResetPassword = "NATechHomePageDisplayResetPassword";

        /// <summary>
        /// The na tech home page password reset failed
        /// </summary>
        public static string NATechHomePagePasswordResetFailed = "NATechHomePagePasswordResetFailed";

        /// <summary>
        /// The na tech home page select admin
        /// </summary>
        public static string NATechHomePageSelectAdmin = "NATechHomePageSelectAdmin";

        #endregion NA Tech Home Page

        #region NA Tech Password

        /// <summary>
        /// The na tech serial key empty
        /// </summary>
        public static string NATechSerialKeyEmpty = "NATechSerialKeyEmpty";

        /// <summary>
        /// The na tech password display
        /// </summary>
        public static string NATechPasswordDisplay = "NATechPasswordDisplay";

        /// <summary>
        /// The na tech password gen unsuccessful
        /// </summary>
        public static string NATechPasswordGenUnsuccessful = "NATechPasswordGenUnsuccessful";

        #endregion NA Tech Password

        #region Audit Trails

        /// <summary>
        /// The audit no trails found
        /// </summary>
        public static string AuditNoTrailsFound = "AuditNoTrailsFound";

        /// <summary>
        /// The error while audit file creation
        /// </summary>
        public static string ErrorWhileAuditFileCreation = "ErrorWhileAuditFileCreation";

        /// <summary>
        /// The audit select duration
        /// </summary>
        public static string AuditSelectDuration = "AuditSelectDuration";

        #endregion Audit Trails

        #region Disk Space

        /// <summary>
        /// The disk space warning MSG
        /// </summary>
        public static string DiskSpaceWarningMsg = "DiskSpaceWarningMsg";

        /// <summary>
        /// The disk space not checked
        /// </summary>
        public static string DiskSpaceNotChecked = "DiskSpaceNotChecked";

        /// <summary>
        /// The disk space full error
        /// </summary>
        public static string DiskSpaceFullError = "DiskSpaceFullError";

        /// <summary>
        /// The disk capacity reached exam stopped
        /// </summary>
        public static string DiskCapacityReachedExamStopped = "DiskCapacityReachedExamStopped";

        /// <summary>
        /// The disk space full error cant generate report
        /// </summary>
        public static string DiskSpaceFullErrorCantCreateReport = "DiskSpaceFullErrorCantCreateReport";

        #endregion Disk Space

        #region Log Export

        /// <summary>
        /// The log select duration
        /// </summary>
        public static string LogselectDuration = "LogselectDuration";

        #endregion Log Export

        #region Microcontroller
        static string LowBattery = "LowBattery";
        public static string ExamStopLowBattery = "ExamStopLowBattery";
        public static string ExamStopTCDDisconnect = "ExamStopTCDDisconnect";
        #endregion Microcontroller

        #region Probe connections
        public static string Probe1Disconnected = "Probe1Disconnected";
        public static string Probe2Disconnected = "Probe2Disconnected";
        public static string ConnectProbe = "ConnectProbe";
        public static string RestartFailed = "RestartFailed";
        #endregion Probe connections

        #region About/Acknowledgment
        public const string CopyrightFileFailureMsg = "CopyrightFileFailureMsg";
        #endregion About/Acknowledgment

        #region PDF
        /// <summary>
        /// The Name on pdf report
        /// </summary>
        public static string PdfReportName = "PdfReportName";

        public static string PdfReportExamType = "PdfReportExamType";
        public static string PdfReportDOB = "PdfReportDOB";
        public static string PdfReportExamDate = "PdfReportExamDate";
        public static string PdfReportGender = "PdfReportGender";
        public static string PdfReportTechnician = "PdfReportTechnician";
        public static string PdfReportMRN = "PdfReportMRN";
        public static string PdfReportPhysician = "PdfReportPhysician";        
        #endregion 

        public const string NotAvailable = "NotAvailable";

    }
}