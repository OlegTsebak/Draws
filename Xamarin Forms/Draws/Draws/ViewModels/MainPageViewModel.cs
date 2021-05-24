using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Draws.Helpers;
using Draws.Helpers.Enums;
using Draws.Models;
using Draws.Services;
using Draws.ViewModels.Bases;
using Prism.Navigation;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace Draws.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        
        private readonly IDataTransferService _dataTransferService;

        private string _userName;

        private ConnectStatusType _connectStatus;

        private User _selectedUser;
        
        protected MainPageViewModel(
            INavigationService navigationService,
            IDataTransferService dataTransferService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dataTransferService = dataTransferService;

            _dataTransferService.ActiveUsersReceived += OnActiveUsersReceived;
            _dataTransferService.RequestReceived += OnRequestReceived;
            _dataTransferService.RequestAccepted += OnRequestAccepted;
            
            SendMessageCommand = new AsyncCommand(async () => await OnSendMessage(), allowsMultipleExecutions: false);
            ConnectCommand = new AsyncCommand(async () => await OnConnect(), allowsMultipleExecutions: false);
            DisconnectCommand = new AsyncCommand(async () => await OnDisconnect(), allowsMultipleExecutions: false);
            GetUsersCommand = new AsyncCommand(async () => await OnGetUsers(), allowsMultipleExecutions: false);

            ConnectStatus = ConnectStatusType.Connecting;
        }

        public IAsyncCommand SendMessageCommand { get; set; }
        
        public IAsyncCommand ConnectCommand { get; set; }
        
        public IAsyncCommand DisconnectCommand { get; set; }
        
        public IAsyncCommand GetUsersCommand { get; set; }
        
        public ObservableCollection<User> ActiveUsers { get; set; }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged();
            }
        }
        
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;

                if (_selectedUser != null)
                    ConnectToSelectedUser(_selectedUser);
                
                _selectedUser = null;
                RaisePropertyChanged();
            }
        }
        
        public string ConnectStatusText => ConnectStatus.ToString();
        
        private ConnectStatusType ConnectStatus
        {
            get => _connectStatus;
            set
            {
                _connectStatus = value;
                RaisePropertyChanged(nameof(ConnectStatusText));
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            
            UserName = Preferences.Get(Constants.UserName, "");
            _dataTransferService.CurrentUser.UserName = UserName;

            _dataTransferService.ActiveUsersReceived += OnActiveUsersReceived;

            if (await _dataTransferService.Connect())
            {
                await _dataTransferService.ConnectUser();
                ConnectStatus = ConnectStatusType.Connected;
            }
            else
                ConnectStatus = ConnectStatusType.Error;
        }

        private async Task OnSendMessage()
        {
            //_dataTransferService.Send(message);
        }
        
        private async Task OnConnect()
        {
            await _dataTransferService.ConnectUser();
        }
        
        private async Task OnDisconnect()
        {
            await _dataTransferService.DisconnectUser();
        }

        private async Task OnGetUsers()
        {
            await _dataTransferService.GetUsers();
        }

        private async void ConnectToSelectedUser(User user)
        {
            var spareRequest = new SpareRequest
            {
                Owner = _dataTransferService.CurrentUser, 
                Receiver = user
            };
            await _dataTransferService.ConnectToUser(spareRequest);
        }

        private void OnActiveUsersReceived(IEnumerable<User> users)
        {
            ActiveUsers = new ObservableCollection<User>(users);
            RaisePropertyChanged(nameof(ActiveUsers));
        }
        
        private async void OnRequestReceived(User user)
        {
            var result = await App.Current.MainPage.DisplayAlert("Request received",
                $"User {user.UserName} with id {user.Id} want to connect to you", 
                "Connect", 
                "Cancel");

            if (result)
            {
                _dataTransferService.ConnectedUser = user;
                await _dataTransferService.ConnectToUserAccepted(user.Id);
                await _navigationService.NavigateAsync("draw-page");
            }
        }
        
        private async void OnRequestAccepted(string userId)
        {
            await _navigationService.NavigateAsync("draw-view-page");
        }
    }
}