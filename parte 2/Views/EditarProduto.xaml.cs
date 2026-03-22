using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    // Botão "Salvar": atualiza os dados do produto
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Recupera o produto enviado como BindingContext
            Produto produto_anexado = BindingContext as Produto;

            // Cria um novo objeto com os dados editados
            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Atualiza o banco de dados
            await App.Db.Update(p);

            // Mostra mensagem de sucesso
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");

            // Volta para a tela anterior
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            // Caso dê erro, mostra mensagem amigável
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}