// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="MessageConstants.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Core.Constants
{
    /// <summary>
    /// Class MessageConstants.
    /// </summary>
    public class MessageConstants
    {
        #region Common Messages

        /// <summary>
        /// The message duration for snapshot
        /// </summary>
        public static string MessageDurationForSnapshot = "2";

        /// <summary>
        /// The message duration
        /// </summary>
        public static string MessageDuration = "3";

        /// <summary>
        /// The message duration
        /// </summary>
        public static string PrecautionMessageDuration = "5";

        /// <summary>
        /// The new user add success MSG
        /// </summary>
        public static string NewUserAddSuccessMsg = "New user added.";

        /// <summary>
        /// The new user add error MSG
        /// </summary>
        public static string NewUserAddErrorMsg = "There was an error while adding new user.";

        /// <summary>
        /// The required fields error MSG
        /// </summary>
        public static string RequiredFieldsErrorMsg = "All the fields are required.";

        /// <summary>
        /// The invalid credential error MSG
        /// </summary>
        public static string InvalidCredentialErrorMsg = "Invalid Credentials! Please Try again";

        /// <summary>
        /// The required user name password error MSG
        /// </summary>
        public static string RequiredUserNamePasswordErrorMsg = "Please enter login and password";

        /// <summary>
        /// The usb failure MSG
        /// </summary>
        public static string UsbFailureMsg = "Export Failed. Please Check if the USB is plugged in properly !";

        #endregion Common Messages

        #region "Login Page"

        /// <summary>
        /// The login username password not match
        /// </summary>
        public static string LoginUsernamePasswordNotMatch = "Username and Password  does not match.";

        /// <summary>
        /// The login user not exist
        /// </summary>
        public static string LoginUserNotExist = "User does not exist.Tried Login with username : ";

        /// <summary>
        /// The login successful
        /// </summary>
        public static string LoginSuccessful = "Login successful.Logged In UserId : ";

        /// <summary>
        /// The login username empty
        /// </summary>
        public static string LoginUsernameEmpty = "Login unsuccessful - Username empty";

        /// <summary>
        /// The login password empty
        /// </summary>
        public static string LoginPasswordEmpty = "Login unsuccessful - Password empty";

        /// <summary>
        /// The login user name password validation failed
        /// </summary>
        public static string LoginUserNamePasswordValidationFailed = "Failed trying to validate username and password.";

        #endregion "Login Page"

        #region "Change Password Page"

        /// <summary>
        /// The change password success
        /// </summary>
        public static string ChangePasswordSuccess = "Password changed successfully! UserId : ";

        /// <summary>
        /// The change password new old password not same
        /// </summary>
        public static string ChangePasswordNewOldPasswordNotSame = "New password and old password should not be same.";

        /// <summary>
        /// The change password error update try again
        /// </summary>
        public static string ChangePasswordErrorUpdateTryAgain = "Some error in update. Please try again. UserId : ";

        /// <summary>
        /// The change password confirm password not match
        /// </summary>
        public static string ChangePasswordConfirmPasswordNotMatch = "Password and confirm password do not match.";

        /// <summary>
        /// The change password all mandatory fields
        /// </summary>
        public static string ChangePasswordAllMandatoryFields = "All fields are mandatory.";

        /// <summary>
        /// The change password incorrect
        /// </summary>
        public static string ChangePasswordIncorrect = "Incorrect old password. UserId : ";

        /// <summary>
        /// The change password validator characters long
        /// </summary>
        public static string ChangePasswordValidatorCharactersLong = "Password must be 6-20 characters long";

        /// <summary>
        /// The change password validator no white space
        /// </summary>
        public static string ChangePasswordValidatorNoWhiteSpace = "Password should not contain white space";

        #endregion "Change Password Page"

        #region Manage Users

        /// <summary>
        /// The manage users user not found
        /// </summary>
        public static string ManageUsersUserNotFound = "User not found";

        /// <summary>
        /// The manage users select user from list
        /// </summary>
        public static string ManageUsersSelectUserFromList = "Please select a user from the user list";

        /// <summary>
        /// The manage users delete user successful
        /// </summary>
        public static string ManageUsersDeleteUserSuccessful = "User Deleted successfully";

        /// <summary>
        /// The manage users delete user un successful
        /// </summary>
        public static string ManageUsersDeleteUserUnSuccessful = "User Deletion Unsuccessful.";

        /// <summary>
        /// The manage users delete user cancelled
        /// </summary>
        public static string ManageUsersDeleteUserCancelled = "User Delete Cancelled";

        /// <summary>
        /// The manage users create user limit reached
        /// </summary>
        public static string ManageUsersCreateUserLimitReached = "The system cannot have more than 20 Users.";

        /// <summary>
        /// The manage users sort by role error
        /// </summary>
        public static string ManageUsersSortByRoleError = "Error while sorting by Role.";

        /// <summary>
        /// The manage users tried to delete own acc
        /// </summary>
        public static string ManageUsersTriedToDeleteOwnAcc = "User tried to delete own account.";

        #endregion Manage Users

        #region Update Users

        /// <summary>
        /// The update user error binding roles
        /// </summary>
        public static string UpdateUserErrorBindingRoles = "Error while binding roles. Please try again.";

        /// <summary>
        /// The update user data updated
        /// </summary>
        public static string UpdateUserDataUpdated = "User updated successfully";

        /// <summary>
        /// The update user error updating user
        /// </summary>
        public static string UpdateUserErrorUpdatingUser = "Error while updating the user";

        /// <summary>
        /// The update user all mandatory fields
        /// </summary>
        public static string UpdateUserAllMandatoryFields = "All fields are mandatory";

        #endregion Update Users

        #region User Details

        /// <summary>
        /// The user details unsuccessful validations
        /// </summary>
        public static string UserDetailsUnsuccessfulValidations = "Validations are unsuccessful";

        /// <summary>
        /// The user details login name already exist
        /// </summary>
        public static string UserDetailsLoginNameAlreadyExist = "Login name is already in use. Login Name : ";

        /// <summary>
        /// The user details new user added
        /// </summary>
        public static string UserDetailsNewUserAdded = "New user is added";

        /// <summary>
        /// The user details error adding new user
        /// </summary>
        public static string UserDetailsErrorAddingNewUser = "Error while adding a new user";

        /// <summary>
        /// The user details all mandatory fields
        /// </summary>
        public static string UserDetailsAllMandatoryFields = "All fields are mandatory";

        #endregion User Details

        #region Test Review Session

        /// <summary>
        /// The test review session select one snap shot
        /// </summary>
        public static string TestReviewSessionSelectOneSnapShot = "Please select at least one Snap Shot";

        /// <summary>
        /// The test review session select CVR
        /// </summary>
        public static string TestReviewSessionSelectCVR = "Please select the CVR";

        /// <summary>
        /// The test review session sort by selected snapshot ex
        /// </summary>
        public static string TestReviewSessionSortBySelectedSnapshotEx = "Exception occured while sorting by snapshot selection.";

        /// <summary>
        /// The test review session sory by vessel ex
        /// </summary>
        public static string TestReviewSessionSoryByVesselEx = "Exception occured while sorting by Vessel.";

        /// <summary>
        /// The test review session sory by sanapshot date
        /// </summary>
        public static string TestReviewSessionSoryBySanapshotDate = "Exception occured while sorting by Snapshot Date.";

        /// <summary>
        /// The test review vessel name not updated
        /// </summary>
        public static string TestReviewVesselNameNotUpdated = "Test Review - Vessel Name not updated.";

        /// <summary>
        /// The test review save success
        /// </summary>
        public static string TestReviewSaveSuccess = "Test Review changes updated.";

        #endregion Test Review Session

        #region Patient Setup

        /// <summary>
        /// The patient setup new patient added
        /// </summary>
        public static string PatientSetupNewPatientAdded = "New patient is added";

        /// <summary>
        /// The patient setup empty fields
        /// </summary>
        public static string PatientSetupEmptyFields = "Patient setup required fields empty.";

        /// <summary>
        /// The patient setup MRN exist
        /// </summary>
        public static string PatientSetupMRNExist = "Patient with same MRN exists. Entered MRN : ";

        /// <summary>
        /// The patient setup unsuccessful
        /// </summary>
        public static string PatientSetupUnsuccessful = "Patient setup unsuccessful";

        /// <summary>
        /// The patient setup error in month selection
        /// </summary>
        public static string PatientSetupErrorInMonthSelection = "Error while selecting Month.";

        /// <summary>
        /// The patient setup error in year selection
        /// </summary>
        public static string PatientSetupErrorInYearSelection = "Error while selecting Year.";

        #endregion Patient Setup

        #region Patient Edit

        /// <summary>
        /// The patient edit patient updated success
        /// </summary>
        public static string PatientEditPatientUpdatedSuccess = "Patient Details are updated";

        /// <summary>
        /// The patient edit patient updated failed
        /// </summary>
        public static string PatientEditPatientUpdatedFailed = "Patient Details are not updated. Please try again.";

        /// <summary>
        /// The patient edit dob required empty
        /// </summary>
        public static string PatientEditDOBRequiredEmpty = "DOB(required field - left empty).";

        /// <summary>
        /// The patient edit MRN exist
        /// </summary>
        public static string PatientEditMRNExist = "Patient Update - Patient with same MRN exist.MRN : ";

        /// <summary>
        /// The patient edit required fields empty
        /// </summary>
        public static string PatientEditRequiredFieldsEmpty = "Patient Edit - required fields empty.";

        #endregion Patient Edit

        #region "Exam Notes page"

        /// <summary>
        /// The exam notes save notes and assessment success
        /// </summary>
        public static string ExamNotesSaveNotesAndAssessmentSuccess = "Notes and assessment saved successfully";

        /// <summary>
        /// The exam notes save notes and assessemnt failed
        /// </summary>
        public static string ExamNotesSaveNotesAndAssessemntFailed = "Could not save Notes and assessment";

        /// <summary>
        /// The exam notes generate PDF success
        /// </summary>
        public static string ExamNotesGeneratePdfSuccess = "PDF created successfully !";

        /// <summary>
        /// The exam notes generate PDF failed
        /// </summary>
        public static string ExamNotesGeneratePdfFailed = "PDF could not be generate";

        /// <summary>
        /// The exam notes exception loading exam notes data
        /// </summary>
        public static string ExamNotesExceptionLoadingExamNotesData = "Exception occured while loading exam notes data.";

        #endregion "Exam Notes page"

        #region "Exam Procedures page"

        /// <summary>
        /// The save exam not enough disk space
        /// </summary>
        public static string SaveExamNotEnoughDiskSpace = "Not enough disk space available. Please cleanup the disk and run the exam";

        #endregion "Exam Procedures page"

        #region "Exam"

        public static string ActiveProbe1Disconnected = "Active probe 1 was disconnected";
        public static string ActiveProbe2Disconnected = "Active probe 2 was disconnected";
        public static string NoProbeOnExamLaunch = "No probes were connected at exam launch";

        /// <summary>
        /// The exam create snapshot success
        /// </summary>
        public static string ExamCreateSnapshotSuccess = "Snapshot created successfully.";

        /// <summary>
        /// The exam create snapshot failed
        /// </summary>
        public static string ExamCreateSnapshotFailed = "Cannot create a snapshot.";

        /// <summary>
        /// The exam TCD connection failed
        /// </summary>
        public static string ExamTCDConnectionFailed = "System Error. Please contact Neural Analytics support.";

        public static string LowBattery = "Exam could not be started due to low battery.";
        public static string PowerNotTurnedOn = "Accoustic Power could not be turned on for the exam.";

        /// <summary>
        /// The exam packet int ex
        /// </summary>
        public static string ExamPacketIntEx = "length must be either 2 or 4";

        /// <summary>
        /// The exam play audio ex
        /// </summary>
        public static string ExamPlayAudioEx = "Play Audio threw exception.";

        /// <summary>
        /// The exam on packet formed ex
        /// </summary>
        public static string ExamOnPacketFormedEx = "Exception while detecting Embolicount and streaming it.";

        /// <summary>
        /// The exam loading ex
        /// </summary>
        public static string ExamLoadingEx = "Exception while loading exam page";

        /// <summary>
        /// The exam streaming failed
        /// </summary>
        public static string ExamStreamingFailed = "Streaming Exam Failed.";

        /// <summary>
        /// The exam TCD off
        /// </summary>
        public static string ExamTCDOff = "Accoustic power to TCD turned off.";

        /// <summary>
        /// The exam TCD on
        /// </summary>
        public static string ExamTCDOn = "Accoustic power to TCD turned on.";

        /// <summary>
        /// The exam started
        /// </summary>
        public static string ExamStarted = "Exam Started. Exam Type : ";

        /// <summary>
        /// The exam ended
        /// </summary>
        public static string ExamEnded = "Exam Ended.";

        /// <summary>
        /// The exam recording on
        /// </summary>
        public static string ExamRecordingOn = "Recording turned ON.";

        /// <summary>
        /// The examrecording off
        /// </summary>
        public static string ExamrecordingOff = "Recording turned OFF.";

        public static string EmbCountError = "Cumulative emobli count not in sync with flag";

        #endregion "Exam"

        #region "Settings"

        /// <summary>
        /// The settings saved success
        /// </summary>
        public static string SettingsSavedSuccess = "Settings saved successfully";

        /// <summary>
        /// The settings saved un success
        /// </summary>
        public static string SettingsSavedUnSuccess = "Saving settings are unsucessful";

        /// <summary>
        /// The settings log files clearing success
        /// </summary>
        public static string SettingsLogFilesClearingSuccess = "Log files cleared successfully.";

        /// <summary>
        /// The settings log files clearing failed
        /// </summary>
        public static string SettingsLogFilesClearingFailed = "Log files clear failed";

        /// <summary>
        /// The settings test connection success
        /// </summary>
        public static string SettingsTestConnectionSuccess = "Connection to streaming server successful.";

        /// <summary>
        /// The settings connection to queues failed
        /// </summary>
        public static string SettingsConnectionToQueuesFailed = "Connection to following queues Failed : ";

        /// <summary>
        /// The settings connection to server successful
        /// </summary>
        public static string SettingsConnectionToServerSuccessful = "Connection to streaming server unsuccessful.";

        /// <summary>
        /// The settings connection failed
        /// </summary>
        public static string SettingsConnectionFailed = "Unable to connect to server!";

        /// <summary>
        /// The settings connection to streaming server failed
        /// </summary>
        public static string SettingsConnectionToStreamingServerFailed = "Connection to streaming server unsuccessful.";

        /// <summary>
        /// The settings log file export success
        /// </summary>
        public static string SettingsLogFileExportSuccess = "Log file exported to USB.";

        /// <summary>
        /// The settings log file export unsuccessful
        /// </summary>
        public static string SettingsLogFileExportUnsuccessful = "Log file export to USB failed.";

        /// <summary>
        /// The settings traceability file export success
        /// </summary>
        public static string SettingsTraceabilityFileExportSuccess = "Traceability file exported to USB.";

        /// <summary>
        /// The settings traceability file export unsuccessful
        /// </summary>
        public static string SettingsTraceabilityFileExportUnsuccessful = "Traceability file export to USB failed.";

        #endregion "Settings"

        #region NA Tech Home page

        /// <summary>
        /// The na tech home page password reset failed
        /// </summary>
        public static string NATechHomePagePasswordResetFailed = "Selected Admin password reset Unsuccessful. UserId : ";

        /// <summary>
        /// The na tech home page password reset successful
        /// </summary>
        public static string NATechHomePagePasswordResetSuccessful = "System Admin password reset by NA_Technician. UserId : ";

        #endregion NA Tech Home page

        #region LockOut

        /// <summary>
        /// The lock out activated
        /// </summary>
        public static string NotAllowUserToLogin = "You are not authorized to login : ";

        /// <summary>
        /// The lock out activated
        /// </summary>
        public static string LockOutActivated = "LockOut Mode Activated. Logged in UserId : ";

        /// <summary>
        /// User Log out
        /// </summary>
        public static string LogoutApplication = "User logged out of application.Was logged in with UserId : ";

        #endregion LockOut

        #region NA Tech Password

        /// <summary>
        /// The na tech serial key empty
        /// </summary>
        public static string NATechSerialKeyEmpty = "Serial Key left empty.";

        /// <summary>
        /// The na tech password gen successful
        /// </summary>
        public static string NATechPasswordGenSuccessful = "NA Technician password generated.";

        /// <summary>
        /// The na tech password gen unsuccessful
        /// </summary>
        public static string NATechPasswordGenUnsuccessful = "Password generation for NATech failed.";

        #endregion NA Tech Password

        #region CVRDualChannel Page

        /// <summary>
        /// The CVR dual channel loading failed
        /// </summary>
        public static string CVRDualChannelLoadingFailed = "CVR Loading Failed.";

        /// <summary>
        /// The CVR dual channel computation error
        /// </summary>
        public static string CVRDualChannelComputationError = "CVR and BHI computation Failed";

        #endregion CVRDualChannel Page

        #region Patient Record

        /// <summary>
        /// The patient record delete exam successful
        /// </summary>
        public static string PatientRecordDeleteExamSuccessful = "Exam delete successful.ExamId : {0}, Exam Type : {1}";

        /// <summary>
        /// The patient record delete exam unsuccessful
        /// </summary>
        public static string PatientRecordDeleteExamUnsuccessful = "Exam Delete - Unsuccessful.ExamId : ";

        /// <summary>
        /// The patient record delete partial success
        /// </summary>
        public static string PatientRecordDeletePartialSuccess = "Exam deleted from Database. Unable to Delete Exam files from Local storage.";

        /// <summary>
        /// The patient record delete exam cancelled
        /// </summary>
        public static string PatientRecordDeleteExamCancelled = "Exam Delete Cancelled.";

        /// <summary>
        /// The patient record exam date sort failed
        /// </summary>
        public static string PatientRecordExamDateSortFailed = "Exception occured while trying to sort by Exam date.";

        /// <summary>
        /// The patient record report exported success
        /// </summary>
        public static string PatientRecordReportExportedSuccess = "Patient Exam report exported successfully.ExamId : ";
        /// <summary>
        /// The patient record report export failed
        /// </summary>
        public static string PatientRecordReportExportFailed = "Patient Exam report export unsuccessful.ExamId : ";
        /// <summary>
        /// The patient dicom record report exported success
        /// </summary>
        public static string PatientDicomReportExportedSuccess = "DICOM report of patient exported.";

        /// <summary>
        /// The patient dicom record report export failed
        /// </summary>
        public static string PatientDicomReportExportFailed = "DICOM report of patient export unsuccessful.";

        #endregion Patient Record

        #region Patient Search

        /// <summary>
        /// The patient search exception
        /// </summary>
        public static string PatientSearchException = "Exception while searching patient.";

        /// <summary>
        /// The patient search sort by dob exception
        /// </summary>
        public static string PatientSearchSortByDOBException = "Exception while sorting by parient DOB.";

        /// <summary>
        /// The patient search select patient to delete
        /// </summary>
        public static string PatientSearchSelectPatientToDelete = "Did not select patient to perform delete action.";

        /// <summary>
        /// The patient search delete patient cancelled
        /// </summary>
        public static string PatientSearchDeletePatientCancelled = "Patient Delete cancelled.";

        /// <summary>
        /// The patient search delete patient successful database
        /// </summary>
        public static string PatientSearchDeletePatientSuccessfulDB = "Patient deleted from Database. PatientId : ";

        /// <summary>
        /// The patient search delete patient successful
        /// </summary>
        public static string PatientSearchDeletePatientSuccessful = "Patient and corresponding patient exams deleted successfully.";

        /// <summary>
        /// The patient search delete patient partial
        /// </summary>
        public static string PatientSearchDeletePatientPartial = "Patient deleted. Few exam data of the patient not deleted from Local Storage. PatientId : ";

        /// <summary>
        /// The patient search delete patient unsuccessful
        /// </summary>
        public static string PatientSearchDeletePatientUnsuccessful = "Patient delete failed. PatientId : ";

        #endregion Patient Search

        #region TCD Library

        /// <summary>
        /// The probe1 connected
        /// </summary>
        public static string Probe1Connected = "Probe 1 is detected and connected to the TCD";

        /// <summary>
        /// The probe2 connected
        /// </summary>
        public static string Probe2Connected = "Probe 2 is detected and connected to the TCD";

        /// <summary>
        /// The probe1 disconnected
        /// </summary>
        public static string Probe1Disconnected = "Probe 1 was disconnected.";

        /// <summary>
        /// The probe2 disconnected
        /// </summary>
        public static string Probe2Disconnected = "Probe 2 was disconnected";

        /// <summary>
        /// The invalid checksum
        /// </summary>
        public static string InvalidChecksum = "Checksum failed for packet number";

        public static string InvalidSequence = "Dropped packet number #";

        /// <summary>
        /// The no response
        /// </summary>
        public static string NoResponse = "NoResponse";

        public static string TCDDisconnected = "TCD was disconnected";
        public static string TCDConnected = "TCD was connected";

        #endregion TCD Library

        #region Exam View Model

        /// <summary>
        /// The delete exam delete report ex
        /// </summary>
        public static string DeleteExamDeleteReportEx = "Delete Pdf report threw exception. FileName - ";

        /// <summary>
        /// The delete exam delete spectrogram snapshot ex
        /// </summary>
        public static string DeleteExamDeleteSpectrogramSnapshotEx = "Delete Spectrogram snapshot image threw exception. FileName : ";

        /// <summary>
        /// The delete exam delete mmode snapshot ex
        /// </summary>
        public static string DeleteExamDeleteMmodeSnapshotEx = "Delete Mmode snapshot image threw exception. FileName : ";

        /// <summary>
        /// The delete exam delete channel1 binary file ex
        /// </summary>
        public static string DeleteExamDeleteChannel1BinaryFileEx = "Delete Channel1 Binary file threw exception. File Name : ";

        /// <summary>
        /// The delete exam delete channel2 binary file ex
        /// </summary>
        public static string DeleteExamDeleteChannel2BinaryFileEx = "Delete Channel2 Binary file threw exception. File Name : ";

        public static string DeleteExamDeleteVideoFilesEx = "Delete video file threw exception. File Name : ";

        #endregion Exam View Model

        #region MicroController

        /// <summary>
        /// The micro controller connection failed
        /// </summary>
        public static string MicroControllerConnectionFailed = "Connection to MicroController could not be established";

        #endregion MicroController

        #region Audit Trail

        /// <summary>
        /// The audit no trails found
        /// </summary>
        public static string AuditNoTrailsFound = "No audit trails found. Please choose a different duration!";

        /// <summary>
        /// The error while audit file creation
        /// </summary>
        public static string ErrorWhileAuditFileCreation = "Error while creating Audit trail file.";

        /// <summary>
        /// The audit select duration
        /// </summary>
        public static string AuditSelectDuration = "Select a duration to export Audit trails.";

        #endregion Audit Trail

        #region Disk Space

        /// <summary>
        /// The disk space warning MSG
        /// </summary>
        public static string DiskSpaceWarningMsg = "Warning : Disk space almost full. Approximately {0} hours of exam storage available.Please delete unused exams to free up system storage space.";

        /// <summary>
        /// The disk space not checked
        /// </summary>
        public static string DiskSpaceNotChecked = "Error : Could not check disk space.";

        /// <summary>
        /// The disk space full error
        /// </summary>
        public static string DiskSpaceFullError = "Error: Disk space full. Please delete unused exams to free up system storage space.";

        #endregion Disk Space

        #region Log Export

        /// <summary>
        /// The log select duration
        /// </summary>
        public static string LogselectDuration = "Select a duration to export Log files.";

        /// <summary>
        /// The usb no files to transfer
        /// </summary>
        static string UsbNoFilesToTransfer = "No log files to transfer. Please choose a different duration!";

        /// <summary>
        /// The usb no exam file to transfer
        /// </summary>
        public static string UsbNoExamFileToTransfer = "No exam file to transfer.";

        #endregion Log Export

        public const string NotAvailable = "Not Available";

        public const string CreateScaleCVRHorizontalFailed = "CVR Horizontal scale creation failed";
    }
}