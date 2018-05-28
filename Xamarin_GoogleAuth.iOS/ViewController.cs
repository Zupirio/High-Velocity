using System;
using UIKit;
using Xamarin_GoogleAuth.Authentication;
using Xamarin_GoogleAuth.Services;

namespace Xamarin_GoogleAuth.iOS
{
    public partial class ViewController : UIViewController, IGoogleAuthenticationDelegate
    {
        public static GoogleAuthenticator Auth;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Auth = new GoogleAuthenticator(Configuration.ClientId, Configuration.Scope, Configuration.RedirectUrl, this);
			GoogleLoginButton.TouchUpInside += OnGoogleLoginButtonClicked;
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            var authenticator = Auth.GetAuthenticator();
            var viewController = authenticator.GetUI();
            PresentViewController(viewController, true, null);
        }

        public async void OnAuthenticationCompleted(GoogleOAuthToken token)
        {
            DismissViewController(true, null);
			var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(token.TokenType, token.AccessToken);

            GoogleLoginButton.SetTitle($"Connected with {email}", UIControlState.Normal);
        }

        public void OnAuthenticationCanceled()
        {
            DismissViewController(true, null);

            var alertController = new UIAlertController
            {
                Title = "Authentication canceled",
                Message = "You didn't completed the authentication process"
            };
            PresentViewController(alertController, true, null);
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            DismissViewController(true, null);

            var alertController = new UIAlertController
            {
                Title = message,
                Message = exception?.ToString()
            };
            PresentViewController(alertController, true, null);
        }
    }
}