using RecipeApp.Services;
using RecipeApp.Context;
using RecipeApp.Security;
using RecipeApp.Exceptions;
using ReactiveUI;
using System;
using System.Reactive;
using RecipeAppUI.Session;


namespace RecipeAppUI.ViewModels {
    /// <summary>
    /// ViewModel for account deletion
    /// </summary>
    public class DeleteAccountViewModel : ViewModelBase {
        private string _accountDeletionErrorMessage = null!;
        private string _username = null!;
        private string _password = null!;
        private UserService _userService = null!;
        private MainWindowViewModel _mainWindowViewModel = null!;

        public string AccountDeletionErrorMessage {
            get => _accountDeletionErrorMessage;
            set => this.RaiseAndSetIfChanged(ref _accountDeletionErrorMessage, value);
        }

        public string Username {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public string Password {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public UserService UserService {
            get => _userService;
            private set => _userService = value;
        }

        public MainWindowViewModel MainWindowViewModel { 
            get => _mainWindowViewModel; 
            private set => _mainWindowViewModel = value; 
        }

        public ReactiveCommand<Unit, Unit> DeleteAccountCommand { get; } = null!;

        /// <summary>
        /// Constructor for delete account view model
        /// </summary>
        /// <param name="context">For db calls</param>
        /// <param name="mainWindowViewModel">For navigation</param>
        public DeleteAccountViewModel(SplankContext context, MainWindowViewModel mainWindowViewModel){
            UserService = new UserService(context, new PasswordEncrypter());
            MainWindowViewModel = mainWindowViewModel;
            DeleteAccountCommand = ReactiveCommand.Create(DeleteAccount);
        }

        /// <summary>
        /// Deletes a user from the db and redirects to homepage
        /// </summary>
        private void DeleteAccount() {
            try {
                var user = UserService.Login(Username, Password);
                UserService.DeleteAccount(user);
                UserSingleton.NullifyUser();
                MainWindowViewModel.ChangeToHomeView();
            } catch (Exception e) {
                if (e is ArgumentException || e is UserDoesNotExistException || e is InvalidCredentialsException) {
                    AccountDeletionErrorMessage = e.Message;
                } else {
                    AccountDeletionErrorMessage = "Cannot delete account at this time, please try again later.";
                }
            }
        }
    }
}