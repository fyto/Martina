using Common.Helpers;
using Common.Models;
using Common.Models.Request;
using Common.Models.Responses;
using Common.Services;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile.Prims.ViewModels
{
    public class EditUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFileHelper _fileHelper;
        private ImageSource _image;
        private UserResponse _user;

        public ObservableCollection<bool> UserTypes { get; set; }

        private bool _isRunning;
        private bool _isEnabled;
        private bool _isOnSaleUser;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;

        public EditUserPageViewModel(INavigationService navigationService,
                                     IApiService apiService,
                                     IFileHelper fileHelper) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _fileHelper = fileHelper;

            UserTypes = new ObservableCollection<bool>();

            IsEnabled = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            User = token.User;
            Image = User.ImageFullPath;

            LoadUserType(User);

            //IsOnSaleUser = User.LoginType == LoginType.OnSale;

        }


        public DelegateCommand ChangeImageCommand => _changeImageCommand ??
          (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand SaveCommand => _saveCommand ??
            (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ??
            (_changePasswordCommand = new DelegateCommand(ChangePasswordAsync));

        public bool IsOnSaleUser
        {
            get => _isOnSaleUser;
            set => SetProperty(ref _isOnSaleUser, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        private void LoadUserType(UserResponse user)
        {

            // Tipo de usuario
            if (User.UserType == "Cuidador")
            {
                UserTypes.Add(true);
                UserTypes.Add(false);
            }
            else
            {
                UserTypes.Add(false);
                UserTypes.Add(true);
            }
        }

        private async void ChangeImageAsync()
        {
            if (!IsOnSaleUser)
            {
                await App.Current.MainPage.DisplayAlert("Error", "hangeOnSocialNetwork", "Accept");
                return;
            }

            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                "PictureSource",
                "Cancel",
                null,
                "FromGallery",
                "FromCamera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "FromCamera")
            {
                if (!CrossMedia.Current.IsCameraAvailable)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "NoCameraSupported", "Accept");
                    return;
                }

                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "NoGallerySupported", "Accept");
                    return;
                }

                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                Image = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }
        }

        private async void SaveAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "ConnectionError", "Accept");
                return;
            }

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _fileHelper.ReadFully(_file.GetStream());
            }

            UserRequest request = new UserRequest
            {
                Address = User.Address,
                Email = User.Email,
                FirstName = User.FirstName,
                ImageArray = imageArray,
                LastName = User.LastName,
                Password = "123456", // Doen't matter, it's only to pass the data annotation
                PhoneNumber = User.PhoneNumber
            };

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.ModifyUserAsync(url, "/api", "/Account", request, token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error001")
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error001", "Accept");
                }
                else if (response.Message == "Error004")
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error004", "Accept");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                }

                return;
            }

            UserResponse updatedUser = (UserResponse)response.Result;
            token.User = updatedUser;
            Settings.Token = JsonConvert.SerializeObject(token);
            AppMasterDetailPageViewModel.GetInstance().LoadUser();
            await App.Current.MainPage.DisplayAlert("Ok", "ChangeUserMessage", "Accept");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "FirstNameError", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "LastNameError", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.Address))
            {
                await App.Current.MainPage.DisplayAlert("Error", "AddressError", "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.PhoneNumber))
            {
                await App.Current.MainPage.DisplayAlert("Error", "PhoneError", "Accept");
                return false;
            }

            return true;
        }

        private async void ChangePasswordAsync()
        {
            if (!IsOnSaleUser)
            {
                await App.Current.MainPage.DisplayAlert("Error", "ChangeOnSocialNetwork", "Accept");
                return;
            }

            //await _navigationService.NavigateAsync(nameof(ChangePasswordPage));
        }


    }
}
