using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MyDigitalWardrobe.Services
{
    public static class FireBaseService
    {
        private static readonly string authKey = "AIzaSyB7wv7xyG148I_XUDv8OvNn-iQE1fjpbp4";
        private static FirebaseAuth _currentAuthInformation;
        public static FirebaseAuth CurrentUserInformation
        {
            get
            {
                if (_currentAuthInformation == null)
                {
                    _currentAuthInformation = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                }
                return _currentAuthInformation;
            }
        }
        /// <summary>
        /// Attempts to create a new user using the provided credentials.
        /// </summary>
        /// <param name="email">Email of the new user</param>
        /// <param name="password">Password of the new user</param>
        /// <returns>Result Object containing { Status, AuthToken, ErrorMessage}</returns>
        public static async Task<Result> RegisterNewUserAsync(string email, string password)
        { 
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                await LoginWithCredentialsAsync(email, password);
                return new Result
                {
                    Status = Status.Success,
                };
            }
            catch (FirebaseAuthException ex)
            {
                return new Result
                {
                    Status = Status.Error,
                    ErrorMessage = FormatExceptionReason(ex.Reason.ToString())
                };
            }
        }

        /// <summary>
        /// Attempts to login a user using the provided credentials.
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>Result Object containing { Status, AuthToken, ErrorMessage}</returns>
        public static async Task<Result> LoginWithCredentialsAsync(string email, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                var content = await auth.GetFreshAuthAsync();
                _currentAuthInformation = content;
                var serialToken = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serialToken);
                return new Result
                {
                    Status = Status.Success,
                };
            }
            catch (FirebaseAuthException ex)
            {
                return new Result
                {
                    Status = Status.Error,
                    ErrorMessage = FormatExceptionReason(ex.Reason.ToString())
                };
            }
        }

        /// <summary>
        /// Attempts to Refresh the current users auth token.
        /// </summary>
        /// <returns>Result Object containing { Status, AuthToken, ErrorMessage}</returns>
        public static async Task<Result> RefreshAuthTokenAsync()
        {
            if (Preferences.Get("MyFirebaseRefreshToken", "") == "")
            {
                return new Result
                {
                    Status = Status.Error,
                    ErrorMessage = "No Refresh Token Found"
                };
            }
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(authKey));
                var oldAuthToken = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(oldAuthToken);
                var serialToken = JsonConvert.SerializeObject(refreshedContent);
                Preferences.Set("MyFirebaseRefreshToken", serialToken);
                return new Result
                {
                    Status = Status.Success,
                };
            }
            catch (FirebaseAuthException ex)
            {
                return new Result
                {
                    Status = Status.Error,
                    ErrorMessage = FormatExceptionReason(ex.Reason.ToString())
                };
            }
        }

        /// <summary>
        /// Removes the FireBaseRefeshToken, 
        /// And also removes the current user's information from the app.
        /// </summary>
        /// <returns></returns>
        public static void ClearAuth()
        {
            _currentAuthInformation = null;
            Preferences.Remove("MyFirebaseRefreshToken");
        }

        
        /// <summary>
        /// Formats the FireBaseException's into a more readable string. 
        /// Using spaces rather than cammelCase.
        /// </summary>
        /// <param name="unformattedReason">FireBaseException's reason</param>
        /// <returns></returns>
        private static string FormatExceptionReason(string unformattedReason)
        {
            string reason = string.Empty;
            foreach (char letter in unformattedReason)
            {
                reason += char.IsUpper(letter) ? " " + letter : letter.ToString();
            }
            return reason;
        }

        public enum Status
        {
            Success,
            Error
        }

        public struct Result
        {
            public Status Status { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
