using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Utilities.Constants
{
    /// <summary>
    /// Represents the errors.
    /// </summary>
    public class Errors
    {
        public static ErrorInfo ServerConnectFailure = new ErrorInfo("SERVER_CONNECT_FAILURE", "Server connection is failure.");
        public static ErrorInfo ServerConnectTimeout = new ErrorInfo("SERVER_CONNECT_TIMEOUT", "Server connection is timeout.");
        public static ErrorInfo AuthFailure = new ErrorInfo("AUTH_FAILURE", "Authentication failure.");
        public static ErrorInfo ErrorAndLoginFailure = new ErrorInfo("ERROR_AND_LOGIN_FAILURE", "Login failure with error.");
        public static ErrorInfo UserNameRequired = new ErrorInfo("USERNAME_REQUIRED", "Username is required.");
        public static ErrorInfo UserNameInvalid = new ErrorInfo("USERNAME_INVALID", "Username is invalid.");
        public static ErrorInfo PasswordRequired = new ErrorInfo("PASSWORD_REQUIRED", "Password is required.");
        public static ErrorInfo PasswordInvalid = new ErrorInfo("PASSWORD_INVALID", "Passowrd is invalid.");
        public static ErrorInfo PasswordIncorrect = new ErrorInfo("PASSWORD_INCORRECRT", "Passowrd is not correct.");
        public static ErrorInfo UserNameOrPasswordIncorrect = new ErrorInfo("USERNAME_OR_PASSWORD_INCORRECRT", "Username or password is incorrect.");

        public static ErrorInfo OldPasswordRequired = new ErrorInfo("OLD_PASSWORD_REQUIRED", "Old password is required.");
        public static ErrorInfo OldPasswordInvalid = new ErrorInfo("OLD_PASSWORD_INVALID", "Old password is invalid.");
        public static ErrorInfo OldPasswordIncorrect = new ErrorInfo("OLD_PASSWORD_INCORRECRT", "Old password is incorrect.");
        public static ErrorInfo NewPasswordRequired = new ErrorInfo("NEW_PASSWORD_REQUIRED", "New password is required.");
        public static ErrorInfo NewPasswordInvalid = new ErrorInfo("NEW_PASSWORD_INVALID", "New password is invalid.");
        public static ErrorInfo ConfirmPasswordRequired = new ErrorInfo("CONFIRM_PASSWORD_REQUIRED", "Confirm password is required.");
        public static ErrorInfo ConfirmPasswordNotMatch = new ErrorInfo("CONFIRM_PASSWORD_REQUIRED", "Confirm password is not match.");
        public static ErrorInfo ChangePasswordFailure = new ErrorInfo("CHANGE_PASSWORD_FAILURE", "Change password is failure.");

        public static ErrorInfo VerifyCodeRequired = new ErrorInfo("VERIFY_CODE_REQUIRED", "Verify code is required.");
        public static ErrorInfo VerifyCodeInvalid = new ErrorInfo("VERIFY_CODE_INVALID", "Verify code is invalid.");
        public static ErrorInfo AuthCodeRequired = new ErrorInfo("AUTH_CODE_REQUIRED", "Authention code is required.");
        public static ErrorInfo AuthCodeInvalid = new ErrorInfo("AUTH_CODE_INVALID", "Authention code is invalid.");

        public static ErrorInfo RecordNotFound = new ErrorInfo("RECORD_NOT_FOUND", "No record has been found.");
        public static ErrorInfo UserNameDuplicated = new ErrorInfo("USERNAME_DUPLICATED", "The username cannot duplicate.");
        public static ErrorInfo UserNameExistAlready = new ErrorInfo("USERNAME_EXIST_ALREADY", "The username has exist already.");
        public static ErrorInfo UserNameNotExist = new ErrorInfo("USERNAME_NOT_EXIST", "The username doesn't exist.");

    }
}
