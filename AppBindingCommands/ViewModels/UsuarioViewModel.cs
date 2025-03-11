using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

public class UsuarioViewModel : INotifyPropertyChanged
{
    
    private string name = string.Empty;
    public string DisplayName => $"Nome digitado : {Name}";
    string displayMessage = string.Empty;


    public event PropertyChangedEventHandler PropertyChanged;

    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

   

    public string Name { get => name; set { if (name == null) return; name = value; OnPropertyChanged(nameof(name)); OnPropertyChanged(nameof(DisplayName)); } }

    public string DisplayMessage { get => displayMessage; set{ if (displayMessage == null) return; displayMessage = value; OnPropertyChanged(nameof(DisplayMessage)); } }

    public ICommand ShowMessageCommand { get; }
    public ICommand CountCommand { get; }
    public ICommand CleanCommand { get; }
    public ICommand OptionCommand { get; }

    public void ShowMessage()
    {
        DateTime data = Preferences.Get("dtAtual", DateTime.MinValue);
        DisplayMessage = $"Boa noite {name}, Hoje é {data}";
    }

    public UsuarioViewModel()
    {
        ShowMessageCommand = new Command(ShowMessage);
        CountCommand = new Command(async () => await CountCharacters());
        CleanCommand = new Command(async () => await CleanConfirmation());
        OptionCommand = new Command(async () => await ShowOptions());
    }

    public async Task CountCharacters()
    {
        string nameLenght = string.Format("Seu nome tem {0} letras.", name.Length);
        await Application.Current.MainPage.DisplayAlert("Informação", nameLenght, "OK");
    }

    public async Task CleanConfirmation()
    {
        if(await Application.Current.MainPage.DisplayAlert("Confirmação", "Confirma limpeza dos dados?", "Sim", "Não")) 
        {
            Name = string.Empty;
            DisplayMessage = string.Empty;
            OnPropertyChanged(Name);
            OnPropertyChanged(DisplayMessage);

            await Application.Current.MainPage.DisplayAlert("Informação", "Limpeza realizada com sucesso!", "Ok");
        }
    }

    public async Task ShowOptions()
    {
        string result = await Application.Current.MainPage.DisplayActionSheet("Selecione uma opção: ", "", "Cancelar", "Limpar", "Contar Caracteres", "Exibir Saudação");

        if(result != null)
        {
            if (result.Equals("Limpar"))
                await CleanConfirmation();
            if (result.Equals("Contar Caracteres"))
                await CountCharacters();
            if (result.Equals("Exibir Saudação"))
                ShowMessage();
        }
    }
}
