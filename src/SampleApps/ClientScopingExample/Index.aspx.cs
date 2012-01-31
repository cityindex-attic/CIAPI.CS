using System;
using CIAPI.DTO;

namespace ClientScopingExample
{
    public partial class Index : RpcClientPage
    {
        private void ManagePanels()
        {
            LogInPanel.Visible = !LoggedIn;
            LoggedInPanel.Visible = LoggedIn;
            ChangePasswordButton.Visible = LoggedIn;
            
        }

        protected override void PageLoad(object sender, EventArgs e)
        {
            ManagePanels();
            DisplayError("");
        }

        private void DisplayError(string message)
        {
            ErrorMessage.Visible = !string.IsNullOrEmpty(message);
            ErrorMessage.Text = message;
        }

        protected void LoginButtonClick(object sender, EventArgs e)
        {
            ApiLogOnResponseDTO response = null;
            try
            {
                response = SessionClient.LogIn(UserNameTextBox.Text, PasswordTextBox.Text);
                if (response.PasswordChangeRequired)
                {
                    DisplayError("You must change your password.");
                    LoggedInPanel.Visible = false;
                    ChangePasswordPanel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }

            ManagePanels();
        }

        protected void ChangePasswordButtonClick(object sender, EventArgs e)
        {
            if (ChangePasswordPanel.Visible)
            {
                try
                {
                    ApiChangePasswordResponseDTO response =
                        SessionClient.Authentication.ChangePassword(new ApiChangePasswordRequestDTO
                                                                        {
                                                                            NewPassword =
                                                                                ChangePasswordNewPasswordTextBox.Text,
                                                                            Password =
                                                                                ChangePasswordOldPasswordTextBox.Text,
                                                                            UserName = SessionClient.UserName
                                                                        });
                    if (!response.IsPasswordChanged)
                    {
                        DisplayError("password not changed. please try again.");
                    }
                    else
                    {
                        ChangePasswordPanel.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    DisplayError(ex.Message);
                }
            }
            else
            {
                ChangePasswordPanel.Visible = true;
                LoggedInPanel.Visible = false;
            }
        }

        protected void GetMarketInfoButtonClick(object sender, EventArgs e)
        {
            try
            {
                var response = SessionClient.Market.GetMarketInformation(MarketInfo.Text);
                // display useful information
                MarketInfo.Text = response.MarketInformation.MarketId.ToString();
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
                
            }

        }
    }
}