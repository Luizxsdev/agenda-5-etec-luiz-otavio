using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    // ObservableCollection: coleção que atualiza automaticamente a interface
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        // Define que a ListView exibirá os itens da lista
        lst_produtos.ItemsSource = lista;
    }

    // Método executado sempre que a tela aparece
    protected async override void OnAppearing()
    {
        try
        {
            // Limpa a lista para evitar duplicados
            lista.Clear();

            // Busca todos os produtos do banco de dados
            List<Produto> tmp = await App.Db.GetAll();

            // Adiciona os produtos na lista exibida
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            // Caso dê erro, exibe mensagem amigável
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Botão "Adicionar": abre a tela de cadastro de produto
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Busca dinâmica: chamada sempre que o texto da SearchBar mudar
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            // Limpa a lista antes de adicionar resultados filtrados
            lista.Clear();

            // Pesquisa no banco
            List<Produto> tmp = await App.Db.Search(q);

            // Atualiza a lista com os resultados
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Botão "Somar": calcula o total dos produtos
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Soma os valores de todos os itens
        double soma = lista.Sum(i => i.Total);

        // Exibe o resultado em uma mensagem
        string msg = $"O total é {soma:C}";
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    // Menu de contexto: Remover produto
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Recupera o item que foi selecionado
            MenuItem selecinado = sender as MenuItem;
            Produto p = selecinado.BindingContext as Produto;

            // Pergunta ao usuário se deseja excluir
            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if(confirm)
            {
                // Remove do banco e da lista
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento de seleção: abre a tela de edição
    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            // Navega para tela EditarProduto, enviando o produto selecionado
            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}