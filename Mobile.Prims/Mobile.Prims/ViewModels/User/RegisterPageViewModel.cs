using Common.Helpers;
using Common.Models;
using Common.Models.Request;
using Common.Services;

using Mobile.Prims.Helpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile.Prims.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IRegexHelper _regexHelper;
        private readonly IFileHelper _fileHelper;

        private readonly IApiService _apiService;     
        //private readonly IGeolocatorService _geolocatorService;
        private ImageSource _image;
        private UserRequest _user;

        public ObservableCollection<bool> UserTypes { get; set; }

        private bool _isRunning;
        private bool _isEnabled;
        private MediaFile _file;

        private DelegateCommand _changeImageCommand;
        private DelegateCommand _registerCommand;

        public RegisterPageViewModel(INavigationService navigationService,
                                    IRegexHelper regexHelper,
                                    IApiService apiService,
                                    IFileHelper fileHelper) : base(navigationService)
        {
            _navigationService = navigationService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            _fileHelper = fileHelper;
            //_geolocatorService = geolocatorService;
            Title = "Registrar usuario";
            Image = App.Current.Resources["UrlNoImage"].ToString();
            IsEnabled = true;
            User = new UserRequest();
            UserTypes = new ObservableCollection<bool>() { true, false };
            //LoadCountriesAsync();

        }


        public DelegateCommand ChangeImageCommand => _changeImageCommand ??
           (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public DelegateCommand RegisterCommand => _registerCommand ??
            (_registerCommand = new DelegateCommand(RegisterAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public UserRequest User
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


        private async void RegisterAsync()
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
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.ConnectionError", "Languages.Accept");
                return;
            }

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _fileHelper.ReadFully(_file.GetStream());
            }

            //await _geolocatorService.GetLocationAsync();
            //if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
            //{
            //    User.Latitude = _geolocatorService.Latitude;
            //    User.Logitude = _geolocatorService.Longitude;
            //}

            User.ImageArray = imageArray;
            //User.CityId = City.Id;

            // Estado del usuario 
            User.UserStatus = "Registrado";

            // Tipo de usuario
            User.UserType = (UserTypes[0] == true && UserTypes[1] == false) ? "Cuidador" : "Cuidado";
        
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.RegisterUserAsync(url, "/api", "/Account/Register", User);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error")
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un problema, intente más tarde.", "Aceptar");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                }

                return;
            }

            await App.Current.MainPage.DisplayAlert("Confirmación", "El usuario se ha registrado correctamente, se envió un correo electronico de confirmación", "Aceptar");
            await _navigationService.GoBackAsync();
        }


        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                "Languages.PictureSource",
                "Cancelar",
                null,
                "Galería",
                "Cámara");

            if (source == "Cancelar")
            {
                _file = null;
                return;
            }

            if (source == "Cámara")
            {
                if (!CrossMedia.Current.IsCameraAvailable)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "NoCameraSupported", "Aceptar");
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
                    await App.Current.MainPage.DisplayAlert("Error", "NoGallerySupported", "Aceptar");
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



        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.FirstNameError", "Languages.Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.LastNameError", "Languages.Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.Address))
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.AddressError", "Languages.Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.Email) || !_regexHelper.IsValidEmail(User.Email))
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.EmailError", "Languages.Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.PhoneNumber))
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.PhoneError", "Languages.Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.Password) || User.Password?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.PasswordError", "Languages.Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.PasswordConfirmError1", "Languages.Accept");
                return false;
            }

            if (User.Password != User.PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert("Languages.Error", "Languages.PasswordConfirmError2", "Languages.Accept");
                return false;
            }

            return true;
        }


    }
}
